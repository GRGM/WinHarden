using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WinHardenApp.Configuration;
using InformationUtils;
using System.Runtime.InteropServices.ComTypes;

namespace WinHardenApp.AnalyzeInformationUtils
{
    class AnalyseComplianceInformationUtils
    {
        //https://docs.microsoft.com/en-US/windows-server/identity/ad-ds/plan/security-best-practices/audit-policy-recommendations

        private string m_configurationFilesFolder;
        private string m_analysisOutputFolder;
        private WinHardenConfiguration m_configuration;
        private bool m_isDomainServer = false;
        private string m_policyHost = "";
        private string m_subCategoryGuid = "";
        private string[] m_headerFiles = null;

        WindowsInformationUtils m_windowsInformationUtils;

        internal AnalyseComplianceInformationUtils(string configurationFilesFolder, string analysisOutputFolder, WinHardenConfiguration configuration, WindowsInformationUtils windowsInformationUtils)
        {
            m_configurationFilesFolder = configurationFilesFolder;
            m_analysisOutputFolder = analysisOutputFolder;
            m_configuration = configuration;
            m_windowsInformationUtils = windowsInformationUtils;
        }


        internal void AnalyseComplianceInformation()
        {
            m_windowsInformationUtils.UpdateProgressBar("Analysing audit policies");
            AnalyseAuditPoliciesCompliance();
            m_windowsInformationUtils.UpdateProgressBar("Analysing password policies");
            AnalysePasswordPoliciesCompliance();
            m_windowsInformationUtils.UpdateProgressBar("Analysing security policies");
            AnalyseSecurityPoliciesCompliance();

            m_windowsInformationUtils.UpdateProgressBar("Analysing registry policies");
            AnalyseRegistryPoliciesCompliance();

        }



        internal void AnalyseSecurityPoliciesCompliance()
        {
            SecurityPolicyConfiguration securityPolicyConfiguration = m_configuration.ComplianceConfiguration.SecurityPolicyConfigruation;
            string inputFileName = "";
            
            //Using defuault securit policy template provokes using secedit.cfg file
            if(securityPolicyConfiguration.IsTranslatedSID && securityPolicyConfiguration.SecurityPolicyFile !=null && securityPolicyConfiguration.SecurityPolicyFile.Trim()!="")
            {
                inputFileName = m_configurationFilesFolder + @"\secedit_translated_SID.cfg";
            }
            else
            {
                inputFileName = m_configurationFilesFolder + @"\secedit.cfg";
            }
            //Sometimes required file as secedit.cfg is not generated due to it was not executed under administrator privileges
            if(!File.Exists(inputFileName))
            {
                return;
            }
            string outputFilename = m_analysisOutputFolder + @"\secedit_RESULT.txt";

            SecurityPolicyUtils.CompareSecurityPolicyFiles(inputFileName, securityPolicyConfiguration.SecurityPolicyFile, outputFilename);
        }

        internal void AnalyseRegistryPoliciesCompliance()
        {
            SecurityPolicyConfiguration securityPolicyConfiguration = m_configuration.ComplianceConfiguration.SecurityPolicyConfigruation;
            string inputFileName = m_configurationFilesFolder + @"\RegistryPolicy.txt";
            string outputFilename = m_analysisOutputFolder + @"\RegistryPolicy.txt_RESULT.txt";


            RegistryUtils.CompareRegistryPolicyFiles(inputFileName, securityPolicyConfiguration.RegistryKeyPolicy, outputFilename);
        }



        internal void AnalysePasswordPoliciesCompliance()
        {
            string inputFileName = m_configurationFilesFolder + @"\localUsers.csv";
            string outputFilename = m_analysisOutputFolder + @"\localUsers_passwordPolicies_RESULT.txt";
            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);


            List<int> lastPasswordDays = m_configuration.ComplianceConfiguration.PasswordPolicyConfiguration.LastPasswordDays;
            List<int> lastLogonDays = m_configuration.ComplianceConfiguration.PasswordPolicyConfiguration.LastLogonDays;
            if (lastPasswordDays != null)
            {
                foreach (int passwordDays in lastPasswordDays)
                {
                    WriteEnabledUsersLastPasswordSetDays(inputFileName, outputFile, passwordDays);
                }
            }
            if (lastLogonDays != null)
            {
                foreach (int logonDays in lastLogonDays)
                {
                    WriteEnabledUsersLastLogonDays(inputFileName, outputFile, logonDays);
                }
            }
            outputFile.Close();
        }

