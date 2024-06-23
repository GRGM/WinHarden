using System.Windows.Forms;

namespace WinHardenApp.Forms
{
    partial class WinHardenForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinHardenForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.extractionOutputFolderTextBox = new System.Windows.Forms.TextBox();
            this.inputPermissionAuditButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.saveExtractionButton = new System.Windows.Forms.Button();
            this.inputFileTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.saveAnalysisButton = new System.Windows.Forms.Button();
            this.analysisOutputFolderTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savesAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findInFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analyseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.complianceOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.activeDirectoryOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutWinHardenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.extractToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.analyseToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.isAdministratorToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.connectDomainServerToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.findInFilesToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.extractionToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.extractProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.filesTreeView = new System.Windows.Forms.TreeView();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.saveFileToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.openGridButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.searchFileToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.searchFileToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.searchTreeView = new System.Windows.Forms.TreeView();
            this.filesTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.formatCSVFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchTreeTextBox = new System.Windows.Forms.TextBox();
            this.searchTreeViewButton = new System.Windows.Forms.Button();
            this.windowOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.extractionOutputFolderTextBox);
            this.groupBox1.Controls.Add(this.inputPermissionAuditButton);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.saveExtractionButton);
            this.groupBox1.Controls.Add(this.inputFileTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 65);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1503, 74);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Extracting folders";
            // 
            // extractionOutputFolderTextBox
            // 
            this.extractionOutputFolderTextBox.Location = new System.Drawing.Point(189, 15);
            this.extractionOutputFolderTextBox.Name = "extractionOutputFolderTextBox";
            this.extractionOutputFolderTextBox.Size = new System.Drawing.Size(1213, 20);
            this.extractionOutputFolderTextBox.TabIndex = 2;
            this.extractionOutputFolderTextBox.TextChanged += new System.EventHandler(this.extractionOutputFolderTextBox_TextChanged);
            // 
            // inputPermissionAuditButton
            // 
            this.inputPermissionAuditButton.Location = new System.Drawing.Point(1414, 45);
            this.inputPermissionAuditButton.Name = "inputPermissionAuditButton";
            this.inputPermissionAuditButton.Size = new System.Drawing.Size(75, 23);
            this.inputPermissionAuditButton.TabIndex = 39;
            this.inputPermissionAuditButton.Text = "Open";
            this.inputPermissionAuditButton.UseVisualStyleBackColor = true;
            this.inputPermissionAuditButton.Click += new System.EventHandler(this.inputPermissionAuditButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select extraction output folder";
            // 
            // saveExtractionButton
            // 
            this.saveExtractionButton.Location = new System.Drawing.Point(1414, 15);
            this.saveExtractionButton.Name = "saveExtractionButton";
            this.saveExtractionButton.Size = new System.Drawing.Size(75, 23);
            this.saveExtractionButton.TabIndex = 0;
            this.saveExtractionButton.Text = "Open";
            this.saveExtractionButton.UseVisualStyleBackColor = true;
            this.saveExtractionButton.Click += new System.EventHandler(this.saveExtractionButton_Click);
            // 
            // inputFileTextBox
            // 
            this.inputFileTextBox.Location = new System.Drawing.Point(189, 46);
            this.inputFileTextBox.Name = "inputFileTextBox";
            this.inputFileTextBox.Size = new System.Drawing.Size(1213, 20);
            this.inputFileTextBox.TabIndex = 40;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 13);
            this.label2.TabIndex = 38;
            this.label2.Text = "Select permissions and audit input file";
            // 
            // saveAnalysisButton
            // 
            this.saveAnalysisButton.Location = new System.Drawing.Point(1426, 145);
            this.saveAnalysisButton.Name = "saveAnalysisButton";
            this.saveAnalysisButton.Size = new System.Drawing.Size(75, 23);
            this.saveAnalysisButton.TabIndex = 10;
            this.saveAnalysisButton.Text = "Open";
            this.saveAnalysisButton.UseVisualStyleBackColor = true;
            this.saveAnalysisButton.Click += new System.EventHandler(this.saveAnalysisButton_Click);
            // 
            // analysisOutputFolderTextBox
            // 
            this.analysisOutputFolderTextBox.Location = new System.Drawing.Point(201, 145);
            this.analysisOutputFolderTextBox.Name = "analysisOutputFolderTextBox";
            this.analysisOutputFolderTextBox.Size = new System.Drawing.Size(1213, 20);
            this.analysisOutputFolderTextBox.TabIndex = 9;
            this.analysisOutputFolderTextBox.TextChanged += new System.EventHandler(this.analysisOutputFolderTextBox_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(139, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Select analysis output folder";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.actionsToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(1537, 24);
            this.menuStrip1.TabIndex = 29;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.savesAsToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(188, 30);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::WinHardenApp.Resource1.SaveStatusBar1_16x;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(188, 30);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // savesAsToolStripMenuItem
            // 
            this.savesAsToolStripMenuItem.Name = "savesAsToolStripMenuItem";
            this.savesAsToolStripMenuItem.Size = new System.Drawing.Size(188, 30);
            this.savesAsToolStripMenuItem.Text = "Saves as";
            this.savesAsToolStripMenuItem.Click += new System.EventHandler(this.savesAsToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(188, 30);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(188, 30);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findToolStripMenuItem,
            this.findInFileToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(54, 22);
            this.editToolStripMenuItem.Text = "Search";
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Image = global::WinHardenApp.Resource1.SearchFolderClosed_16x;
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findToolStripMenuItem.Size = new System.Drawing.Size(188, 30);
            this.findToolStripMenuItem.Text = "Find in files";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // findInFileToolStripMenuItem
            // 
            this.findInFileToolStripMenuItem.Image = global::WinHardenApp.Resource1.SearchContract_16x;
            this.findInFileToolStripMenuItem.Name = "findInFileToolStripMenuItem";
            this.findInFileToolStripMenuItem.Size = new System.Drawing.Size(188, 30);
            this.findInFileToolStripMenuItem.Text = "Find in open file";
            // 
            // actionsToolStripMenuItem
            // 
            this.actionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractToolStripMenuItem,
            this.analyseToolStripMenuItem,
            this.extractTemplateToolStripMenuItem});
            this.actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
            this.actionsToolStripMenuItem.Size = new System.Drawing.Size(60, 22);
            this.actionsToolStripMenuItem.Text = "Analyse";
            // 
            // extractToolStripMenuItem
            // 
            this.extractToolStripMenuItem.Image = global::WinHardenApp.Resource1.ExtractConstant_16x;
            this.extractToolStripMenuItem.Name = "extractToolStripMenuItem";
            this.extractToolStripMenuItem.Size = new System.Drawing.Size(188, 30);
            this.extractToolStripMenuItem.Text = "Extract";
            this.extractToolStripMenuItem.Click += new System.EventHandler(this.extractToolStripMenuItem_Click);
            // 
            // analyseToolStripMenuItem
            // 
            this.analyseToolStripMenuItem.Image = global::WinHardenApp.Resource1.AnalyzeTrace_16x;
            this.analyseToolStripMenuItem.Name = "analyseToolStripMenuItem";
            this.analyseToolStripMenuItem.Size = new System.Drawing.Size(188, 30);
            this.analyseToolStripMenuItem.Text = "Analyse";
            this.analyseToolStripMenuItem.Click += new System.EventHandler(this.analyseToolStripMenuItem_Click);
            // 
            // extractTemplateToolStripMenuItem
            // 
            this.extractTemplateToolStripMenuItem.Image = global::WinHardenApp.Resource1.ConfigurationEditor_16x;
            this.extractTemplateToolStripMenuItem.Name = "extractTemplateToolStripMenuItem";
            this.extractTemplateToolStripMenuItem.Size = new System.Drawing.Size(188, 30);
            this.extractTemplateToolStripMenuItem.Text = "Extract templates";
            this.extractTemplateToolStripMenuItem.Click += new System.EventHandler(this.extractTemplateToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.complianceOptionsToolStripMenuItem,
            this.activeDirectoryOptionsToolStripMenuItem,
            this.windowOptionsToolStripMenuItem});
            this.optionsToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // complianceOptionsToolStripMenuItem
            // 
            this.complianceOptionsToolStripMenuItem.Image = global::WinHardenApp.Resource1.Compliant_16x;
            this.complianceOptionsToolStripMenuItem.Name = "complianceOptionsToolStripMenuItem";
            this.complianceOptionsToolStripMenuItem.Size = new System.Drawing.Size(211, 30);
            this.complianceOptionsToolStripMenuItem.Text = "Compliance options";
            this.complianceOptionsToolStripMenuItem.Click += new System.EventHandler(this.complianceOptionsToolStripMenuItem_Click);
            // 
            // activeDirectoryOptionsToolStripMenuItem
            // 
            this.activeDirectoryOptionsToolStripMenuItem.Name = "activeDirectoryOptionsToolStripMenuItem";
            this.activeDirectoryOptionsToolStripMenuItem.Size = new System.Drawing.Size(211, 30);
            this.activeDirectoryOptionsToolStripMenuItem.Text = "Active Directory Options";
            this.activeDirectoryOptionsToolStripMenuItem.Click += new System.EventHandler(this.activeDirectoryOptionsToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(12, 22);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutWinHardenToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 22);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutWinHardenToolStripMenuItem
            // 
            this.aboutWinHardenToolStripMenuItem.Name = "aboutWinHardenToolStripMenuItem";
            this.aboutWinHardenToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.aboutWinHardenToolStripMenuItem.Text = "About WinHarden";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripButton,
            this.toolStripSeparator1,
            this.extractToolStripButton,
            this.analyseToolStripButton,
            this.isAdministratorToolStripLabel,
            this.toolStripSeparator2,
            this.connectDomainServerToolStripButton,
            this.toolStripSeparator6,
            this.findInFilesToolStripButton,
            this.toolStripSeparator3,
            this.extractionToolStripLabel,
            this.extractProgressBar});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1537, 31);
            this.toolStrip1.TabIndex = 33;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = global::WinHardenApp.Resource1.SaveStatusBar1_16x;
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(28, 28);
            this.saveToolStripButton.Text = "toolStripButton1";
            this.saveToolStripButton.ToolTipText = "Save";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // extractToolStripButton
            // 
            this.extractToolStripButton.Image = global::WinHardenApp.Resource1.ExtractConstant_16x;
            this.extractToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.extractToolStripButton.Name = "extractToolStripButton";
            this.extractToolStripButton.Size = new System.Drawing.Size(71, 28);
            this.extractToolStripButton.Text = "Extract";
            this.extractToolStripButton.Click += new System.EventHandler(this.extractToolStripButton_Click);
            // 
            // analyseToolStripButton
            // 
            this.analyseToolStripButton.Image = global::WinHardenApp.Resource1.AnalyzeTrace_16x;
            this.analyseToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.analyseToolStripButton.Name = "analyseToolStripButton";
            this.analyseToolStripButton.Size = new System.Drawing.Size(76, 28);
            this.analyseToolStripButton.Text = "Analyse";
            this.analyseToolStripButton.Click += new System.EventHandler(this.analyseToolStripButton_Click);
            // 
            // isAdministratorToolStripLabel
            // 
            this.isAdministratorToolStripLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.isAdministratorToolStripLabel.BackColor = System.Drawing.SystemColors.Control;
            this.isAdministratorToolStripLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.isAdministratorToolStripLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.isAdministratorToolStripLabel.ForeColor = System.Drawing.Color.Blue;
            this.isAdministratorToolStripLabel.Name = "isAdministratorToolStripLabel";
            this.isAdministratorToolStripLabel.Size = new System.Drawing.Size(105, 28);
            this.isAdministratorToolStripLabel.Text = "ADMINISTRATOR";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // connectDomainServerToolStripButton
            // 
            this.connectDomainServerToolStripButton.Image = global::WinHardenApp.Properties.Resources.DomainController_blue_16x;
            this.connectDomainServerToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.connectDomainServerToolStripButton.Name = "connectDomainServerToolStripButton";
            this.connectDomainServerToolStripButton.Size = new System.Drawing.Size(158, 28);
            this.connectDomainServerToolStripButton.Text = "Connect domain server";
            this.connectDomainServerToolStripButton.ToolTipText = "Connect domain server";
            this.connectDomainServerToolStripButton.Click += new System.EventHandler(this.connectDomainServerToolStripButton_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 31);
            // 
            // findInFilesToolStripButton
            // 
            this.findInFilesToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.findInFilesToolStripButton.Image = global::WinHardenApp.Resource1.SearchFolderClosed_16x;
            this.findInFilesToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.findInFilesToolStripButton.Name = "findInFilesToolStripButton";
            this.findInFilesToolStripButton.Size = new System.Drawing.Size(28, 28);
            this.findInFilesToolStripButton.Text = "Find";
            this.findInFilesToolStripButton.Click += new System.EventHandler(this.findInFilesToolStripButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 31);
            // 
            // extractionToolStripLabel
            // 
            this.extractionToolStripLabel.AutoSize = false;
            this.extractionToolStripLabel.ForeColor = System.Drawing.Color.Blue;
            this.extractionToolStripLabel.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.extractionToolStripLabel.Name = "extractionToolStripLabel";
            this.extractionToolStripLabel.Size = new System.Drawing.Size(250, 22);
            this.extractionToolStripLabel.Text = "Not started task:";
            this.extractionToolStripLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // extractProgressBar
            // 
            this.extractProgressBar.Name = "extractProgressBar";
            this.extractProgressBar.Size = new System.Drawing.Size(467, 28);
            // 
            // filesTreeView
            // 
            this.filesTreeView.Location = new System.Drawing.Point(12, 233);
            this.filesTreeView.Name = "filesTreeView";
            this.filesTreeView.Size = new System.Drawing.Size(273, 547);
            this.filesTreeView.TabIndex = 30;
            this.filesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.filesTreeView_AfterSelect);
            this.filesTreeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.filesTreeView_KeyDown);
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Size = new System.Drawing.Size(848, 0);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 798);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 36;
            this.label3.Text = "Search results";
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveFileToolStripButton,
            this.toolStripSeparator7,
            this.openGridButton,
            this.toolStripSeparator8,
            this.closeToolStripButton,
            this.toolStripSeparator4,
            this.toolStripLabel1,
            this.searchFileToolStripTextBox,
            this.searchFileToolStripButton});
            this.toolStrip2.Location = new System.Drawing.Point(1, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip2.Size = new System.Drawing.Size(416, 31);
            this.toolStrip2.TabIndex = 0;
            // 
            // saveFileToolStripButton
            // 
            this.saveFileToolStripButton.Image = global::WinHardenApp.Resource1.Save_grey_16x;
            this.saveFileToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveFileToolStripButton.Name = "saveFileToolStripButton";
            this.saveFileToolStripButton.Size = new System.Drawing.Size(108, 28);
            this.saveFileToolStripButton.Text = "Save open file";
            this.saveFileToolStripButton.Click += new System.EventHandler(this.saveFileToolStripButton_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 31);
            // 
            // openGridButton
            // 
            this.openGridButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openGridButton.Name = "openGridButton";
            this.openGridButton.Size = new System.Drawing.Size(64, 28);
            this.openGridButton.Text = "Open grid";
            this.openGridButton.Click += new System.EventHandler(this.openGridButton_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 31);
            // 
            // closeToolStripButton
            // 
            this.closeToolStripButton.Image = global::WinHardenApp.Resource1.Close_12x_16x;
            this.closeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.closeToolStripButton.Name = "closeToolStripButton";
            this.closeToolStripButton.Size = new System.Drawing.Size(83, 28);
            this.closeToolStripButton.Text = "Close file";
            this.closeToolStripButton.Click += new System.EventHandler(this.closeToolStripButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(0, 28);
            this.toolStripLabel1.Text = "Find in file:";
            // 
            // searchFileToolStripTextBox
            // 
            this.searchFileToolStripTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.searchFileToolStripTextBox.Name = "searchFileToolStripTextBox";
            this.searchFileToolStripTextBox.Size = new System.Drawing.Size(100, 31);
            this.searchFileToolStripTextBox.TextChanged += new System.EventHandler(this.searchFileToolStripTextBox_TextChanged);
            // 
            // searchFileToolStripButton
            // 
            this.searchFileToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.searchFileToolStripButton.Image = global::WinHardenApp.Resource1.SearchContract_16x;
            this.searchFileToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.searchFileToolStripButton.Name = "searchFileToolStripButton";
            this.searchFileToolStripButton.Size = new System.Drawing.Size(28, 28);
            this.searchFileToolStripButton.Text = "toolStripButton6";
            this.searchFileToolStripButton.ToolTipText = "Search in file";
            this.searchFileToolStripButton.Click += new System.EventHandler(this.searchFileToolStripButton_Click);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.toolStrip2);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1106, 32);
            this.toolStripContainer1.Location = new System.Drawing.Point(302, 176);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1106, 57);
            this.toolStripContainer1.TabIndex = 34;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // searchTreeView
            // 
            this.searchTreeView.Location = new System.Drawing.Point(16, 814);
            this.searchTreeView.Name = "searchTreeView";
            this.searchTreeView.Size = new System.Drawing.Size(1509, 150);
            this.searchTreeView.TabIndex = 40;
            this.searchTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.searchTreeView_AfterSelect);
            // 
            // filesTabControl
            // 
            this.filesTabControl.Location = new System.Drawing.Point(291, 233);
            this.filesTabControl.Name = "filesTabControl";
            this.filesTabControl.SelectedIndex = 0;
            this.filesTabControl.ShowToolTips = true;
            this.filesTabControl.Size = new System.Drawing.Size(1224, 547);
            this.filesTabControl.TabIndex = 41;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(0, 0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(200, 100);
            this.tabPage1.TabIndex = 0;
            // 
            // formatCSVFilesToolStripMenuItem
            // 
            this.formatCSVFilesToolStripMenuItem.Name = "formatCSVFilesToolStripMenuItem";
            this.formatCSVFilesToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // searchTreeTextBox
            // 
            this.searchTreeTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.searchTreeTextBox.Location = new System.Drawing.Point(15, 201);
            this.searchTreeTextBox.Name = "searchTreeTextBox";
            this.searchTreeTextBox.Size = new System.Drawing.Size(243, 20);
            this.searchTreeTextBox.TabIndex = 42;
            // 
            // searchTreeViewButton
            // 
            this.searchTreeViewButton.Image = global::WinHardenApp.Resource1.SearchContract_16x;
            this.searchTreeViewButton.Location = new System.Drawing.Point(260, 201);
            this.searchTreeViewButton.Name = "searchTreeViewButton";
            this.searchTreeViewButton.Size = new System.Drawing.Size(25, 23);
            this.searchTreeViewButton.TabIndex = 43;
            this.searchTreeViewButton.UseVisualStyleBackColor = true;
            this.searchTreeViewButton.Click += new System.EventHandler(this.searchTreeViewButton_Click);
            // 
            // windowOptionsToolStripMenuItem
            // 
            this.windowOptionsToolStripMenuItem.Name = "windowOptionsToolStripMenuItem";
            this.windowOptionsToolStripMenuItem.Size = new System.Drawing.Size(211, 30);
            this.windowOptionsToolStripMenuItem.Text = "Window options";
            this.windowOptionsToolStripMenuItem.Click += new System.EventHandler(this.windowOptionsToolStripMenuItem_Click);
            // 
            // WinHardenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1537, 971);
            this.Controls.Add(this.saveAnalysisButton);
            this.Controls.Add(this.analysisOutputFolderTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.searchTreeViewButton);
            this.Controls.Add(this.searchTreeTextBox);
            this.Controls.Add(this.filesTabControl);
            this.Controls.Add(this.searchTreeView);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.filesTreeView);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "WinHardenForm";
            this.RightToLeftLayout = true;
            this.Text = "WinHarden";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }




        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox extractionOutputFolderTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button saveExtractionButton;
        private System.Windows.Forms.Button saveAnalysisButton;
        private System.Windows.Forms.TextBox analysisOutputFolderTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem actionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem analyseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem complianceOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ToolStripButton extractToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton analyseToolStripButton;
        private System.Windows.Forms.ToolStripLabel isAdministratorToolStripLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton findInFilesToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel extractionToolStripLabel;
        private System.Windows.Forms.ToolStripProgressBar extractProgressBar;
        private System.Windows.Forms.ToolStripMenuItem savesAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutWinHardenToolStripMenuItem;
        private System.Windows.Forms.TreeView filesTreeView;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton saveFileToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox searchFileToolStripTextBox;
        private System.Windows.Forms.ToolStripButton searchFileToolStripButton;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripMenuItem findInFileToolStripMenuItem;
        private System.Windows.Forms.TreeView searchTreeView;
        private System.Windows.Forms.TabControl filesTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ToolStripButton closeToolStripButton;
        private ToolStripMenuItem activeDirectoryOptionsToolStripMenuItem;
        private ToolStripButton connectDomainServerToolStripButton;
        private ToolStripSeparator toolStripSeparator6;
        private TextBox inputFileTextBox;
        private Button inputPermissionAuditButton;
        private Label label2;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem formatCSVFilesToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripButton openGridButton;
        private ToolStripSeparator toolStripSeparator8;
        private TextBox searchTreeTextBox;
        private Button searchTreeViewButton;
        private ToolStripMenuItem windowOptionsToolStripMenuItem;
    }
}

