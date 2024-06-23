using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

namespace InformationUtils
{
    internal class SIDUtils
    {
        //Well known SIDs
        //https://renenyffenegger.ch/notes/Windows/security/SID/index
        //https://docs.microsoft.com/en-Us/windows-server/identity/ad-ds/manage/understand-security-identifiers
        //https://docs.microsoft.com/en-us/windows/win32/secauthz/well-known-sids
        //https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-dtyp/81d92bba-d22b-4a8c-908a-554ab29148ab?redirectedfrom=MSDN
        //https://mskb.pkisolutions.com/kb/867462

        internal const string s_Builtin_Administrators= "S-1-5-32-544";
        internal const string s_CREATOR_OWNER= "S-1-3-0";
        internal const string s_NT_AUTHORITY_SYSTEM = "S-1-5-18";

        internal const string s_NT_SERVICE_TRUSTEDINSTALLER = "S-1-5-80-956008885-3418522649-1831038044-1853292631-2271478464";

        internal const string s_ALL_APP_PACKAGES = "S-1-15-2-1";

        internal const string s_Builtin_NetworkConfigurationOperators = "S-1-5-32-556";
        internal const string s_Builtin_BackupOperators = "S-1-5-32-551";
        internal const string s_Builtin_RemoteDesktopUsers = "S-1-5-32-555";
        internal const string s_Builtin_ServerOperators = "S-1-5-32-549";
        internal const string s_Builtin_PowerUsers = "S-1-5-32-547";
        internal const string s_Builtin_StorageReplicaAdministrators = "S-1-5-32-582";

        //CREATOR OWNER usually is a privileged account. Nevertheless, we prefer replacing CREATOR OWNER by corresponding account for a more precise analysis during the code
        //We do not include in this list to identify possible paths where we have not replaced it by corresponding account.

        //Some services are managed by Builtin_NetworkConfigurationOperators. So, we consider it as a trusted account
        private static List<string> s_PrivilegedSIDs = new List<string> { s_Builtin_Administrators, s_NT_AUTHORITY_SYSTEM, s_NT_SERVICE_TRUSTEDINSTALLER, s_ALL_APP_PACKAGES, s_Builtin_NetworkConfigurationOperators };

        private static List<string> s_nullSIDS = new List<string>();

        internal static string GetSidValue(string user)
        {
            //Some commands retrieve sometimes SIDs instead user names
            if(user.StartsWith("S-1-"))
            {
                return user;
            }
            //We use the SID obtained in the files if we have obtained them during the extraction. Sometimes Windows is not able to map some well know accounts with the Translate .NET function, as ALL_APP_PACKAGES
            SecurityIdentifier sid = (SecurityIdentifier)(new NTAccount(user)).Translate(typeof(SecurityIdentifier));
            return sid.Value;
        }

        internal static bool IsPrivilegedSID(string sidValue)
        {
            //We consider that all service accounts are privileged.
            if(sidValue.StartsWith("S-1-5-80"))
            {
                return true;
            }
            foreach (string privilegedSID in s_PrivilegedSIDs)
            {
                if (sidValue == privilegedSID)
                {
                    return true;
                }
            }

            return false;
        }


        internal static string TranslatSIDToUser(string sidValue)
        {
            string user = null;
            try
            {
                //We use this "cache" list to save time when SID is not found. Trying to translate an unkown SID consumes a lot time each time:
                if(s_nullSIDS.Contains(sidValue))
                {
                    return null;
                }

                //SID is translated in this function, but it does not work if it is translated from Security Identifier to NT account...
                user = new SecurityIdentifier(sidValue).Translate(typeof(NTAccount)).ToString();
            }
            catch
            {
                user = TranslateWellKnownSID(sidValue);
                if(user==null)
                {
                    s_nullSIDS.Add(sidValue);
                }
            }
            return user;
        }


        internal static string TranslateWellKnownSID(string sidValue)
        {
            //Sometimes Windows is not able to map some well know accounts with the Translate .NET function, as ALL_APP_PACKAGES, Backup Operators
            //We include all SID that we have collectaed as not mapped or collectect by other reasons.
            //https://docs.microsoft.com/en-Us/windows/win32/secauthz/well-known-sids
            //https://mskb.pkisolutions.com/kb/867462
            switch (sidValue)
            {
                case s_Builtin_Administrators:
                    return @"BUILTIN\Administrators";
                case s_CREATOR_OWNER:
                    return @"CREATOR OWNER";
                case s_NT_AUTHORITY_SYSTEM:
                    return @"NT Authority\System";
                case s_NT_SERVICE_TRUSTEDINSTALLER:
                    return @"NT Service\TrustedInstaller";
                case s_ALL_APP_PACKAGES:
                    return @"APPLICATION PACKAGE AUTHORITY\ALL APPLICATION PACKAGES";
                case s_Builtin_NetworkConfigurationOperators:
                    return @"BUILTIN\Network Configuration Operators";
                case s_Builtin_BackupOperators:
                    return @"BUILTIN\Backup Operators";
                case s_Builtin_RemoteDesktopUsers:
                    return @"BUILTIN\Remote Desktop Users";
                case s_Builtin_ServerOperators:
                    return @"BUILTIN\Server Operators";
                case s_Builtin_PowerUsers:
                    return @"BUILTIN\Power Users";
                case s_Builtin_StorageReplicaAdministrators:
                    return @"BUILTIN\Storage Replica Administrators";
                default:
                    return null;
            }
        }


        internal static string[] TranslateSIDsInLines(string[] lines)
        {
            int length=lines.Length;
            string[] replacedLines=new string[length];
            for(int i=0;i<length;i++)
            {
                string line = lines[i];
                string replacedLine = ReplaceLine(line);
                replacedLines[i] = replacedLine;
            }
            return replacedLines;
        }

        private static string ReplaceLine(string line)
        {
            int pos = line.IndexOf("*S-1");
            while (pos >= 0)
            {
                //We search after *S-1 token
                int posCommma = line.IndexOf(",", pos);
                //This case would mean last SID token
                if (posCommma < 0)
                {
                    string SID = line.Substring(pos);
                    string translatedSID = TranslatSIDToUser(SID.Replace(@"*", ""));
                    if (translatedSID != null)
                    {
                        line = line.Replace(SID, translatedSID);
                    }
                    return line;
                }
                else
                {
                    int SIDlength = posCommma - pos;
                    string SID = line.Substring(pos, SIDlength);
                    string translatedSID = TranslatSIDToUser(SID.Replace(@"*", ""));
                    if (translatedSID != null)
                    {
                        line = line.Replace(SID, translatedSID);
                    }
                    //We serach next *S-1 after current token
                    pos = line.IndexOf("*S-1", pos+1);
                }

            }
            return line;
        }

    }
}
