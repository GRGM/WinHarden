using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace InformationUtils
{
    public class Permission
    {
        //        User: BUILTIN\Users
        //Type: Allow
        //Rights: ReadKey
        //Inheritance: None
        //Propagation: None
        //Inherited? True

        private string m_user = null;
        private string m_SID = null;
        private string m_type=null;
        private string m_rights=null;
        private string m_inheritance=null;        
        private string m_propagation=null;
        private string m_inherited=null;

        //We do not include Delete and DeleteSubdirectoriesAndFiles because we consider that only with such privileges is hard to generate a relevant security risk as escalation privileges...
        //private static List<string> s_writingPermissions = new List<string> { "WriteData".ToUpper(), "CreateFiles".ToUpper(), "Write".ToUpper(), "Delete".ToUpper(), "Modify".ToUpper(), "FullControl".ToUpper(), "ChangePermissions".ToUpper(), "TakeOwnership".ToUpper(), "GENERIC_ALL", "GENERIC_WRITE", "KEY_SET_VALUE", "KEY_CREATE_SUB_KEY", "KEY_WRITE", "KEY_ALL_ACCESS", "DeleteSubdirectoriesAndFiles".ToUpper(), "SERVICE_CHANGE_CONFIG", "SERVICE_ALL_ACCESS" };
        private static List<string> s_writingPermissions = new List<string> { "WriteData".ToUpper(), "CreateFiles".ToUpper(), "Write".ToUpper(), "Modify".ToUpper(), "FullControl".ToUpper(), "ChangePermissions".ToUpper(), "TakeOwnership".ToUpper(), "GENERIC_ALL", "GENERIC_WRITE", "KEY_SET_VALUE", "KEY_CREATE_SUB_KEY", "KEY_WRITE", "KEY_ALL_ACCESS", "SERVICE_CHANGE_CONFIG", "SERVICE_ALL_ACCESS" };



        //We ignore SID that OS has not been able to resolve. They may be removed accounts or capability SID
        //https://docs.microsoft.com/en-us/troubleshoot/windows-server/windows-security/sids-not-resolve-into-friendly-names
        //https://docs.microsoft.com/en-us/windows/security/identity-protection/access-control/special-identities

        private static List<string> s_startServicePermissions = new List<string> { "SERVICE_START", "SERVICE_STOP", "SERVICE_PAUSE_CONTINUE" };      


        internal Permission(string user, string SID, string type, string rights, string inheritance, string propagation, string inherited)
        {
            m_user = user.ToUpper();
            m_SID = SID;
            m_type = type;
            m_rights = rights.ToUpper();
            m_inheritance = inheritance;
            m_propagation = propagation;
            m_inherited = inherited;
        }

        internal Permission(string user, string SID, string type, string rights)
        {
            m_user = user.ToUpper();
            m_SID = SID;
            m_type = type;
            m_rights = rights.ToUpper();
        }


        public static void ProcessPathsPermission(string inputFileName, string outputFilename,string header)
        {
            ProcessPathsPermission(inputFileName, outputFilename, header, null);
        }


        public static void ProcessPathsPermission(string inputFileName, string outputFilename, string header, ProfileAccountUtils profileAccountUtils)
        {
            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            string objectToReview = "";
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();
                if (line == "")
                {
                    continue;
                }                
                if (line.StartsWith(header))
                {
                    objectToReview = inputFile.ReadLine().Trim();
                    continue;
                }
                if (line.StartsWith("User:"))
                {
                    if (profileAccountUtils == null)
                    {
                        ProcessPermission(line, inputFile, outputFile, objectToReview);
                    }
                    else
                    {
                        if(!profileAccountUtils.IsUserProfileFolderSelfAccessed(objectToReview,line))
                        {
                            ProcessPermission(line, inputFile, outputFile, objectToReview);
                        }
                    }
                }
            }
            inputFile.Close();
            outputFile.Close();
        }

        public static Permission GetPermission(string line, StreamReader inputFile)
        {
            string user = line.Replace("User: ", "").Trim();
            string sidValue = inputFile.ReadLine().Replace("SID: ", "").Trim();
            string type = inputFile.ReadLine().Replace("Type: ", "").Trim();
            string rights = inputFile.ReadLine().Replace("Rights: ", "").Trim();
            string inheritance = inputFile.ReadLine().Replace("Inheritance: ", "").Trim();
            string propagation = inputFile.ReadLine().Replace("Propagation: ", "").Trim();
            string inherited = inputFile.ReadLine().Replace("Inherited?", "").Trim();

            Permission permission = new Permission(user, sidValue, type, rights, inheritance, propagation, inherited);
            return permission;
        }


        public static void ProcessPermission(string line, StreamReader inputFile, StreamWriter outputFile, string objectToReview)
        {
            //This funciotn may be used for files, folder and registry protection.
            Permission permission = GetPermission(line, inputFile);
            bool isUnsecurePermission = permission.IsUnsecurePermission();
            if (isUnsecurePermission)
            {
                outputFile.WriteLine(objectToReview);
                outputFile.WriteLine(permission.GetProperties());
                outputFile.WriteLine();
            }
        }

        public static void ProcessServicePermission(string line, StreamReader inputFile, StreamWriter outputFile, string objectToReview, List<string> writableServices)
        {
            string user = line.Replace("User: ", "");
            string SID = inputFile.ReadLine().Replace("SID:", "").Trim();
            string type = inputFile.ReadLine().Replace("Type:", "").Trim();
            string rights = inputFile.ReadLine().Replace("Rights:", "").Trim();

            Permission permission = new Permission(user, SID, type, rights);
            string serviceName = objectToReview.Replace("Security of service or driver:","").Trim();
            int position=serviceName.IndexOf(" ");
            serviceName=serviceName.Substring(0, position);


            bool isUnsecurePermission = permission.IsUnsecureServicePermission(serviceName,writableServices);
            if (isUnsecurePermission)
            {
                outputFile.WriteLine(objectToReview);
                outputFile.WriteLine(permission.GetProperties());
                outputFile.WriteLine();
            }
        }
        
        internal static void ProcessStartUpPermission(string line, StreamReader inputFile, StreamWriter outputFile, string objectToReview)
        {
            string user = line.Replace("User: ", "");
            Permission permission = GetPermission(line, inputFile);

            int position = objectToReview.LastIndexOf(",");
            string startUpUser = objectToReview.Substring(position+1);

            bool isUnsecurePermission = permission.IsUnsecurePermission();
            //We consider that a task run modified by the same user in which is running is not a risk.
            if (isUnsecurePermission && startUpUser!= user)
            {
                outputFile.WriteLine(objectToReview);
                outputFile.WriteLine(permission.GetProperties());
                outputFile.WriteLine();
            }
        }

        internal bool IsUnsecureServicePermission(string serviceName, List<string> writableServices)
        {
            string sidValue = m_SID;
            //We use the SID obtained in the files if we have obtained them during the extraction. Sometimes Windows is not able to map some well know accounts with the .NET function, as ALL_APP_PACKAGES
            if (sidValue is null)
            {
                sidValue = SIDUtils.GetSidValue(m_user);
            }
            if (m_type == "Allow")
            {
                //First we check if it is privileged account. In this case, it is not a unsercure service permission
                bool isPrivileged = SIDUtils.IsPrivilegedSID(sidValue);
                if(isPrivileged)
                {
                    return false;
                }

                //We check if not privileged account has access to writing permission in service

                // https://docs.microsoft.com/en-us/windows/win32/secauthz/access-mask-format
                //https://www.ntfs.com/ntfs-permissions-file-advanced.htm
                foreach (string writingPermission in s_writingPermissions)
                {
                    if (m_rights.Contains(writingPermission.ToUpper()))
                    {
                        return true;
                    }
                }

                //We check if service path may be writable by non priviiged accounts. If not, we returno false.
                if(!writableServices.Contains(serviceName))
                {
                    return false;
                }

                // lastly We check if not privielged account has access on "SERVICE_START", "SERVICE_STOP" or "SERVICE_PAUSE_CONTINUE" for a service path writable by a non privileged account
                foreach (string startServicePermission in s_startServicePermissions)
                {
                    if (m_rights.Contains(startServicePermission))
                    {
                        return true;
                    }
                }
                return false;
            }
            return false;
            //
        }

        internal bool IsUnsecurePermission()
        {
            string sidValue = m_SID;
            if (sidValue is null)
            {
                sidValue = SIDUtils.GetSidValue(m_user);
            }
            //We do not include permission that are only applied on descendant objets. So, we discard accesses with propagation filed as InheritOnly
            if (m_type.ToUpper() == "ALLOW" && m_propagation.ToUpper().Trim()!= "InheritOnly".ToUpper())
            {
                // https://docs.microsoft.com/en-us/windows/win32/secauthz/access-mask-format

                //https://www.ntfs.com/ntfs-permissions-file-advanced.htm
                foreach(string writingPermission in s_writingPermissions)
                {
                    if (m_rights.Contains(writingPermission.ToUpper()))
                    {
                        //We only return true as unsecure permission when there is a writing permission and account is not included in the list of privileged accounts.
                        bool isPrivileged=SIDUtils.IsPrivilegedSID(sidValue);
                        return !isPrivileged;
                    }
                }
                return false;
            }
            return false;

        }

        internal string GetProperties()
        {
            string properties = "User: " + m_user + Environment.NewLine +
                                "SID: " + m_SID + Environment.NewLine +
                                "Type: " + m_type + Environment.NewLine +
                                "Rights: " + m_rights + Environment.NewLine +
                                "Inheritance: " + m_inheritance + Environment.NewLine +
                                "Propagation: " + m_propagation + Environment.NewLine +
                                "Inherited?" + m_inherited + Environment.NewLine;

            return properties;
        }


        internal string User
        {
            get
            {
                return m_user;
            }
        }

        internal string Type
        {
            get
            {
                return m_type;
            }
        }

        internal string Rights
        {
            get
            {
                return m_rights;
            }
        }

        internal string Inheritance
        {
            get
            {
                return m_inheritance;
            }
        }

        internal string Propagation
        {
            get
            {
                return m_propagation;
            }
        }

        internal string Inherited
        {
            get
            {
                return m_inherited;
            }
        }

    }
}
