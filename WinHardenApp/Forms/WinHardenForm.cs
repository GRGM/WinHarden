using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WinHardenApp.AnalyzeInformationUtils;
using WinHardenApp.Configuration;
using System.DirectoryServices.AccountManagement;
using InformationUtils;
using static WinHardenApp.Forms.FindForm;


namespace WinHardenApp.Forms
{
    public partial class WinHardenForm : Form
    {

        private const int s_maxLengthFolder = 214;
        private const int s_maximumAnalysisProgressBar = 26;

        private bool m_isSearch = false;
        private WindowsInformationUtils m_windowsInformationUtils;


        private TreeView m_treeViewCopy=new TreeView();
        private bool m_isFilteredTreeView = false;

        public WinHardenForm()
        {
            InitializeComponent();
            if (!ExtractWindowsInformationUtils.IsAdministrator())
            {
                isAdministratorToolStripLabel.Visible = false;
            }
            Text = "New - WinHarden";
            TreeViewUtils.ClearTreeView(filesTreeView);
            m_windowsInformationUtils = new WindowsInformationUtils(extractionToolStripLabel, extractProgressBar);
            WinHardenConfiguration.InitWinHardenConfiguration();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }





        private void UpdateTreeView(string folderPath, string typeFolder)
        {
            string hostnameFile = folderPath + @"\hostname.txt";
            //This case happens with hostname file does not exist due to file are going to be generated in the folders
            if (!File.Exists(hostnameFile))
            {
                return;
            }
            string workstation = File.ReadAllText(hostnameFile).Trim();
            filesTreeView.BeginUpdate();
            TreeNode workstationNode = TreeViewUtils.GetNode(filesTreeView.Nodes, workstation);
            UpdateNodes(workstationNode, folderPath, typeFolder);
            filesTreeView.EndUpdate();
        }



        private void SetTreeView(TreeView treeView)
        {
            filesTreeView.BeginUpdate();

            filesTreeView.Nodes.Clear();
            foreach (TreeNode originalNode in treeView.Nodes)
            {
                if (originalNode.Level != 0)
                {
                    continue;
                }
                TreeNode newNode = filesTreeView.Nodes.Add(originalNode.Text, originalNode.Text);
                newNode.ToolTipText = originalNode.ToolTipText;

                IterateTreeNodes(originalNode, newNode);
            }

            filesTreeView.EndUpdate();
        }


        private void IterateTreeNodes(TreeNode originalNode, TreeNode rootNode)
        {
            foreach (TreeNode childNode in originalNode.Nodes)
            {
                TreeNode newNode = rootNode.Nodes.Add(childNode.Text, childNode.Text);
                newNode.ToolTipText = childNode.ToolTipText;

                IterateTreeNodes(childNode, newNode);
            }
            if (rootNode.Level == 1)
            {
                UpdateFileNodes(rootNode);
            }
        }



        private void UpdateNodes(TreeNode workstationNode, string folder, string typeFolder)
        {
            if (folder.Trim() == "")
            {
                return;
            }
            TreeNode node = TreeViewUtils.GetNode(workstationNode.Nodes, typeFolder);
            node.ToolTipText = folder;
            UpdateFileNodes(node);
        }

        private void UpdateFileNodes(TreeNode folderNode)
        {
            string folder = folderNode.ToolTipText;
            if(!Directory.Exists(folder))
            {
                return;
            }
            DirectoryInfo directory = new DirectoryInfo(folder);
            FileInfo[] files = directory.GetFiles();
            folderNode.Nodes.Clear();
            foreach (FileInfo file in files)
            {
                if (WinHardenConfiguration.Configuration.WindowOptionsConfiguration.HideEmptyFiles && file.Length == 0)
                {
                    continue;
                }
                TreeNode fileNode = folderNode.Nodes.Add(file.Name, file.Name);
                fileNode.ToolTipText = file.FullName;
                if (WinHardenConfiguration.Configuration.WindowOptionsConfiguration.ColorFiles && file.Length != 0)
                {
                    fileNode.BackColor = Color.LightGreen;
                }
            }
        }


