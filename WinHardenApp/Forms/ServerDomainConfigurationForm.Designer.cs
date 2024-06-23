namespace WinHardenApp.Forms
{
    partial class ServerDomainConfigurationForm
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
            this.ldapTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.workstationListBox = new System.Windows.Forms.ListBox();
            this.extractedWorkstationRadioButton = new System.Windows.Forms.RadioButton();
            this.currentWorksationRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.serverLabel = new System.Windows.Forms.Label();
            this.defaultServerRadioButton = new System.Windows.Forms.RadioButton();
            this.userDomainRadioButton = new System.Windows.Forms.RadioButton();
            this.DNSDomainRadioButton = new System.Windows.Forms.RadioButton();
            this.userDomainLabel = new System.Windows.Forms.Label();
            this.DNSDomainLabel = new System.Windows.Forms.Label();
            this.connectDomainServerButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.obtainGroupsCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ldapTextBox
            // 
            this.ldapTextBox.Location = new System.Drawing.Point(280, 392);
            this.ldapTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ldapTextBox.Name = "ldapTextBox";
            this.ldapTextBox.Size = new System.Drawing.Size(732, 26);
            this.ldapTextBox.TabIndex = 58;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 397);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 20);
            this.label7.TabIndex = 57;
            this.label7.Text = "Ldap Server";
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(900, 504);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(112, 35);
            this.cancelButton.TabIndex = 67;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Location = new System.Drawing.Point(770, 504);
            this.acceptButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(112, 35);
            this.acceptButton.TabIndex = 66;
            this.acceptButton.Text = "Accept";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.workstationListBox);
            this.groupBox1.Controls.Add(this.extractedWorkstationRadioButton);
            this.groupBox1.Controls.Add(this.currentWorksationRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(2, 84);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1031, 116);
            this.groupBox1.TabIndex = 70;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Reference workstation";
            // 
            // workstationListBox
            // 
            this.workstationListBox.FormattingEnabled = true;
            this.workstationListBox.ItemHeight = 20;
            this.workstationListBox.Location = new System.Drawing.Point(277, 75);
            this.workstationListBox.Name = "workstationListBox";
            this.workstationListBox.Size = new System.Drawing.Size(360, 24);
            this.workstationListBox.TabIndex = 79;
            this.workstationListBox.SelectedIndexChanged += new System.EventHandler(this.workstationListBox_SelectedIndexChanged);
            // 
            // extractedWorkstationRadioButton
            // 
            this.extractedWorkstationRadioButton.AutoSize = true;
            this.extractedWorkstationRadioButton.Location = new System.Drawing.Point(14, 74);
            this.extractedWorkstationRadioButton.Name = "extractedWorkstationRadioButton";
            this.extractedWorkstationRadioButton.Size = new System.Drawing.Size(187, 24);
            this.extractedWorkstationRadioButton.TabIndex = 78;
            this.extractedWorkstationRadioButton.Text = "Extracted workstation";
            this.extractedWorkstationRadioButton.UseVisualStyleBackColor = true;
            this.extractedWorkstationRadioButton.CheckedChanged += new System.EventHandler(this.extractedWorkstationRadioButton_CheckedChanged);
            // 
            // currentWorksationRadioButton
            // 
            this.currentWorksationRadioButton.AutoSize = true;
            this.currentWorksationRadioButton.Checked = true;
            this.currentWorksationRadioButton.Location = new System.Drawing.Point(14, 35);
            this.currentWorksationRadioButton.Name = "currentWorksationRadioButton";
            this.currentWorksationRadioButton.Size = new System.Drawing.Size(172, 24);
            this.currentWorksationRadioButton.TabIndex = 77;
            this.currentWorksationRadioButton.TabStop = true;
            this.currentWorksationRadioButton.Text = "Current workstation";
            this.currentWorksationRadioButton.UseVisualStyleBackColor = true;
            this.currentWorksationRadioButton.CheckedChanged += new System.EventHandler(this.currentWorksationRadioButton_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.serverLabel);
            this.groupBox2.Controls.Add(this.defaultServerRadioButton);
            this.groupBox2.Controls.Add(this.userDomainRadioButton);
            this.groupBox2.Controls.Add(this.DNSDomainRadioButton);
            this.groupBox2.Controls.Add(this.userDomainLabel);
            this.groupBox2.Controls.Add(this.DNSDomainLabel);
            this.groupBox2.Location = new System.Drawing.Point(2, 214);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1031, 170);
            this.groupBox2.TabIndex = 71;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Default LDAP server choices";
            // 
            // serverLabel
            // 
            this.serverLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.serverLabel.Location = new System.Drawing.Point(282, 120);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(355, 31);
            this.serverLabel.TabIndex = 76;
            // 
            // defaultServerRadioButton
            // 
            this.defaultServerRadioButton.AutoSize = true;
            this.defaultServerRadioButton.Location = new System.Drawing.Point(18, 122);
            this.defaultServerRadioButton.Name = "defaultServerRadioButton";
            this.defaultServerRadioButton.Size = new System.Drawing.Size(133, 24);
            this.defaultServerRadioButton.TabIndex = 75;
            this.defaultServerRadioButton.Text = "Default server";
            this.defaultServerRadioButton.UseVisualStyleBackColor = true;
            this.defaultServerRadioButton.CheckedChanged += new System.EventHandler(this.defaultServerRadioButton_CheckedChanged);
            // 
            // userDomainRadioButton
            // 
            this.userDomainRadioButton.AutoSize = true;
            this.userDomainRadioButton.Location = new System.Drawing.Point(18, 77);
            this.userDomainRadioButton.Name = "userDomainRadioButton";
            this.userDomainRadioButton.Size = new System.Drawing.Size(127, 24);
            this.userDomainRadioButton.TabIndex = 74;
            this.userDomainRadioButton.Text = "User Domain";
            this.userDomainRadioButton.UseVisualStyleBackColor = true;
            this.userDomainRadioButton.CheckedChanged += new System.EventHandler(this.userDomainRadioButton_CheckedChanged);
            // 
            // DNSDomainRadioButton
            // 
            this.DNSDomainRadioButton.AutoSize = true;
            this.DNSDomainRadioButton.Checked = true;
            this.DNSDomainRadioButton.Location = new System.Drawing.Point(14, 38);
            this.DNSDomainRadioButton.Name = "DNSDomainRadioButton";
            this.DNSDomainRadioButton.Size = new System.Drawing.Size(165, 24);
            this.DNSDomainRadioButton.TabIndex = 73;
            this.DNSDomainRadioButton.TabStop = true;
            this.DNSDomainRadioButton.Text = "User DNS Domain";
            this.DNSDomainRadioButton.UseVisualStyleBackColor = true;
            this.DNSDomainRadioButton.CheckedChanged += new System.EventHandler(this.DNSDomainRadioButton_CheckedChanged);
            // 
            // userDomainLabel
            // 
            this.userDomainLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.userDomainLabel.Location = new System.Drawing.Point(282, 78);
            this.userDomainLabel.Name = "userDomainLabel";
            this.userDomainLabel.Size = new System.Drawing.Size(355, 28);
            this.userDomainLabel.TabIndex = 72;
            // 
            // DNSDomainLabel
            // 
            this.DNSDomainLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.DNSDomainLabel.Location = new System.Drawing.Point(282, 36);
            this.DNSDomainLabel.Name = "DNSDomainLabel";
            this.DNSDomainLabel.Size = new System.Drawing.Size(355, 25);
            this.DNSDomainLabel.TabIndex = 71;
            // 
            // connectDomainServerButton
            // 
            this.connectDomainServerButton.Location = new System.Drawing.Point(900, 441);
            this.connectDomainServerButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.connectDomainServerButton.Name = "connectDomainServerButton";
            this.connectDomainServerButton.Size = new System.Drawing.Size(112, 35);
            this.connectDomainServerButton.TabIndex = 72;
            this.connectDomainServerButton.Text = "Connect";
            this.connectDomainServerButton.UseVisualStyleBackColor = true;
            this.connectDomainServerButton.Click += new System.EventHandler(this.connectDomainServerButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 456);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 20);
            this.label1.TabIndex = 73;
            this.label1.Text = "Connect domain server";
            // 
            // obtainGroupsCheckBox
            // 
            this.obtainGroupsCheckBox.AutoSize = true;
            this.obtainGroupsCheckBox.Location = new System.Drawing.Point(17, 30);
            this.obtainGroupsCheckBox.Name = "obtainGroupsCheckBox";
            this.obtainGroupsCheckBox.Size = new System.Drawing.Size(343, 24);
            this.obtainGroupsCheckBox.TabIndex = 74;
            this.obtainGroupsCheckBox.Text = "Get list of groups from users during analysis";
            this.obtainGroupsCheckBox.UseVisualStyleBackColor = true;
            // 
            // ServerDomainConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 575);
            this.Controls.Add(this.obtainGroupsCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.connectDomainServerButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.ldapTextBox);
            this.Controls.Add(this.label7);
            this.Name = "ServerDomainConfigurationForm";
            this.Text = "Domain server configuration";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ldapTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton extractedWorkstationRadioButton;
        private System.Windows.Forms.RadioButton currentWorksationRadioButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.RadioButton defaultServerRadioButton;
        private System.Windows.Forms.RadioButton userDomainRadioButton;
        private System.Windows.Forms.RadioButton DNSDomainRadioButton;
        private System.Windows.Forms.Label userDomainLabel;
        private System.Windows.Forms.Label DNSDomainLabel;
        private System.Windows.Forms.ListBox workstationListBox;
        private System.Windows.Forms.Button connectDomainServerButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox obtainGroupsCheckBox;
    }
}