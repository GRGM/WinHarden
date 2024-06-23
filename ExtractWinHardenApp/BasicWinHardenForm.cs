using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InformationUtils;
using System.IO;


namespace ExtractWinHardenApp
{
    public partial class BasicWinHardenForm : Form
    {
        private WindowsInformationUtils m_windowsInformationUtils;
        public BasicWinHardenForm()
        {
            InitializeComponent();
            if (!ExtractWindowsInformationUtils.IsAdministrator())
            {
                isAdministratorToolStripLabel.Visible = false;
            }
            m_windowsInformationUtils = new WindowsInformationUtils(extractionToolStripLabel, extractProgressBar);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveExtractionButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fileDialog = new FolderBrowserDialog();
            fileDialog.ShowDialog();
            extractionOutputFolderTextBox.Text = fileDialog.SelectedPath;
        }

        private void extractButton_Click(object sender, EventArgs e)
        {
            bool isError = false;
            if (extractionOutputFolderTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Configuration input folder has not been selected.");
                return;
            }
            string outputFolder = WindowsInformationUtils.CheckFolder(extractionOutputFolderTextBox.Text);
            if (outputFolder is null)
            {
                MessageBox.Show("Extraction input folder does not exist");
                return;
            }
            string inputFile = inputFileTextBox.Text;
            if (inputFile.Trim()!="")
            {
                if (!File.Exists(inputFile))
                {
                    MessageBox.Show("Input file with folder to retrieve permissions and audit configuration does not exist");
                    return;
                }
            }
            try
            {
                ExtractWindowsInformationUtils extractWindowsInformationUtils = new ExtractWindowsInformationUtils(outputFolder, m_windowsInformationUtils);
                extractWindowsInformationUtils.ExtractWindowsInformation();
                if(inputFile.Trim() != "")
                {
                    if(!ExtractWindowsInformationUtils.IsAdministrator())
                    {
                        MessageBox.Show("Retrieve audit configuration requires executing this program under administrator privileges.");
                        return;
                    }
                    extractionToolStripLabel.Text = "Permissions and audit configuration";
                    ExtractWindowsInformationUtils.ExtractPermissionsAndAuditConfiguration(outputFolder, inputFile);
                }
            }
            catch (Exception exception)
            {
                string exceptionInformation = "Message: " + exception.Message + Environment.NewLine +
                            "Source: " + exception.Source + Environment.NewLine +
                            "Stack Trace: " + exception.StackTrace + Environment.NewLine;
                File.AppendAllText(outputFolder + @"\log.txt", exceptionInformation);
                MessageBox.Show("Error: " + exception.Message);
                isError = true;
            }
            finally
            {
                if (!isError)
                {
                    extractionToolStripLabel.Text = "Completed extraction.";
                }
            }
        }

        private void inputPermissionAuditButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            inputFileTextBox.Text = openFileDialog.FileName;
        }



        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
