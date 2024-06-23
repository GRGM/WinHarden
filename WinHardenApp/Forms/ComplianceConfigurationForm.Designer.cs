
namespace WinHardenApp.Forms
{
    partial class ComplianceConfigurationForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.userDefinedTextBox = new System.Windows.Forms.TextBox();
            this.openUserDefinedPolicyButton = new System.Windows.Forms.Button();
            this.userDefinedRadioButton = new System.Windows.Forms.RadioButton();
            this.serverRadioButton = new System.Windows.Forms.RadioButton();
            this.workstationRadioButton = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lastLogonDaysTextBox = new System.Windows.Forms.TextBox();
            this.lastPasswordDaysTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.registryKeyPolicyTextBox = new System.Windows.Forms.TextBox();
            this.openRegistryKeyPolicyButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.translatedSIDCheckBox = new System.Windows.Forms.CheckBox();
            this.securityPolicyTextBox = new System.Windows.Forms.TextBox();
            this.openSecurityPolicyButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(9, 11);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(961, 212);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(953, 161);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Audit policy";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.userDefinedTextBox);
            this.groupBox1.Controls.Add(this.openUserDefinedPolicyButton);
            this.groupBox1.Controls.Add(this.userDefinedRadioButton);
            this.groupBox1.Controls.Add(this.serverRadioButton);
            this.groupBox1.Controls.Add(this.workstationRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(17, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(905, 114);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configuration to check compliance in audit policy";
            // 
            // userDefinedTextBox
            // 
            this.userDefinedTextBox.Location = new System.Drawing.Point(212, 81);
            this.userDefinedTextBox.Name = "userDefinedTextBox";
            this.userDefinedTextBox.Size = new System.Drawing.Size(588, 20);
            this.userDefinedTextBox.TabIndex = 11;
            // 
            // openUserDefinedPolicyButton
            // 
            this.openUserDefinedPolicyButton.Location = new System.Drawing.Point(818, 82);
            this.openUserDefinedPolicyButton.Name = "openUserDefinedPolicyButton";
            this.openUserDefinedPolicyButton.Size = new System.Drawing.Size(75, 23);
            this.openUserDefinedPolicyButton.TabIndex = 10;
            this.openUserDefinedPolicyButton.Text = "Open";
            this.openUserDefinedPolicyButton.UseVisualStyleBackColor = true;
            this.openUserDefinedPolicyButton.Click += new System.EventHandler(this.openUserDefinedPolicyButton_Click);
            // 
            // userDefinedRadioButton
            // 
            this.userDefinedRadioButton.AutoSize = true;
            this.userDefinedRadioButton.Location = new System.Drawing.Point(16, 82);
            this.userDefinedRadioButton.Name = "userDefinedRadioButton";
            this.userDefinedRadioButton.Size = new System.Drawing.Size(115, 17);
            this.userDefinedRadioButton.TabIndex = 9;
            this.userDefinedRadioButton.Text = "User-defined policy";
            this.userDefinedRadioButton.UseVisualStyleBackColor = true;
            // 
            // serverRadioButton
            // 
            this.serverRadioButton.AutoSize = true;
            this.serverRadioButton.Location = new System.Drawing.Point(16, 55);
            this.serverRadioButton.Name = "serverRadioButton";
            this.serverRadioButton.Size = new System.Drawing.Size(172, 17);
            this.serverRadioButton.TabIndex = 8;
            this.serverRadioButton.Text = "Server policy recommendations";
            this.serverRadioButton.UseVisualStyleBackColor = true;
            // 
            // workstationRadioButton
            // 
            this.workstationRadioButton.AutoSize = true;
            this.workstationRadioButton.Checked = true;
            this.workstationRadioButton.Location = new System.Drawing.Point(16, 29);
            this.workstationRadioButton.Name = "workstationRadioButton";
            this.workstationRadioButton.Size = new System.Drawing.Size(198, 17);
            this.workstationRadioButton.TabIndex = 7;
            this.workstationRadioButton.TabStop = true;
            this.workstationRadioButton.Text = "Workstation policy recommendations";
            this.workstationRadioButton.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(953, 161);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Password policy";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lastLogonDaysTextBox);
            this.groupBox2.Controls.Add(this.lastPasswordDaysTextBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(13, 12);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(927, 105);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Configuration to check compliance in password policy";
            // 
            // lastLogonDaysTextBox
            // 
            this.lastLogonDaysTextBox.Location = new System.Drawing.Point(328, 68);
            this.lastLogonDaysTextBox.Name = "lastLogonDaysTextBox";
            this.lastLogonDaysTextBox.Size = new System.Drawing.Size(562, 20);
            this.lastLogonDaysTextBox.TabIndex = 38;
            // 
            // lastPasswordDaysTextBox
            // 
            this.lastPasswordDaysTextBox.Location = new System.Drawing.Point(328, 34);
            this.lastPasswordDaysTextBox.Name = "lastPasswordDaysTextBox";
            this.lastPasswordDaysTextBox.Size = new System.Drawing.Size(562, 20);
            this.lastPasswordDaysTextBox.TabIndex = 37;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 66);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(265, 13);
            this.label2.TabIndex = 36;
            this.label2.Text = "List of days to check last logon (separated by commas)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(304, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "List of days to check last password set (separated by commas) ";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(953, 186);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Security policy";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.registryKeyPolicyTextBox);
            this.groupBox3.Controls.Add(this.openRegistryKeyPolicyButton);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.translatedSIDCheckBox);
            this.groupBox3.Controls.Add(this.securityPolicyTextBox);
            this.groupBox3.Controls.Add(this.openSecurityPolicyButton);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(11, 14);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Size = new System.Drawing.Size(927, 158);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Configuration to check compliance in security policy";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(14, 58);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(705, 13);
            this.label5.TabIndex = 42;
            this.label5.Text = "Analysing process requires that extraction was performed considering the defined " +
    "registry keys policy template. Blank file means default registry policy";
            // 
            // registryKeyPolicyTextBox
            // 
            this.registryKeyPolicyTextBox.Location = new System.Drawing.Point(249, 35);
            this.registryKeyPolicyTextBox.Name = "registryKeyPolicyTextBox";
            this.registryKeyPolicyTextBox.Size = new System.Drawing.Size(587, 20);
            this.registryKeyPolicyTextBox.TabIndex = 41;
            // 
            // openRegistryKeyPolicyButton
            // 
            this.openRegistryKeyPolicyButton.Location = new System.Drawing.Point(841, 31);
            this.openRegistryKeyPolicyButton.Name = "openRegistryKeyPolicyButton";
            this.openRegistryKeyPolicyButton.Size = new System.Drawing.Size(75, 23);
            this.openRegistryKeyPolicyButton.TabIndex = 40;
            this.openRegistryKeyPolicyButton.Text = "Open";
            this.openRegistryKeyPolicyButton.UseVisualStyleBackColor = true;
            this.openRegistryKeyPolicyButton.Click += new System.EventHandler(this.openRegistryKeyPolicyButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 37);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(239, 13);
            this.label3.TabIndex = 39;
            this.label3.Text = "Registry keys policy template (key = value format)";
            // 
            // translatedSIDCheckBox
            // 
            this.translatedSIDCheckBox.AutoSize = true;
            this.translatedSIDCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.translatedSIDCheckBox.Location = new System.Drawing.Point(16, 126);
            this.translatedSIDCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.translatedSIDCheckBox.Name = "translatedSIDCheckBox";
            this.translatedSIDCheckBox.Size = new System.Drawing.Size(186, 17);
            this.translatedSIDCheckBox.TabIndex = 38;
            this.translatedSIDCheckBox.Text = "Security policy with translated SID";
            this.translatedSIDCheckBox.UseVisualStyleBackColor = true;
            // 
            // securityPolicyTextBox
            // 
            this.securityPolicyTextBox.Location = new System.Drawing.Point(249, 84);
            this.securityPolicyTextBox.Name = "securityPolicyTextBox";
            this.securityPolicyTextBox.Size = new System.Drawing.Size(587, 20);
            this.securityPolicyTextBox.TabIndex = 37;
            // 
            // openSecurityPolicyButton
            // 
            this.openSecurityPolicyButton.Location = new System.Drawing.Point(840, 81);
            this.openSecurityPolicyButton.Name = "openSecurityPolicyButton";
            this.openSecurityPolicyButton.Size = new System.Drawing.Size(75, 23);
            this.openSecurityPolicyButton.TabIndex = 36;
            this.openSecurityPolicyButton.Text = "Open";
            this.openSecurityPolicyButton.UseVisualStyleBackColor = true;
            this.openSecurityPolicyButton.Click += new System.EventHandler(this.openSecurityPolicyButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 83);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(225, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "Security policy template (secedit export format)";
            // 
            // acceptButton
            // 
            this.acceptButton.Location = new System.Drawing.Point(759, 229);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 23);
            this.acceptButton.TabIndex = 33;
            this.acceptButton.Text = "Accept";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(852, 229);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 34;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(14, 107);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(188, 13);
            this.label6.TabIndex = 43;
            this.label6.Text = "Blank file means default security policy";
            // 
            // ComplianceConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 264);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.tabControl1);
            this.Name = "ComplianceConfigurationForm";
            this.ShowIcon = false;
            this.Text = "Compliance configuration";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox userDefinedTextBox;
        private System.Windows.Forms.Button openUserDefinedPolicyButton;
        private System.Windows.Forms.RadioButton userDefinedRadioButton;
        private System.Windows.Forms.RadioButton serverRadioButton;
        private System.Windows.Forms.RadioButton workstationRadioButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox lastLogonDaysTextBox;
        private System.Windows.Forms.TextBox lastPasswordDaysTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox translatedSIDCheckBox;
        private System.Windows.Forms.TextBox securityPolicyTextBox;
        private System.Windows.Forms.Button openSecurityPolicyButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox registryKeyPolicyTextBox;
        private System.Windows.Forms.Button openRegistryKeyPolicyButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
    }
}