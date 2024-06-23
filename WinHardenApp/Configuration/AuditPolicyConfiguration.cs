using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinHardenApp.Configuration
{
    internal enum AuditPolicyComplianceType
    {
        WorkstationRecommendations,
        ServerRecommendations,
        UserDefinedRecommendations

    }
    internal class AuditPolicyConfiguration
    {
        private AuditPolicyComplianceType m_auditPolicyComplianceType;
        private string m_userDefinedRecommendationsFile = null;

        internal AuditPolicyConfiguration(AuditPolicyComplianceType auditPolicyComplianceType, string userDefinedRecommendationsFile)
        {
            m_auditPolicyComplianceType = auditPolicyComplianceType;
            m_userDefinedRecommendationsFile = userDefinedRecommendationsFile;
        }



        internal List<string> GetAuditPolicyRecommendations()
        {
            List<string> lines = new List<string>();
            StreamReader ms = null;
            switch (m_auditPolicyComplianceType)
            {
                case AuditPolicyComplianceType.WorkstationRecommendations:
                    ms = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(Resource1.auditpolicy_WORKSTATION)));
                    break;
                case AuditPolicyComplianceType.ServerRecommendations:
                    ms = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(Resource1.auditpolicy_SERVER)));
                    break;
                case AuditPolicyComplianceType.UserDefinedRecommendations:
                    FileStream inputFileStream = new FileStream(m_userDefinedRecommendationsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    ms = new StreamReader(inputFileStream, Encoding.Default);
                    break;
            }
            string line;
            while ((line = ms.ReadLine()) != null)
            {
                lines.Add(line);
            }
            ms.Close();
            return lines;

        }

        internal static AuditPolicyConfiguration GetAuditPolicyConfiguration(StreamReader inputFile)
        {
            AuditPolicyComplianceType auditPolicyComplianceType = GetAuditPolicyComplianceType(inputFile);
            string userDefinedRecommendationsFile = null;
            if (auditPolicyComplianceType == AuditPolicyComplianceType.UserDefinedRecommendations)
            {
                userDefinedRecommendationsFile = GetuserDefinedRecommendationsFile(inputFile);
            }
            AuditPolicyConfiguration auditPolicyConfiguration = new AuditPolicyConfiguration(auditPolicyComplianceType, userDefinedRecommendationsFile);
            return auditPolicyConfiguration;
        }


        private static string GetuserDefinedRecommendationsFile(StreamReader inputFile)
        {
            string line = inputFile.ReadLine().ToUpper();
            if (line.StartsWith("USER_DEFINED_RECOMMENDATIONS_FILE=".ToUpper()))
            {
                string userDefinedRecommendationsFile = line.Replace("USER_DEFINED_RECOMMENDATIONS_FILE=", "");
                if (!File.Exists(userDefinedRecommendationsFile))
                {
                    inputFile.Close();
                    throw new Exception("User defined recommendation file does not exist. It should be USER_DEFINED_RECOMMENDATIONS_FILE=Path. Program has found: " + line);
                }
                return userDefinedRecommendationsFile;
            }
            else
            {
                inputFile.Close();
                throw new Exception("Not expected configuration line. It should be USER_DEFINED_RECOMMENDATIONS_FILE=Path. Program has found: " + line);
            }
        }

        private static AuditPolicyComplianceType GetAuditPolicyComplianceType(StreamReader inputFile)
        {

            AuditPolicyComplianceType auditPolicyComplianceType;
            string line = inputFile.ReadLine().ToUpper();
            if (line.StartsWith("AUDIT_POLICY_TYPE="))
            {
                string auditPolicyStr = line.Replace("AUDIT_POLICY_TYPE=", "");
                if (auditPolicyStr == "Workstation_Recommendations".ToUpper())
                {
                    auditPolicyComplianceType = AuditPolicyComplianceType.WorkstationRecommendations;
                    return auditPolicyComplianceType;
                }
                if (auditPolicyStr == "Server_Recommendations".ToUpper())
                {
                    auditPolicyComplianceType = AuditPolicyComplianceType.ServerRecommendations;
                    return auditPolicyComplianceType;
                }
                if (auditPolicyStr == "User_Defined_Recommendations".ToUpper())
                {
                    auditPolicyComplianceType = AuditPolicyComplianceType.UserDefinedRecommendations;
                    return auditPolicyComplianceType;
                }
                inputFile.Close();
                throw new Exception("Not expected configuration line. It should be AUDIT_POLICY_TYPE=Workstation_Recommendations|Server_Recommendations|User_Defined_Recommendations. Program has found: " + line);
            }
            else
            {
                inputFile.Close();
                throw new Exception("Not expected configuration line. It should be AUDIT_POLICY_TYPE=Workstation_Recommendations|Server_Recommendations|User_Defined_Recommendations. Program has found: " + line);
            }
        }



        internal AuditPolicyComplianceType AuditPolicyComplianceType
        {
            get
            {
                return m_auditPolicyComplianceType;
            }
        }

        internal string UserDefinedRecommendationsFile
        {
            get
            {
                return m_userDefinedRecommendationsFile;
            }
        }
    }
}
