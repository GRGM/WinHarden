using System;
using System.IO;

namespace WinHardenApp.Configuration
{
    internal class SecurityPolicyConfiguration
    {

        private string m_securityPolicyFile = null;
        private bool m_isTranslatedSID;
        private string m_registryKeyPolicyFile = null;


        internal SecurityPolicyConfiguration(string securityPolicyFile, bool isTranslatedSID, string registryKeyPolicyFile)
        {
            m_securityPolicyFile = securityPolicyFile;
            m_isTranslatedSID = isTranslatedSID;
            m_registryKeyPolicyFile = registryKeyPolicyFile;
        }
        internal static SecurityPolicyConfiguration GetSecurityPolicyConfiguration(StreamReader inputFile)
        {
            string securityPolicyFile = GetSecurityPolicyFile(inputFile);
            bool isTranslatedSID = StringUtils.GetBoolNextLine(inputFile, "IS_TRANSLATED_SID");
            string registryKeyPolicy = GetRegistryKeyPolicyFile(inputFile);

            SecurityPolicyConfiguration securityPolicyConfiguration = new SecurityPolicyConfiguration(securityPolicyFile, isTranslatedSID, registryKeyPolicy);
            return securityPolicyConfiguration;
        }

        private static string GetRegistryKeyPolicyFile(StreamReader inputFile)
        {
            string line = inputFile.ReadLine().ToUpper();
            if (line.StartsWith("REGISTRY_KEY_POLICY_FILE=".ToUpper()))
            {
                string securityPolicyFile = line.Replace("REGISTRY_KEY_POLICY_FILE=", "");
                return securityPolicyFile;
            }
            else
            {
                inputFile.Close();
                throw new Exception("Not expected configuration line. It should be REGISTRY_KEY_POLICY_FILE=file. Program has found: " + line);
            }
        }
        private static string GetSecurityPolicyFile(StreamReader inputFile)
        {
            string line = inputFile.ReadLine().ToUpper();
            if (line.StartsWith("SECURITY_POLICY_FILE=".ToUpper()))
            {
                string securityPolicyFile = line.Replace("SECURITY_POLICY_FILE=", "");
                return securityPolicyFile;
            }
            else
            {
                inputFile.Close();
                throw new Exception("Not expected configuration line. It should be SECURITY_POLICY_FILE=file. Program has found: " + line);
            }
        }

        internal bool IsTranslatedSID
        {
            get
            {
                return m_isTranslatedSID;
            }
        }

        internal string SecurityPolicyFile
        {
            get
            {
                return m_securityPolicyFile;
            }
        }
        internal string RegistryKeyPolicy
        {
            get
            {
                return m_registryKeyPolicyFile;
            }
        }

    }
}
