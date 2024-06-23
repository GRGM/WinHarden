
namespace WinHardenApp.Forms
{
    partial class ExtractRecommendationsForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.serverRadioButton = new System.Windows.Forms.RadioButton();
            this.saveAuditPolicyButton = new System.Windows.Forms.Button();
            this.workstationRadioButton = new System.Windows.Forms.RadioButton();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.saveRegistryPolicyFileButton = new System.Windows.Forms.Button();
            this.registryPolicyFileTextBox = new System.Windows.Forms.TextBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.securityPolicyFileTextBox = new System.Windows.Forms.TextBox();
            this.saveSecurityPolicyFileButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(961, 162);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(953, 136);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Audit policy";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.serverRadioButton);
            this.groupBox2.Controls.Add(this.saveAuditPolicyButton);
            this.groupBox2.Controls.Add(this.workstationRadioButton);
            this.groupBox2.Controls.Add(this.pathTextBox);
            this.groupBox2.Location = new System.Drawing.Point(3, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(905, 110);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Extract Windows policy recommendations";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Patch to extract";
            // 
            // serverRadioButton
            // 
            this.serverRadioButton.AutoSize = true;
            this.serverRadioButton.Location = new System.Drawing.Point(6, 53);
            this.serverRadioButton.Name = "serverRadioButton";
            this.serverRadioButton.Size = new System.Drawing.Size(172, 17);
            this.serverRadioButton.TabIndex = 13;
            this.serverRadioButton.Text = "Server policy recommendations";
            this.serverRadioButton.UseVisualStyleBackColor = true;
            // 
            // saveAuditPolicyButton
            // 
            this.saveAuditPolicyButton.Location = new System.Drawing.Point(818, 74);
            this.saveAuditPolicyButton.Name = "saveAuditPolicyButton";
            this.saveAuditPolicyButton.Size = new System.Drawing.Size(75, 23);
            this.saveAuditPolicyButton.TabIndex = 24;
            this.saveAuditPolicyButton.Text = "Save";
            this.saveAuditPolicyButton.UseVisualStyleBackColor = true;
            this.saveAuditPolicyButton.Click += new System.EventHandler(this.saveAuditPolicyButton_Click);
            // 
            // workstationRadioButton
            // 
            this.workstationRadioButton.AutoSize = true;
            this.workstationRadioButton.Checked = true;
            this.workstationRadioButton.Location = new System.Drawing.Point(6, 27);
            this.workstationRadioButton.Name = "workstationRadioButton";
            this.workstationRadioButton.Size = new System.Drawing.Size(198, 17);
            this.workstationRadioButton.TabIndex = 12;
            this.workstationRadioButton.TabStop = true;
            this.workstationRadioButton.Text = "Workstation policy recommendations";
            this.workstationRadioButton.UseVisualStyleBackColor = true;
            // 
            // pathTextBox
            // 
            this.pathTextBox.Location = new System.Drawing.Point(229, 77);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(583, 20);
            this.pathTextBox.TabIndex = 20;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(953, 136);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Security policy";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.saveSecurityPolicyFileButton);
            this.groupBox1.Controls.Add(this.securityPolicyFileTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.saveRegistryPolicyFileButton);
            this.groupBox1.Controls.Add(this.registryPolicyFileTextBox);
            this.groupBox1.Location = new System.Drawing.Point(25, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(905, 110);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Extract Security policy recommendations";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Default registry policy template";
            // 
            // saveRegistryPolicyFileButton
            // 
            this.saveRegistryPolicyFileButton.Location = new System.Drawing.Point(818, 21);
            this.saveRegistryPolicyFileButton.Name = "saveRegistryPolicyFileButton";
            this.saveRegistryPolicyFileButton.Size = new System.Drawing.Size(75, 23);
            this.saveRegistryPolicyFileButton.TabIndex = 24;
            this.saveRegistryPolicyFileButton.Text = "Save";
            this.saveRegistryPolicyFileButton.UseVisualStyleBackColor = true;
            this.saveRegistryPolicyFileButton.Click += new System.EventHandler(this.saveRegistryPolicyFileButton_Click);
            // 
            // registryPolicyFileTextBox
            // 
            this.registryPolicyFileTextBox.Location = new System.Drawing.Point(213, 24);
            this.registryPolicyFileTextBox.Name = "registryPolicyFileTextBox";
            this.registryPolicyFileTextBox.Size = new System.Drawing.Size(583, 20);
            this.registryPolicyFileTextBox.TabIndex = 20;
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(837, 176);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 25;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(153, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Default security policy template";
            // 
            // securityPolicyFileTextBox
            // 
            this.securityPolicyFileTextBox.Location = new System.Drawing.Point(213, 64);
            this.securityPolicyFileTextBox.Name = "securityPolicyFileTextBox";
            this.securityPolicyFileTextBox.Size = new System.Drawing.Size(583, 20);
            this.securityPolicyFileTextBox.TabIndex = 27;
            // 
            // saveSecurityPolicyFileButton
            // 
            this.saveSecurityPolicyFileButton.Location = new System.Drawing.Point(818, 67);
            this.saveSecurityPolicyFileButton.Name = "saveSecurityPolicyFileButton";
            this.saveSecurityPolicyFileButton.Size = new System.Drawing.Size(75, 23);
            this.saveSecurityPolicyFileButton.TabIndex = 28;
            this.saveSecurityPolicyFileButton.Text = "Save";
            this.saveSecurityPolicyFileButton.UseVisualStyleBackColor = true;
            this.saveSecurityPolicyFileButton.Click += new System.EventHandler(this.saveSecurityPolicyFileButton_Click);
            // 
            // ExtractRecommendationsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 207);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.tabControl1);
            this.Name = "ExtractRecommendationsForm";
            this.ShowIcon = false;
            this.Text = "Extract recommendations";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton serverRadioButton;
        private System.Windows.Forms.Button saveAuditPolicyButton;
        private System.Windows.Forms.RadioButton workstationRadioButton;
        private System.Windows.Forms.TextBox pathTextBox;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button saveRegistryPolicyFileButton;
        private System.Windows.Forms.TextBox registryPolicyFileTextBox;
        private System.Windows.Forms.Button saveSecurityPolicyFileButton;
        private System.Windows.Forms.TextBox securityPolicyFileTextBox;
        private System.Windows.Forms.Label label3;
    }
}