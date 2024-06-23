using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinHardenApp.Configuration;
using System.IO;

namespace WinHardenApp.Forms
{
    public partial class ComplianceConfigurationForm : Form
    {
        internal ComplianceConfigurationForm()
        {
            InitializeComponent();
            ComplianceConfiguration complianceConfiguration = WinHardenConfiguration.Configuration.ComplianceConfiguration;


            AuditPolicyConfiguration auditPolicyConfiguration = complianceConfiguration.AuditPolicyConfiguration;
            switch (auditPolicyConfiguration.AuditPolicyComplianceType)
            {
                case AuditPolicyComplianceType.WorkstationRecommendations:
                    workstationRadioButton.Checked = true;
                    break;
                case AuditPolicyComplianceType.ServerRecommendations:
                    serverRadioButton.Checked = true;
                    break;
                case AuditPolicyComplianceType.UserDefinedRecommendations:
                    userDefinedRadioButton.Checked = true;
                    userDefinedTextBox.Text = auditPolicyConfiguration.UserDefinedRecommendationsFile;
                    break;
            }

            PasswordPolicyConfiguration passwordPolicyConfiguration = complianceConfiguration.PasswordPolicyConfiguration;
            lastPasswordDaysTextBox.Text = StringUtils.GetStringFromListInt(passwordPolicyConfiguration.LastPasswordDays);
            lastLogonDaysTextBox.Text = StringUtils.GetStringFromListInt(passwordPolicyConfiguration.LastLogonDays);

            SecurityPolicyConfiguration securityPolicyConfiguration = complianceConfiguration.SecurityPolicyConfigruation;
            securityPolicyTextBox.Text = securityPolicyConfiguration.SecurityPolicyFile;
            translatedSIDCheckBox.Checked = securityPolicyConfiguration.IsTranslatedSID;
            registryKeyPolicyTextBox.Text = securityPolicyConfiguration.RegistryKeyPolicy;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            AuditPolicyComplianceType auditPolicyComplianceType = AuditPolicyComplianceType.WorkstationRecommendations;
            string userDefinedRecommendationsFile = "";

            if(workstationRadioButton.Checked)
            {
                auditPolicyComplianceType = AuditPolicyComplianceType.WorkstationRecommendations;
            }
            if (serverRadioButton.Checked)
            {
                auditPolicyComplianceType = AuditPolicyComplianceType.ServerRecommendations;
            }
            if (userDefinedRadioButton.Checked)
            {
                auditPolicyComplianceType = AuditPolicyComplianceType.UserDefinedRecommendations;
                userDefinedRecommendationsFile = userDefinedTextBox.Text;
                if (!File.Exists(userDefinedRecommendationsFile))
                {
                    MessageBox.Show("Compliance configuration has not been defined. User defined recommendations file does not exist.");
                    return;
                }
            }
            List<int> lastPasswordDays = null;
            List<int> lastLogonDays = null;
            if (lastPasswordDaysTextBox.Text.Trim()!="")
            {
                lastPasswordDays = StringUtils.GetListInt(lastPasswordDaysTextBox.Text);
                if(lastPasswordDays==null)
                {
                    MessageBox.Show("Compliance configuration has not been defined. Incorrect format input for list of last password days.");
                    return;
                }
            }
            if (lastLogonDaysTextBox.Text.Trim() != "")
            {
                lastLogonDays = StringUtils.GetListInt(lastLogonDaysTextBox.Text);
                if (lastLogonDays == null)
                {
                    MessageBox.Show("Compliance configuration has not been defined. Incorrect format input for list of last logon days.");
                    return;
                }
            }
            string securityPolicyFile = securityPolicyTextBox.Text;
            if (securityPolicyFile.Trim()!="" && !File.Exists(securityPolicyFile))
            {
                MessageBox.Show("Compliance configuration has not been defined. Selected secuirity policy template file does not exist.");
                return;
            }
            bool isTranslatedSID=translatedSIDCheckBox.Checked;


            string registryKeyPolicy = registryKeyPolicyTextBox.Text;
            if (registryKeyPolicy.Trim() != "" && !File.Exists(registryKeyPolicy))
            {
                MessageBox.Show("Compliance configuration has not been defined. Selected registry key policy template file does not exist.");
                return;
            }


            AuditPolicyConfiguration auditPolicyConfiguration = new AuditPolicyConfiguration(auditPolicyComplianceType, userDefinedRecommendationsFile);
            PasswordPolicyConfiguration passwordPolicyConfiguration = new PasswordPolicyConfiguration(lastPasswordDays, lastLogonDays);
            SecurityPolicyConfiguration securityPolicyConfiguration = new SecurityPolicyConfiguration(securityPolicyFile, isTranslatedSID, registryKeyPolicy);

            WinHardenConfiguration.UpdateComplianceConfiguration(auditPolicyConfiguration, passwordPolicyConfiguration, securityPolicyConfiguration);
            MessageBox.Show("Compliance configuration has been defined");
            Close();
        }

        private void openUserDefinedPolicyButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result= openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                userDefinedRadioButton.Checked = true;
            }
            userDefinedTextBox.Text = openFileDialog.FileName;
        }

        private void openSecurityPolicyButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            securityPolicyTextBox.Text = openFileDialog.FileName;
        }

        private void openRegistryKeyPolicyButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            registryKeyPolicyTextBox.Text = openFileDialog.FileName;
        }
    }
}
