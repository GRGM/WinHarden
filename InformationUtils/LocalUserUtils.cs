using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;

namespace InformationUtils
{
    public class LocalUserUtils
    {

        private string m_extractionBaseFolder;
        private string m_analysisOutputFolder;

        public const string Header =
            "Context"
        + "," + "SamAccountName"
        + "," + "Name"
        + "," + "description"
        + "," + "DistinguishedName"
        + "," + "Enabled"
        + "," + "LastLogon"
        + ","+ "LastPasswordSet"
        + "," + "PasswordNeverExpires"
        + "," + "PasswordNotRequired"
        + "," + "AccountExpirationDate"
        + "," + "AccountLockoutTime"
        + "," + "AllowReversiblePasswordEncryption"
        + "," + "BadLogonCount"
        + "," + "DisplayName"
        + "," + "GivenName"
        + "," + "IsAccountLockedOut"
        + "," + "LastBadPasswordAttempt"
        + "," + "ScriptPath"
        + "," + "Sid"
        + "," + "UserPrincipalName"
      //  + "," + "UserCannotChangePassword"
        + "," + "PermittedWorkstations"
        + "," + "Company"
        + "," + "Department"
        + "," + "Title"
        + "," + "EmployeeType"
        + "," + "Manager"
        + "," + "Groups";

        internal const string PrincipalHeader =
            "ContexType"
        + "," +" SamAccountName"
        + "," + "Name"
        + "," + "description"
        + "," + "DistinguishedName"
        + "," + "DisplayName"
        + "," + "Sid"
        + "," + "UserPrincipalName";



        public LocalUserUtils(string extractionBaseFolder, string analysisOutputFolder)
        {
            m_extractionBaseFolder = extractionBaseFolder;
            m_analysisOutputFolder = analysisOutputFolder;
        }

        public static List<Principal> GetMembersFromGroup(ContextType contextType, IdentityType identityTpe,  string groupName,string connectedServer)
        {
            List<Principal> principals = new List<Principal>();
            //We use a list of sam account names to prevent adding duplicated accounts. So, program is faster
            List<string> members = new List<string>();

    
            PrincipalContext context = new PrincipalContext(contextType, connectedServer);
                       
            GroupPrincipal group = GroupPrincipal.FindByIdentity(context, identityTpe, groupName);
            if(group is null)
            {
                return principals;
            }
            // Iterate over members
            foreach (Principal groupMember in group.GetMembers())
            {
                //Local user
                if (groupMember.StructuralObjectClass is null)
                {
                    //We use Sid because Azure accounts or roles has SID but not name
                    if (!members.Contains(groupMember.Sid.Value))
                    {
                        principals.Add(groupMember);
                        members.Add(groupMember.Sid.Value);
                    }
                    continue;
                }
                // Domain user
                if (groupMember.StructuralObjectClass == "user" || groupMember.StructuralObjectClass == "msDS-GroupManagedServiceAccount")
                {
                    if (!members.Contains(groupMember.Sid.Value))
                    {
                        principals.Add(groupMember);
                        members.Add(groupMember.Sid.Value);
                    }
                    continue;
                }
                //Group. Domain or local according to ContextType
                if (groupMember.StructuralObjectClass == "group")
                {
                    //This function is recursive. We replace groups by corresponding members
                    string groupConnectedServer = connectedServer;
                    if (groupMember.ContextType==ContextType.Machine)
                    {
                        groupConnectedServer = Environment.MachineName;
                    }
                    List<Principal> groupPrincipals = GetMembersFromGroup(groupMember.ContextType, IdentityType.Name, groupMember.Name, groupConnectedServer);
                    foreach (Principal groupPrincipal in groupPrincipals)
                    {
                        if (!members.Contains(groupPrincipal.Sid.Value))
                        {
                            principals.Add(groupPrincipal);
                            members.Add(groupPrincipal.Sid.Value);
                        }
                    }
                }
                else
                {
                    throw new Exception("Unknwon StructuralObjectClass:" + groupMember.StructuralObjectClass + " for member: " + groupMember.Name);
                }
            }
            return principals;
        }




        private static void WriteEnabledUsersPasswordNeverExpires(string inputFileName, StreamWriter outputFile)
        {
            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);


