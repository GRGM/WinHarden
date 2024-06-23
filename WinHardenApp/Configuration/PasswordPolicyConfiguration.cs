using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WinHardenApp.Configuration
{
    internal class PasswordPolicyConfiguration
    {
        private List<int> m_lastPasswordDays = null;
        private List<int> m_lastLogonDays = null;

        internal PasswordPolicyConfiguration( List<int> lastPasswordDays, List<int> lastLogonDays)
        {
            m_lastPasswordDays = lastPasswordDays;
            m_lastLogonDays = lastLogonDays;
        }
        internal static PasswordPolicyConfiguration GetPasswordPolicyConfiguration(StreamReader inputFile)
        {

            List<int> lastPasswordDays = GetLastPasswordDays(inputFile);
            List<int> lastLogonDays = GetLastLogonDays(inputFile);
            PasswordPolicyConfiguration passwordPolicyConfiguration = new PasswordPolicyConfiguration(lastPasswordDays, lastLogonDays);
            return passwordPolicyConfiguration;
        }
        private static List<int> GetLastPasswordDays(StreamReader inputFile)
        {
            string line = inputFile.ReadLine().ToUpper();
            if (line.StartsWith("LAST_PASSWORD_DAYS=".ToUpper()))
            {
                string lastPasswordDaysStr = line.Replace("LAST_PASSWORD_DAYS=", "");
                List<int> lastPasswordDays = StringUtils.GetListInt(lastPasswordDaysStr);
                return lastPasswordDays;
            }
            else
            {
                inputFile.Close();
                throw new Exception("Not expected configuration line. It should be LAST_PASSWORD_DAYS=days_separated_by_commmas. Program has found: " + line);
            }
        }
        private static List<int> GetLastLogonDays(StreamReader inputFile)
        {
            string line = inputFile.ReadLine().ToUpper();
            if (line.StartsWith("LAST_LOGON_DAYS=".ToUpper()))
            {
                string lastLogonDaysStr = line.Replace("LAST_LOGON_DAYS=", "");
                List<int> lastLogonDays = StringUtils.GetListInt(lastLogonDaysStr);
                return lastLogonDays;
            }
            else
            {
                inputFile.Close();
                throw new Exception("Not expected configuration line. It should be LAST_LOGON_DAYS=days_separated_by_commmas. Program has found: " + line);
            }
        }

        internal List<int> LastPasswordDays
        {
            get
            {
                return m_lastPasswordDays;
            }
        }

        internal List<int> LastLogonDays
        {
            get
            {
                return m_lastLogonDays;
            }
        }
    }
}
