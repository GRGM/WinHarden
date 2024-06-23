using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using InformationUtils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinHardenApp.Forms
{
    public partial class ExtractRecommendationsForm : Form
    {
        public ExtractRecommendationsForm()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private string SaveFileDialog(string auxFileName)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
    
            if (Directory.Exists(auxFileName))
            {
                fileDialog.InitialDirectory = auxFileName;
            }
            else
            {
                if (auxFileName.Trim() != "")
                {
                    FileInfo file = new FileInfo(auxFileName);
                    string folder = file.DirectoryName;
                    if (Directory.Exists(folder))
                    {
                        fileDialog.InitialDirectory = folder;
                        fileDialog.FileName = auxFileName;
                    }
                    else
                    {
                        MessageBox.Show("Select a file to save the template.");
                        return null;
                    }
                }

            }

            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return null;
            }

            string fileName = fileDialog.FileName;
            if (fileName.Trim() == "")
            {
                return null;
            }
            return fileName;
        }

        private void saveAuditPolicyButton_Click(object sender, EventArgs e)
        {
            string fileName=SaveFileDialog(pathTextBox.Text);
            if(fileName is null)
            {
                return;
            }

            StreamWriter configurationtFile = new StreamWriter(fileName, false, Encoding.Default);
            string resource = null;
            if(workstationRadioButton.Checked)
            {
                resource = Resource1.auditpolicy_WORKSTATION;
            }
            if(serverRadioButton.Checked)
            {
                resource = Resource1.auditpolicy_SERVER;
            }

            string line;
            using (StreamReader ms = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(resource))))
            {
                while ((line = ms.ReadLine()) != null)
                {
                    configurationtFile.WriteLine(line);
                }
            }

            configurationtFile.Close();
            MessageBox.Show("Template has been extracted.");
        }

        private void saveRegistryPolicyFileButton_Click(object sender, EventArgs e)
        {
            string fileName = SaveFileDialog(registryPolicyFileTextBox.Text);
            if (fileName is null)
            {
                return;
            }
            RegistryUtils.SaveDefaultRegistryPolicyFile(fileName);
            MessageBox.Show("Registry policy template has been extracted.");
        }

        private void saveSecurityPolicyFileButton_Click(object sender, EventArgs e)
        {
            string fileName = SaveFileDialog(securityPolicyFileTextBox.Text);
            if (fileName is null)
            {
                return;
            }
            SecurityPolicyUtils.SaveDefaultSecurityPolicyFile(fileName);
            MessageBox.Show("Security policy template has been extracted.");
        }


    }
}