        private static void WriteEnabledUsersLastPasswordSetDays(string inputFileName, StreamWriter outputFile,int lastDays)
        {
            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);

            outputFile.WriteLine("Enabled users with last password set more than "+ lastDays.ToString()+" days ago: ");

            string[] separator = new string[1] { @"," };
            //We read header
            inputFile.ReadLine();
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();

                string[] fields = line.Split(separator, StringSplitOptions.None);
                string enabled = fields[5].ToUpper().Trim();
                if (enabled == "TRUE")
                {
                    string lastPasswordSetStr = fields[7].ToUpper().Trim();
                    string name = fields[2];
                    string groups = fields[22];
                    if (lastPasswordSetStr.Trim() == "")
                    {
                        string lastLogon = fields[6].ToUpper().Trim();
                        outputFile.WriteLine("User :" + name + " Last password set is blank. Last Logon is: " + lastLogon + " Groups: " + groups);
                        continue;
                    }
                    DateTime dateTime = Convert.ToDateTime(lastPasswordSetStr);
                    DateTime daysAgo = DateTime.Today.AddDays(-1*lastDays);
                    if (DateTime.Compare(dateTime, daysAgo) < 0)
                    {
                        outputFile.WriteLine("User :" + name + " Last password set: " + lastPasswordSetStr + " Groups: " + groups);
                    }
                }
            }
            outputFile.WriteLine();


            inputFile.Close();

        }




        private static void WriteEnabledUsersLastLogonDays(string inputFileName, StreamWriter outputFile, int logonDays)
        {


            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);

            outputFile.WriteLine("Enabled users with last logon more than  " + logonDays.ToString() + " days ago: ");

            string[] separator = new string[1] { @"," };
            //We read header
            inputFile.ReadLine();
            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().Trim();

                string[] fields = line.Split(separator, StringSplitOptions.None);
                string enabled = fields[5].ToUpper().Trim();
                if (enabled == "TRUE")
                {
                    string lastLogon = fields[6].ToUpper().Trim();
                    string name = fields[2];
                    string groups = fields[22];
                    if (lastLogon.Trim() == "")
                    {
                        outputFile.WriteLine("User :" + name + " Last logon is blank. Groups: " + groups);
                        continue;
                    }

                    DateTime parsed = Convert.ToDateTime(lastLogon);
                    DateTime daysAgo = DateTime.Today.AddDays(-1* logonDays);
                    if (DateTime.Compare(parsed, daysAgo) < 0)
                    {
                        outputFile.WriteLine("User :" + name + " Last logon set: " + lastLogon + " Groups: " + groups);
                    }
                }
            }
            outputFile.WriteLine();


            inputFile.Close();

        }


        internal void AnalyseAuditPoliciesCompliance()
        {
            string inputFileName = m_configurationFilesFolder + @"\auditpolicies.csv";

            string domainFileName=m_configurationFilesFolder + @"\DomainRole.txt";

            string outputFilename = m_analysisOutputFolder + @"\auditpolicies_RESULT.txt";
            FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);

            FileStream domainFileStream = new FileStream(domainFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader domainFile = new StreamReader(domainFileStream, Encoding.Default);

            StreamWriter outputFile = new StreamWriter(outputFilename, false, Encoding.Default);

            //We read the header
            domainFile.ReadLine();
            //Next line is the value
            string domainRole=domainFile.ReadLine().Trim();
            domainFile.Close();
            //https://dirteam.com/sander/2009/09/23/how-to-tell-whether-it-s-a-server-core-domain-controller/
            if (domainRole=="5" || domainRole == "4")
            {
                m_isDomainServer = true;
            }

            List<string> auditPolicies = m_configuration.ComplianceConfiguration.AuditPolicyConfiguration.GetAuditPolicyRecommendations();

            string[] separator = new string[1] { @"," };
            //We read the header
            inputFile.ReadLine();



            m_headerFiles = GetPolicyFields(auditPolicies[0]);
            auditPolicies.RemoveAt(0);


            while (!inputFile.EndOfStream)
            {
                string line = inputFile.ReadLine().ToUpper().Trim();
                if (line == "")
                {
                    continue;
                }
                string[] fields = line.Split(separator, StringSplitOptions.None);
                string subCategoryGuid = fields[3];
                m_policyHost = fields[4];
                string checkResult = CheckAuditPolicies(auditPolicies, subCategoryGuid);
                if(checkResult!="")
                {
                    outputFile.Write(checkResult);
                }
            }
            outputFile.Close();
        }

        private string CheckAuditPolicies(List<string>  auditPolicies, string subCategoryGuidHost)
        {
            string checkResult = "";
            string[] separator = new string[1] { @"," };

            foreach (string policy in auditPolicies)
            {
                string[] fields = policy.Split(separator, StringSplitOptions.None);
                string subCategoryGuid = fields[2];
                subCategoryGuid=subCategoryGuid.Replace("{", "");
                subCategoryGuid=subCategoryGuid.Replace("}", "");
                if (subCategoryGuidHost== subCategoryGuid)
                {
                    m_subCategoryGuid = subCategoryGuid;
                    string[] policyFields = GetPolicyFields(policy);
                    checkResult +=CheckAuditPolicy(policyFields);
                }
            }
            return checkResult;
        }
        private string[] GetPolicyFields(string header)
        {
            string[] separator = new string[1] { @"," };
            string[] fields = header.Split(separator, StringSplitOptions.None);
            int policyHeaders = fields.Length - 3;
            if(policyHeaders<1)
            {
                throw new Exception("Audit compliance recommendations do not include required columns or format as comma separated values file.");
            }
            string[] headerFields = new string[policyHeaders];
            for(int i= 3, j=0; i< fields.Length;i++,j++)
            {
                headerFields[j] = fields[i];

            }
            return headerFields;
        }

        private string CheckAuditPolicy(string[] policyFields)
        {
            string checkResult = "";
            for(int i=0; i< policyFields.Length;i++)
            {
                checkResult += CheckPolicyField(policyFields[i], m_headerFiles[i]);
            }
            return checkResult;
        }

        private string CheckPolicyField(string policy,string headerFile)
        {
            string result = "";
            string[] separator = new string[1] { @"," };
            string[] fields = policy.Split(separator, StringSplitOptions.None);
            foreach(string policyField in fields)
            {
                if(policyField.Trim()=="")
                {
                    //There is no recommendation
                    continue;
                }
                separator = new string[1] { @"|" };
                string[] fields2 = policyField.Split(separator, StringSplitOptions.None);
                if(fields2.Length!=2)
                {
                    throw new Exception("Format not accepted in policy. It should be blank. [YES][NO][DC]|[YES][NO][DC] : " + policy);
                }
                string success = fields2[0].ToUpper().Trim();
                string failure = fields2[1].ToUpper().Trim();
                result += CheckPolicyToken(success, "SUCESS", headerFile);
                result += CheckPolicyToken(failure, "FAILURE", headerFile);
            }
            return result;
        }

        private string CheckPolicyToken(string policyToken, string requiredString, string headerFile)
        {
            switch(policyToken)
            {
                case "YES":
                    if(!m_policyHost.Contains(requiredString))
                    {
                        string subCategory = AuditPolicyUtils.GetSubCategoryName(m_subCategoryGuid);
                        return subCategory+ " " + m_subCategoryGuid + " " + headerFile +" Expected: " + requiredString + Environment.NewLine;
                    }
                    return "";
                case "NO":
                    return "";
                case "DC":
                    if (m_isDomainServer && !m_policyHost.Contains(requiredString))
                    {
                        string subCategory = AuditPolicyUtils.GetSubCategoryName(m_subCategoryGuid);
                        return subCategory + " " + m_subCategoryGuid + " " + headerFile + " Recommendation for domain controller, expected: " + requiredString+Environment.NewLine;
                    }
                    return "";
                default:
                    throw new Exception(m_subCategoryGuid+ " " + headerFile + " Only YES, NO and DC are valid values as policy token. Policy token not expected:" + policyToken);
            }
        }






    }
}
