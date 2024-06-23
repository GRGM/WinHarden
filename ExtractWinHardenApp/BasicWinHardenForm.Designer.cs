namespace ExtractWinHardenApp
{
    partial class BasicWinHardenForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BasicWinHardenForm));
            this.extractionOutputFolderTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.saveExtractionButton = new System.Windows.Forms.Button();
            this.extractButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.extractionToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.isAdministratorToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.extractProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.inputPermissionAuditButton = new System.Windows.Forms.Button();
            this.inputFileTextBox = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // extractionOutputFolderTextBox
            // 
            this.extractionOutputFolderTextBox.Location = new System.Drawing.Point(355, 59);
            this.extractionOutputFolderTextBox.Name = "extractionOutputFolderTextBox";
            this.extractionOutputFolderTextBox.Size = new System.Drawing.Size(527, 20);
            this.extractionOutputFolderTextBox.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Select extraction output folder";
            // 
            // saveExtractionButton
            // 
            this.saveExtractionButton.Location = new System.Drawing.Point(909, 57);
            this.saveExtractionButton.Name = "saveExtractionButton";
            this.saveExtractionButton.Size = new System.Drawing.Size(75, 23);
            this.saveExtractionButton.TabIndex = 3;
            this.saveExtractionButton.Text = "Open";
            this.saveExtractionButton.UseVisualStyleBackColor = true;
            this.saveExtractionButton.Click += new System.EventHandler(this.saveExtractionButton_Click);
            // 
            // extractButton
            // 
            this.extractButton.Location = new System.Drawing.Point(807, 123);
            this.extractButton.Name = "extractButton";
            this.extractButton.Size = new System.Drawing.Size(75, 23);
            this.extractButton.TabIndex = 6;
            this.extractButton.Text = "Extract";
            this.extractButton.UseVisualStyleBackColor = true;
            this.extractButton.Click += new System.EventHandler(this.extractButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(909, 123);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
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
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.isAdministratorToolStripLabel,
            this.extractionToolStripLabel,
            this.extractProgressBar});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1015, 38);
            this.toolStrip1.TabIndex = 34;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // isAdministratorToolStripLabel
            // 
            this.isAdministratorToolStripLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.isAdministratorToolStripLabel.BackColor = System.Drawing.SystemColors.Control;
            this.isAdministratorToolStripLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.isAdministratorToolStripLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.isAdministratorToolStripLabel.ForeColor = System.Drawing.Color.Blue;
            this.isAdministratorToolStripLabel.Name = "isAdministratorToolStripLabel";
            this.isAdministratorToolStripLabel.Size = new System.Drawing.Size(105, 35);
            this.isAdministratorToolStripLabel.Text = "ADMINISTRATOR";
            // 
            // extractProgressBar
            // 
            this.extractProgressBar.Name = "extractProgressBar";
            this.extractProgressBar.Size = new System.Drawing.Size(407, 35);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(337, 13);
            this.label2.TabIndex = 35;
            this.label2.Text = "Select input file to obtain permissions and audit configuration (optional)";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // inputPermissionAuditButton
            // 
            this.inputPermissionAuditButton.Location = new System.Drawing.Point(909, 86);
            this.inputPermissionAuditButton.Name = "inputPermissionAuditButton";
            this.inputPermissionAuditButton.Size = new System.Drawing.Size(75, 23);
            this.inputPermissionAuditButton.TabIndex = 36;
            this.inputPermissionAuditButton.Text = "Open";
            this.inputPermissionAuditButton.UseVisualStyleBackColor = true;
            this.inputPermissionAuditButton.Click += new System.EventHandler(this.inputPermissionAuditButton_Click);
            // 
            // inputFileTextBox
            // 
            this.inputFileTextBox.Location = new System.Drawing.Point(355, 89);
            this.inputFileTextBox.Name = "inputFileTextBox";
            this.inputFileTextBox.Size = new System.Drawing.Size(527, 20);
            this.inputFileTextBox.TabIndex = 37;
            // 
            // BasicWinHardenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 176);
            this.Controls.Add(this.inputFileTextBox);
            this.Controls.Add(this.inputPermissionAuditButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.extractButton);
            this.Controls.Add(this.extractionOutputFolderTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.saveExtractionButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BasicWinHardenForm";
            this.Text = "Basic WinHarden";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox extractionOutputFolderTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button saveExtractionButton;
        private System.Windows.Forms.Button extractButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ToolStripLabel extractionToolStripLabel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel isAdministratorToolStripLabel;
        private System.Windows.Forms.ToolStripProgressBar extractProgressBar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button inputPermissionAuditButton;
        private System.Windows.Forms.TextBox inputFileTextBox;
    }
}