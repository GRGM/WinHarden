using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinHardenApp.Configuration
{

    internal class ComplianceConfiguration
    {

        private AuditPolicyConfiguration m_auditPolicyConfiguration;
        private PasswordPolicyConfiguration m_passwordPolicyConfiguration;
        private SecurityPolicyConfiguration m_securityPolicyConfigruation;


        internal ComplianceConfiguration(AuditPolicyConfiguration auditPolicyConfiguration, PasswordPolicyConfiguration passwordPolicyConfiguration, SecurityPolicyConfiguration securityPolicyConfigruation)
        {
            m_auditPolicyConfiguration = auditPolicyConfiguration;
            m_passwordPolicyConfiguration = passwordPolicyConfiguration;
            m_securityPolicyConfigruation = securityPolicyConfigruation;
        }



        internal static ComplianceConfiguration GetComplianceConfiguration(StreamReader inputFile)
        {
            AuditPolicyConfiguration auditPolicyConfiguration=AuditPolicyConfiguration.GetAuditPolicyConfiguration(inputFile);
            PasswordPolicyConfiguration passwordPolicyConfiguration = PasswordPolicyConfiguration.GetPasswordPolicyConfiguration(inputFile);
            SecurityPolicyConfiguration securityPolicyConfigruation = SecurityPolicyConfiguration.GetSecurityPolicyConfiguration(inputFile);

            ComplianceConfiguration auditPolicyComplianceConfiguration = new ComplianceConfiguration(auditPolicyConfiguration, passwordPolicyConfiguration, securityPolicyConfigruation);
            return auditPolicyComplianceConfiguration;
        }

        internal string GetAuditPolicyString()
        {
            string auditPolicyString = "";
            switch (m_auditPolicyConfiguration.AuditPolicyComplianceType)
            {
                case AuditPolicyComplianceType.WorkstationRecommendations:
                    auditPolicyString = "AUDIT_POLICY_TYPE=Workstation_Recommendations".ToUpper() + Environment.NewLine;
                    break;
                case AuditPolicyComplianceType.ServerRecommendations:
                    auditPolicyString = "AUDIT_POLICY_TYPE=Server_Recommendations".ToUpper() + Environment.NewLine;
                    break;
                case AuditPolicyComplianceType.UserDefinedRecommendations:
                    auditPolicyString = "AUDIT_POLICY_TYPE=User_Defined_Recommendations".ToUpper() + Environment.NewLine;
                    auditPolicyString += "USER_DEFINED_RECOMMENDATIONS_FILE=".ToUpper() + m_auditPolicyConfiguration.UserDefinedRecommendationsFile;
                    break;
            }
            auditPolicyString += "LAST_PASSWORD_DAYS=" + StringUtils.GetStringFromListInt(m_passwordPolicyConfiguration.LastPasswordDays) + Environment.NewLine;
            auditPolicyString += "LAST_LOGON_DAYS=" + StringUtils.GetStringFromListInt(m_passwordPolicyConfiguration.LastLogonDays) + Environment.NewLine;

            auditPolicyString += "SECURITY_POLICY_FILE=" + m_securityPolicyConfigruation.SecurityPolicyFile + Environment.NewLine;
            auditPolicyString += "IS_TRANSLATED_SID=" + m_securityPolicyConfigruation.IsTranslatedSID + Environment.NewLine;
            auditPolicyString += "REGISTRY_KEY_POLICY_FILE=" + m_securityPolicyConfigruation.RegistryKeyPolicy + Environment.NewLine;

            return auditPolicyString;
        }

        internal AuditPolicyConfiguration AuditPolicyConfiguration
        {
            get
            {
                return m_auditPolicyConfiguration;
            }
        }

        internal PasswordPolicyConfiguration PasswordPolicyConfiguration
        {
            get
            {
                return m_passwordPolicyConfiguration;
            }
        }


        internal SecurityPolicyConfiguration SecurityPolicyConfigruation
        {
            get
            {
                return m_securityPolicyConfigruation;
            }
        }
    }
}