        private void saveExtractionButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fileDialog = new FolderBrowserDialog();
            fileDialog.ShowDialog();
            extractionOutputFolderTextBox.Text = fileDialog.SelectedPath;
            UpdateTreeView(extractionOutputFolderTextBox.Text, WinHardenConfiguration.ExtractionLabel);
        }

        private void saveAnalysisButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fileDialog = new FolderBrowserDialog();
            fileDialog.ShowDialog();
            analysisOutputFolderTextBox.Text = fileDialog.SelectedPath;
            UpdateTreeView(analysisOutputFolderTextBox.Text, WinHardenConfiguration.AnalysisLabel);
        }

        private void complianceOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComplianceConfigurationForm complianceConfigurationForm = new ComplianceConfigurationForm();
            complianceConfigurationForm.ShowDialog();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            string fileName = fileDialog.FileName;
            bool isSet = WinHardenConfiguration.SetConfiguration(fileName);
            if (!isSet)
            {
                return;
            }
            //We copy the working folders in the TreeView
            if (WinHardenConfiguration.Configuration.TreeViewConfiguration != null)
            {
                SetTreeView(WinHardenConfiguration.Configuration.TreeViewConfiguration.FilesTreeView);
            }
            Text = fileDialog.FileName + " - WinHarden";

        }

        private void extractToolStripButton_Click(object sender, EventArgs e)
        {
            Extract();
        }
        private void Extract()
        {
            bool isError = false;
            string outputFolder = WindowsInformationUtils.CheckFolder(extractionOutputFolderTextBox.Text);
            if (extractionOutputFolderTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Configuration input folder has not been selected.");
                return;
            }
            if (extractionOutputFolderTextBox.Text.Length > s_maxLengthFolder)
            {
                MessageBox.Show("Configuration input folder has a too long name. Please select other folder");
                return;
            }
            if (outputFolder is null)
            {
                MessageBox.Show("Extraction input folder does not exist");
                return;
            }
            string inputFile = inputFileTextBox.Text;
            if (inputFile.Trim() != "")
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
                string registryKeyPolicyName = WinHardenConfiguration.Configuration.ComplianceConfiguration.SecurityPolicyConfigruation.RegistryKeyPolicy;
                extractWindowsInformationUtils.ExtractWindowsInformation(registryKeyPolicyName);

                if (inputFile.Trim() != "")
                {
                    if (!ExtractWindowsInformationUtils.IsAdministrator())
                    {
                        MessageBox.Show("Retrieve audit configuration requires executing this program under administrator privileges.");
                        return;
                    }
                    extractionToolStripLabel.Text = "Permissions and audit configuration";
                    ExtractWindowsInformationUtils.ExtractPermissionsAndAuditConfiguration(outputFolder, inputFile);
                }
                UpdateTreeView(extractionOutputFolderTextBox.Text, WinHardenConfiguration.ExtractionLabel);
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

        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Extract();
        }
        private void SaveConfiguration()
        {
            if (WinHardenConfiguration.Configuration.ConfigurationFile == null)
            {
                SaveAsConfiguration();
            }
            else
            {
                WinHardenConfiguration.UpdateTreeView(filesTreeView);
                WinHardenConfiguration.SaveConfiguration();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveConfiguration();
        }

        private void savesAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAsConfiguration();
        }

        private void SaveAsConfiguration()
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }
            string fileName = fileDialog.FileName;
            if (fileName.Trim() == "")
            {
                return;
            }
            WinHardenConfiguration.UpdateConfigurationFile(fileName);
            WinHardenConfiguration.UpdateTreeView(filesTreeView);
            WinHardenConfiguration.SaveConfiguration();
            Text = fileDialog.FileName + " - WinHarden";
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WinHardenConfiguration.Close();
            Text = "New - WinHarden";
            extractionOutputFolderTextBox.Text = "";
            analysisOutputFolderTextBox.Text = "";
            TreeViewUtils.ClearTreeView(filesTreeView);
            filesTabControl.TabPages.Clear();

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Analyse()
        {
            bool isError = false;
            try
            {

                if (extractionOutputFolderTextBox.Text.Trim() == "")
                {
                    MessageBox.Show("Configuration input folder has not been selected.");
                    return;
                }
                if (analysisOutputFolderTextBox.Text.Trim() == "")
                {
                    MessageBox.Show("Analysis output folder has not been selected.");
                    return;
                }

                if (analysisOutputFolderTextBox.Text.Length > s_maxLengthFolder)
                {
                    MessageBox.Show("Analysis output folder has a too long name. Please select other folder");
                    return;
                }

                string configurationFolder = WindowsInformationUtils.CheckFolder(extractionOutputFolderTextBox.Text);
                if (configurationFolder is null)
                {
                    MessageBox.Show("Configuration input folder does not exist");
                    return;
                }
                string analysisOutputFolder = WindowsInformationUtils.CheckFolder(analysisOutputFolderTextBox.Text);
                if (analysisOutputFolder is null)
                {
                    MessageBox.Show("Analysis output folder does not exist");
                    return;
                }
                m_windowsInformationUtils.InitProgressBar(s_maximumAnalysisProgressBar);

                AnalyseWindowsInformationUtils analyseWindowsInformationUtils = new AnalyseWindowsInformationUtils(configurationFolder, analysisOutputFolder, m_windowsInformationUtils);
                AnalyseComplianceInformationUtils complianceUtils = new AnalyseComplianceInformationUtils(configurationFolder, analysisOutputFolder, WinHardenConfiguration.Configuration, m_windowsInformationUtils);
                LogUtils.InitLogFile(analysisOutputFolder + @"\log_analysis.txt");
                analyseWindowsInformationUtils.AnalyseWindowsInformation();
                complianceUtils.AnalyseComplianceInformation();
                UpdateTreeView(analysisOutputFolderTextBox.Text, WinHardenConfiguration.AnalysisLabel);

                if(File.Exists(LogUtils.LogFile))
                {
                    MessageBox.Show("Analysis has included some error. See log_analysis.txt file");
                    OpenFileTab(LogUtils.LogFile);
                }            
            }
            catch (Exception exception)
            {
                string exceptionInformation = "Message: " + exception.Message + Environment.NewLine +
                            "Source: " + exception.Source + Environment.NewLine +
                            "Stack Trace: " + exception.StackTrace + Environment.NewLine;
                File.AppendAllText(analysisOutputFolderTextBox.Text + @"\log_analysis.txt", exceptionInformation);
                MessageBox.Show("Analysis has not been completed. Error: " + exception.Message);
                isError = true;
            }
            finally
            {
                if (!isError)
                {
                    extractionToolStripLabel.Text = "Completed analysis.";
                }
            }
        }

        private void analyseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Analyse();
        }

        private void analyseToolStripButton_Click(object sender, EventArgs e)
        {
            Analyse();

        }

        private void extractTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtractRecommendationsForm extractRecommendationsForm = new ExtractRecommendationsForm();
            extractRecommendationsForm.Show();
        }

        private void extractionOutputFolderTextBox_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(extractionOutputFolderTextBox.Text))
            {
                UpdateTreeView(extractionOutputFolderTextBox.Text, WinHardenConfiguration.ExtractionLabel);
            }
        }

        private void analysisOutputFolderTextBox_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(analysisOutputFolderTextBox.Text))
            {
                UpdateTreeView(analysisOutputFolderTextBox.Text, WinHardenConfiguration.AnalysisLabel);
            }
        }
        internal void UpdateFullTreeView()
        {
            if (Directory.Exists(extractionOutputFolderTextBox.Text))
            {
                UpdateTreeView(extractionOutputFolderTextBox.Text, WinHardenConfiguration.ExtractionLabel);
            }
            if (Directory.Exists(analysisOutputFolderTextBox.Text))
            {
                UpdateTreeView(analysisOutputFolderTextBox.Text, WinHardenConfiguration.AnalysisLabel);
            }
        }

        private void filesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = e.Node;


            if (selectedNode.Level == 1)
            {
                if(selectedNode.Text.ToUpper() == WinHardenConfiguration.AnalysisLabel.ToUpper())
                {
                    analysisOutputFolderTextBox.Text= selectedNode.ToolTipText;
                    return;
                }
                if (selectedNode.Text.ToUpper() == WinHardenConfiguration.ExtractionLabel.ToUpper())
                {
                    extractionOutputFolderTextBox.Text = selectedNode.ToolTipText;
                    return;
                }
            }
            if (selectedNode.Level == 2)
            {
                //In this case, node is file. We read the data to show it in the textbox.
                OpenFileTab(selectedNode.ToolTipText);
                return;
            }

        }


        private void filesTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                filesTreeView.Nodes.Remove(filesTreeView.SelectedNode);
            }

        }

        private void OpenFileTab(string fileName)
        {
            //If file is open, we focus on it
            foreach (TabPage tabPage in filesTabControl.TabPages)
            {
                if (tabPage.ToolTipText == fileName)
                {
                    filesTabControl.SelectTab(tabPage);
                    return;
                }
            }
            //If file is not open, we add a new tab page
            if(!File.Exists(fileName))
            {
                MessageBox.Show("File does not exist:"+ fileName);
                return;
            }
            FileInfo file = new FileInfo(fileName);
            TabPage newTabPage = new TabPage(file.Name);
            newTabPage.ToolTipText = file.FullName;

            RichTextBox richTextBox = new RichTextBox();
            richTextBox.TextChanged += RichTextBox_TextChanged;

            //string text = File.ReadAllText(fileName, Encoding.Default);
            //We use this way to read files that are open.
            try
            {
                FileStream inputFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader inputFile = new StreamReader(inputFileStream, Encoding.Default);
                string text = inputFile.ReadToEnd();
                //We do this replacing because RichTexBox does not show \r\r\n of auditpol as it is expecte
       
                text = text.Replace("\r\r\n", "\r\n\r\n");

                richTextBox.Text = text;
                Size size = new Size(filesTabControl.Size.Width, filesTabControl.Size.Height - 20);
                newTabPage.Size = size;
                richTextBox.Size = size;

                richTextBox.Multiline = true;
                richTextBox.WordWrap = false;
                richTextBox.ScrollBars = RichTextBoxScrollBars.Both;

                newTabPage.Controls.Add(richTextBox);
                filesTabControl.TabPages.Add(newTabPage);
                filesTabControl.SelectTab(newTabPage);

                if (WinHardenConfiguration.Configuration.WindowOptionsConfiguration.ColorEveryone)
                {
                    ColorFindText("EVERYONE");
                }
                inputFile.Close();
            }
            catch
            {
                MessageBox.Show("Error during opening the file");
            }
        }


        private void RichTextBox_TextChanged(object sender, EventArgs e)
        {

            TabPage selectedTab = filesTabControl.SelectedTab;
            if (!m_isSearch && !selectedTab.Text.EndsWith("*"))
            {
                selectedTab.Text += "*";
            }

        }

        private void saveFileToolStripButton_Click(object sender, EventArgs e)
        {
            TabPage selectedTab = filesTabControl.SelectedTab;

            if (selectedTab == null)
            {
                MessageBox.Show("There is no open file to save.");
                return;
            }
            //We only save if there is modified data
            if (selectedTab.Text.EndsWith("*"))
            {
                RichTextBox richTextBox = selectedTab.Controls[0] as RichTextBox;
                string filePath = selectedTab.ToolTipText;
                try
                {
                    FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                    fs.Close();
                    File.WriteAllText(selectedTab.ToolTipText, richTextBox.Text, Encoding.Default);
                }
                catch (IOException)
                {
                    MessageBox.Show("File is open by other process.");
                }
                selectedTab.Text = selectedTab.Text.Replace("*", "");
            }
        }




        private void ColorFindText()
        {
            string searchText = searchFileToolStripTextBox.Text.ToUpper();
            ColorFindText(searchText);
        }


        private void ColorFindText(string searchText)
        {
            TabPage selectedTab = filesTabControl.SelectedTab;
            RichTextBox richTextBox = selectedTab.Controls[0] as RichTextBox;

            string fileText = richTextBox.Text.ToUpper();
            string auxFileText = richTextBox.Text;

            richTextBox.ResetText();
            richTextBox.Text = auxFileText;

            if (searchText == "")
            {
                return;
            }

            int index = fileText.IndexOf(searchText);
            while (index >= 0)
            {
                richTextBox.SelectionStart = index;
                richTextBox.SelectionLength = searchText.Length;
                richTextBox.SelectionBackColor = Color.LightBlue;
                if ((index + 1) >= fileText.Length)
                {
                    return;
                }
                index = fileText.IndexOf(searchText, index + 1);
            }
        }



        private void searchFileToolStripButton_Click(object sender, EventArgs e)
        {
            m_isSearch = true;
            ColorFindText();
            SearchUtils searchUtils = new SearchUtils(searchTreeView);
            string searchText = searchFileToolStripTextBox.Text;
            TabPage selectedTab = filesTabControl.SelectedTab;
            searchUtils.SearchCurrentFileTreeView(searchText, selectedTab);
            m_isSearch = false;
        }

        private void searchFileToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            m_isSearch = true;
            ColorFindText();
            m_isSearch = false;
        }

        private void SelectLineRichTexbox(int numberLine)

        {
            TabPage selectedTab = filesTabControl.SelectedTab;
            RichTextBox richTextBox = selectedTab.Controls[0] as RichTextBox;

            string fileText = richTextBox.Text;

            int offset = 0;
            int lineLenght = -1;
            int i = 0;
            foreach (string line in richTextBox.Lines)
            {
                offset += lineLenght + 1;
                lineLenght = line.Length;
                if (i >= numberLine)
                {
                    break;
                }
                i++;
            }

            richTextBox.ResetText();
            richTextBox.Text = fileText;

            richTextBox.SelectionStart = offset;
            richTextBox.SelectionLength = lineLenght;
            richTextBox.SelectionBackColor = Color.LightGreen;

            richTextBox.Focus();

        }

        private void searchTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {

            TreeNode selectedNode = e.Node;
            //If selected node is a parent folder ndoe, we ignore it
            if (!selectedNode.Text.StartsWith("Line"))
            {
                return;
            }
            string line = selectedNode.Text;
            //We remove "Line "
            line = line.Substring(5).Trim();

            string[] separator = new string[1] { @":" };
            string[] fields = line.Split(separator, StringSplitOptions.None);
            int number = int.Parse(fields[0]);
            m_isSearch = true;
            TreeNode parentNode = selectedNode.Parent;
            OpenFileTab(parentNode.Text);
            SelectLineRichTexbox(number);
            m_isSearch = false;
        }

        private void closeToolStripButton_Click(object sender, EventArgs e)
        {

            TabPage selectedTab = filesTabControl.SelectedTab;
            if(selectedTab is null)
            {
                return;
            }
            if (selectedTab.Text.EndsWith("*"))
            {
                DialogResult result = MessageBox.Show("Do you want save the file before closing?", "Close file", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    RichTextBox richTextBox = selectedTab.Controls[0] as RichTextBox;
                    File.WriteAllText(selectedTab.ToolTipText, richTextBox.Text, Encoding.Default);
                }
                if (result == DialogResult.Cancel)
                {
                    //Tab page is not removed
                    return;
                }
            }
            //We unselect the node to allow selectecing the same node that has been just closed
            filesTreeView.SelectedNode = null;
            //Tab page is removed of result is YES or NOT
            filesTabControl.TabPages.Remove(selectedTab);
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindForm findForm = new FindForm(SearchType.AllOpenFiles, this);
            findForm.Show();
        }

        private void activeDirectoryOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServerDomainConfigurationForm serverDomainConfigurationForm = new ServerDomainConfigurationForm(filesTreeView);
            serverDomainConfigurationForm.ShowDialog();
        }



        private void connectDomainServerToolStripButton_Click(object sender, EventArgs e)
        {
            PrincipalContext context;
            string connectedServer = WinHardenConfiguration.Configuration.ServerDomainConfiguration.ConnectedServer;
            try
            {

                context = new PrincipalContext(ContextType.Domain, WinHardenConfiguration.Configuration.ServerDomainConfiguration.ConnectedServer);
                MessageBox.Show("Successful domain server connection: " + connectedServer);
            }
            catch
            {
                MessageBox.Show("Failed domain server connection: " + connectedServer);
            }
        }

        internal TreeView SearchTreeView
        {
            get
            {
                return searchTreeView;
            }
        }

        internal TabControl FilesTabControl
        {
            get
            {
                return filesTabControl;
            }
        }

        internal TreeView FilesTreeView
        {
            get
            {
                return filesTreeView;
            }
        }

        private void findInFilesToolStripButton_Click(object sender, EventArgs e)
        {
            FindForm findForm = new FindForm(SearchType.AllOpenFiles, this);
            findForm.ShowDialog();
        }

        private void windowOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowOptionsForm windowOptionsForm = new WindowOptionsForm(this);
            windowOptionsForm.ShowDialog();
        }

        private void inputPermissionAuditButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            inputFileTextBox.Text = openFileDialog.FileName;
        }

        private void openGridButton_Click(object sender, EventArgs e)
        {
            TabPage selectedTab = filesTabControl.SelectedTab;

            if (selectedTab == null)
            {
                MessageBox.Show("There is no open file to show.");
                return;
            }
            string filePath = selectedTab.ToolTipText.Replace("*", "").ToUpper();
            //We only save if there is modified data
            if (!filePath.EndsWith(".CSV"))
            {
                MessageBox.Show("Select a .CSV file.");
                return;
            }
            DataGridForm dataGridForm = new DataGridForm(filePath);
            dataGridForm.ShowDialog();
        }


        private void RecoverTreeView()
        {
            //We recover previous TreeVew
            filesTreeView.BeginUpdate();
            filesTreeView.Nodes.Clear();
            foreach (TreeNode node in m_treeViewCopy.Nodes)
            {
                TreeNode nodeCopy = (TreeNode)node.Clone();
                filesTreeView.Nodes.Add(nodeCopy);
            }
            filesTreeView.EndUpdate();
            m_isFilteredTreeView = false;
        }

        private void searchTreeViewButton_Click(object sender, EventArgs e)
        {
   
            string filterText = searchTreeTextBox.Text.Trim().ToUpper();
            if(filterText=="")
            {
                if (m_isFilteredTreeView)
                {
                    RecoverTreeView();
                }
                return;
            }
            if (!m_isFilteredTreeView)
            {
                m_treeViewCopy = new TreeView();
                foreach (TreeNode node in filesTreeView.Nodes)
                {
                    TreeNode nodeCopy = (TreeNode)node.Clone();
                    m_treeViewCopy.Nodes.Add(nodeCopy);
                }
            }
            m_isFilteredTreeView = true;
            TreeView filterTreeView = new TreeView();

            foreach (TreeNode workstationNode in m_treeViewCopy.Nodes)
            {
                foreach (TreeNode subNode in workstationNode.Nodes)
                {
                    foreach (TreeNode subSubNode in subNode.Nodes)
                    {
                        if (subSubNode.Text.ToUpper().Contains(filterText))
                        {
                            TreeNode levelOneNode = subSubNode.Parent;
                            TreeNode levelZeroNode= levelOneNode.Parent;
                            TreeNode levelZeroNodeCopy=TreeViewUtils.GetNode(filterTreeView.Nodes, levelZeroNode.Name, levelZeroNode.ToolTipText);
                            TreeNode levelOneNodeCopy=TreeViewUtils.GetNode(levelZeroNodeCopy.Nodes, levelOneNode.Name, levelOneNode.ToolTipText);
                            TreeViewUtils.GetNode(levelOneNodeCopy.Nodes, subSubNode.Name, subSubNode.ToolTipText);
                        }
                    }
                }
            }

            filesTreeView.BeginUpdate();
            filesTreeView.Nodes.Clear();
            foreach (TreeNode node in filterTreeView.Nodes)
            {
                TreeNode nodeCopy = (TreeNode)node.Clone();
                filesTreeView.Nodes.Add(nodeCopy);
            }
            filesTreeView.ExpandAll();
            filesTreeView.EndUpdate();

        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveConfiguration();
        }
    }
}