            string[] separator = new string[1] { @"," };
            //We read header
            inputFile.ReadLine();
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();

                string[] fields = line.Split(separator, StringSplitOptions.None);
                //We ignore error messages
                if(fields.Length==1)
                {
                    continue;
                }
                string enabled = fields[5].ToUpper().Trim();
                if (enabled == "TRUE")
                {
                    string passwordNeverExpires = fields[8].ToUpper().Trim();
                    if (passwordNeverExpires == "TRUE")
                    {
                        string name = fields[1];
                        string groups = fields[22];
                        outputFile.WriteLine("User :" + name + " Password Never Expires: " + passwordNeverExpires + " Groups: " + groups);
                    }
                }
            }
            inputFile.Close();
        }


        private static void WriteEnabledUsersPasswordNotRequired(string inputFileName, StreamWriter outputFile)
        {

            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);


            string[] separator = new string[1] { @"," };
            //We read header
            inputFile.ReadLine();
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();

                string[] fields = line.Split(separator, StringSplitOptions.None);
                //We ignore error messages;
                if(fields.Length==1)
                {
                    continue;
                }
                string enabled = fields[5].ToUpper().Trim();
                if (enabled == "TRUE")
                {
                    string passwordNotRequired = fields[9].ToUpper().Trim();
                    if (passwordNotRequired == "TRUE")
                    {
                        string name = fields[2];
                        string groups = fields[22];
                        outputFile.WriteLine("User :" + name + " Password Not Required: " + passwordNotRequired + " Groups: " + groups);
                    }
                }
            }
            inputFile.Close();
        }


        private static void WriteEnabledUserReversiblesPassword(string inputFileName, StreamWriter outputFile)
        {

            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);

            string[] separator = new string[1] { @"," };
            //We read header
            inputFile.ReadLine();
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();

                string[] fields = line.Split(separator, StringSplitOptions.None);
                if(fields.Length==1)
                {
                    continue;
                }  

                string enabled = fields[5].ToUpper().Trim();
                if (enabled == "TRUE")
                {
                    string reversiblePassword = fields[12].ToUpper().Trim();
                    if (reversiblePassword == "TRUE")
                    {
                        string name = fields[2];
                        string groups = fields[22];
                        outputFile.WriteLine("User :" + name + " Revesible password encryption: " + reversiblePassword + " Groups: " + groups);
                    }
                }
            }
            inputFile.Close();
        }


        public static string GetUserInformation(UserPrincipal user,bool obtainGroups)
        {
            string description = "";
            if (user.Description != null)
            {
                description = user.Description.Replace(',', '|');
            }

            string distinguishedName = "";
            if (user.DistinguishedName != null)
            {
                distinguishedName = user.DistinguishedName.Replace(',', '|');
            }

            string displayName = "";
            if (user.DisplayName != null)
            {
                displayName = user.DisplayName.Replace(',', '|');
            }

            string givenName = "";
            if (user.GivenName != null)
            {
                givenName = user.GivenName.Replace(',', '|');
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(user.Context.Name + ",");
            sb.Append(user.SamAccountName+",");
            sb.Append(user.Name + ",");
            sb.Append(description + ",");
            sb.Append(distinguishedName + ",");
            sb.Append(user.Enabled + ",");
            sb.Append(user.LastLogon + ",");
            sb.Append(user.LastPasswordSet + ",");
            sb.Append(user.PasswordNeverExpires + ",");
            sb.Append(user.PasswordNotRequired + ",");
            sb.Append(user.AccountExpirationDate + ",");
            sb.Append(user.AccountLockoutTime + ",");
            sb.Append(user.AllowReversiblePasswordEncryption + ",");
            sb.Append(user.BadLogonCount + ",");
            sb.Append(displayName + ",");
            sb.Append(givenName + ",");
            sb.Append(user.IsAccountLockedOut() + ",");
            sb.Append(user.LastBadPasswordAttempt + ",");
            sb.Append(user.ScriptPath + ",");
            sb.Append(user.Sid + ",");
            sb.Append(user.UserPrincipalName + ",");
            //Removed because obtaining this prooperty affects to perfomance
            //sb.Append(user.UserCannotChangePassword + ",");

            string permittedWorkstations = "";
            foreach (string permittedWorkstation in user.PermittedWorkstations)
            {
                permittedWorkstations += @" \\ " + permittedWorkstation;
            }
            if(permittedWorkstations=="")
            {
                permittedWorkstations = "All";
            }

            DirectoryEntry entry = user.GetUnderlyingObject() as DirectoryEntry;
            string attributes = GetString(entry.Properties["company"].Value).Replace(',', '|') + "," +
                GetString(entry.Properties["department"].Value).Replace(',', '|') + "," +
                GetString(entry.Properties["title"].Value).Replace(',', '|') + "," +
                GetString(entry.Properties["employeeType"].Value).Replace(',', '|') + "," +
                GetString(entry.Properties["manager"].Value).Replace(',', '|');


            string groups = "";
            if (obtainGroups)
            {
                foreach (Principal group in user.GetGroups())
                {
                    groups += @" \\ " + group.Name;
                }
            }
            sb.Append(permittedWorkstations + "," + attributes + "," + groups);
            return sb.ToString();
        }

        private static string GetString(object strObject)
        {
            if (strObject is null)
            {
                return "";
            }
            else
            {
                return strObject.ToString();
            }
        }

        internal static string GetPrincipalInformation(Principal principal)
        {
            string description = "";
            if (principal.Description != null)
            {
                description = principal.Description.Replace(',', '|');
            }
            string val = principal.ContextType.ToString()
                + "," + principal.SamAccountName
                + "," + principal.Name
                + "," + description
                + "," + principal.DistinguishedName
                + "," + principal.DisplayName
                + "," + principal.Sid
                + "," + principal.UserPrincipalName;

            return val;
        }

        public static string GetUserInformation(ComputerPrincipal user, bool obtainGroups)
        {
            string description = "";
            if (user.Description != null)
            {
                description = user.Description.Replace(',', '|');
            }
            string val =
                user.Context.Name
                + "," + user.SamAccountName
                + "," + user.Name
                + "," + description
                + "," + user.DistinguishedName
                + "," + user.Enabled
                + "," + user.LastLogon
                + "," + user.LastPasswordSet
                + "," + user.PasswordNeverExpires
                + "," + user.PasswordNotRequired
                + "," + user.AccountExpirationDate
                + "," + user.AccountLockoutTime
                + "," + user.AllowReversiblePasswordEncryption
                + "," + user.BadLogonCount
                + "," + user.DisplayName
                + "," + ""
                + "," + user.IsAccountLockedOut()
                + "," + user.LastBadPasswordAttempt
                + "," + user.PermittedWorkstations
                + "," + user.ScriptPath
                + "," + user.Sid
                + "," + user.UserPrincipalName;
                //+ "," + user.UserCannotChangePassword;


            string permittedWorkstations = "";
            foreach (string permittedWorkstation in user.PermittedWorkstations)
            {
                permittedWorkstations += @" \\ " + permittedWorkstation;
            }
            if (permittedWorkstations == "")
            {
                permittedWorkstations="All";
            }

            //Computer principal has not make sense to have user attributes
            string attributes = ",,,,";

            string groups = "";
            if (obtainGroups)
            {
                foreach (Principal group in user.GetGroups())
                {
                    groups += @" \\ " + group.Name;
                }
            }
            string userInformation= val + "," + permittedWorkstations + "," + attributes + "," + groups;
            return userInformation;
        }




        public bool AnalyseDomainAccountsLocalAdministrators(string connectedServer, bool obtainGroups)
        {

            string inputFileName = m_extractionBaseFolder + @"\domainLocalAdministratorGroup.csv";
            string outputFileName = m_analysisOutputFolder + @"\domainLocalAdministratorGroup_RESULT.csv";


            string[] inputLines = System.IO.File.ReadAllLines(inputFileName);
            StreamWriter outputFile = new StreamWriter(outputFileName, false, Encoding.Default);

            string[] separator = new string[1] { @"," };

            
            //If there are no domain local administrator accounts, we return from ths function
            if(inputLines.Length==1)
            {
                outputFile.Close();
                return true;
            }
        
            PrincipalContext context;
            try
            {

                context = new PrincipalContext(ContextType.Domain, connectedServer);

            }
            catch(Exception e)
            {
                //If we obtain an exception trying to connect on Principal Context is usually relate that we are trying to connect to an unexisting LDAP server from a personal home workstation.
                outputFile.WriteLine(e.Message);
                outputFile.Close();
                return false;
            }

            //We write header

            bool exitsHeader = false;
            for (int i = 1; i < inputLines.Length; i++)
            {
                string line = inputLines[i].Trim();
                string[] fields = line.Split(separator, StringSplitOptions.None);
                //We ignore lines without fields. Probably they are message errors to be ignored.
                if(fields.Length==1)
                {
                    continue;
                }
                if(!exitsHeader)
                {
                    outputFile.WriteLine(Header);
                    exitsHeader = true;
                }
                string samAccountName = fields[1].ToUpper().Trim();
                Principal principal = Principal.FindByIdentity(context, IdentityType.SamAccountName, samAccountName);
                if (principal.GetType().Equals(typeof(UserPrincipal)))
                {
                    string userInformation = GetUserInformation((UserPrincipal)principal, obtainGroups);
                    outputFile.WriteLine(userInformation);
                }
                if (principal.GetType().Equals(typeof(ComputerPrincipal)))
                {
                    string computerPrincipalInformation = GetUserInformation((ComputerPrincipal)principal, obtainGroups);
                    outputFile.WriteLine(computerPrincipalInformation);
                }

                //This condition should not happen due to all members of groups were obtained recursively during GetMembersFromGroup in GetMembersFromSIDGroup function
                if (principal.GetType().Equals(typeof(GroupPrincipal)))
                {
                    outputFile.Close();
                    throw new Exception("This function would not expect a group princial. All members of groups should be obtained during extraction process.");
                }

            }

            outputFile.Close();
            return true;
        }



        public void MergeTotalAdministrators()
        {
            string inputLocalFileName = m_extractionBaseFolder + @"\localAdministratorGroup.csv";

            //This file has been generated in step before AnalyseDomainAdministrators()
            string inputDomainFileName = m_analysisOutputFolder + @"\domainLocalAdministratorGroup_RESULT.csv";


            string outputTotalFilename = m_analysisOutputFolder + @"\totalAdministratorGroup_RESULT.csv";


            FileStream inputLocalFileStream = new FileStream(inputLocalFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            FileStream inputDomainFileStream = new FileStream(inputDomainFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            StreamReader inputLocalFile = new StreamReader(inputLocalFileStream, Encoding.Default);
            StreamReader inputDomainFile = new StreamReader(inputDomainFileStream, Encoding.Default);

            StreamWriter outputTotalFile = new StreamWriter(outputTotalFilename, false, Encoding.Default);

            bool existsHeader = false;


            //We read header
            inputLocalFile.ReadLine();
            while (!inputLocalFile.EndOfStream)
            {
                if(!existsHeader)
                {
                    outputTotalFile.WriteLine(LocalUserUtils.Header);
                    existsHeader = true;
                }
                outputTotalFile.WriteLine(inputLocalFile.ReadLine());
            }
            inputLocalFile.Close();

            //We read header
            inputDomainFile.ReadLine();
            while (!inputDomainFile.EndOfStream)
            {
                if (!existsHeader)
                {
                    outputTotalFile.WriteLine(LocalUserUtils.Header);
                    existsHeader = true;
                }
                outputTotalFile.WriteLine(inputDomainFile.ReadLine());
            }
            inputDomainFile.Close();

            outputTotalFile.Close();
        }


        public static void AnalyseLocalUserInformation(string inputFileName, string outputFilename)
        {
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);
            WriteEnabledUsersPasswordNeverExpires(inputFileName, outputFile);
            WriteEnabledUsersPasswordNotRequired(inputFileName, outputFile);
            WriteEnabledUserReversiblesPassword(inputFileName, outputFile);
            outputFile.Close();
        }




        public string ExtractionBaseFolder
        {
            get
            {
                return m_extractionBaseFolder;
            }
        }
        public string AnalysisOutputFolder
        {
            get
            {
                return m_analysisOutputFolder;
            }
        }
    }
}
