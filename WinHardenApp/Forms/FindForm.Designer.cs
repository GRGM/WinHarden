
namespace WinHardenApp.Forms
{
    partial class FindForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.allWorkingFoldersFilesRadioButton = new System.Windows.Forms.RadioButton();
            this.allOpenFileRadioButton = new System.Windows.Forms.RadioButton();
            this.openFileRadioButton = new System.Windows.Forms.RadioButton();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.allWorkingFoldersFilesRadioButton);
            this.groupBox1.Controls.Add(this.allOpenFileRadioButton);
            this.groupBox1.Controls.Add(this.openFileRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(18, 89);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(500, 169);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Modes";
            // 
            // allWorkingFoldersFilesRadioButton
            // 
            this.allWorkingFoldersFilesRadioButton.AutoSize = true;
            this.allWorkingFoldersFilesRadioButton.Location = new System.Drawing.Point(24, 126);
            this.allWorkingFoldersFilesRadioButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.allWorkingFoldersFilesRadioButton.Name = "allWorkingFoldersFilesRadioButton";
            this.allWorkingFoldersFilesRadioButton.Size = new System.Drawing.Size(193, 24);
            this.allWorkingFoldersFilesRadioButton.TabIndex = 9;
            this.allWorkingFoldersFilesRadioButton.Text = "All working folders files";
            this.allWorkingFoldersFilesRadioButton.UseVisualStyleBackColor = true;
            // 
            // allOpenFileRadioButton
            // 
            this.allOpenFileRadioButton.AutoSize = true;
            this.allOpenFileRadioButton.Location = new System.Drawing.Point(24, 85);
            this.allOpenFileRadioButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.allOpenFileRadioButton.Name = "allOpenFileRadioButton";
            this.allOpenFileRadioButton.Size = new System.Drawing.Size(123, 24);
            this.allOpenFileRadioButton.TabIndex = 8;
            this.allOpenFileRadioButton.Text = "All open files";
            this.allOpenFileRadioButton.UseVisualStyleBackColor = true;
            // 
            // openFileRadioButton
            // 
            this.openFileRadioButton.AutoSize = true;
            this.openFileRadioButton.Checked = true;
            this.openFileRadioButton.Location = new System.Drawing.Point(24, 45);
            this.openFileRadioButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.openFileRadioButton.Name = "openFileRadioButton";
            this.openFileRadioButton.Size = new System.Drawing.Size(111, 24);
            this.openFileRadioButton.TabIndex = 7;
            this.openFileRadioButton.TabStop = true;
            this.openFileRadioButton.Text = "Current file";
            this.openFileRadioButton.UseVisualStyleBackColor = true;
            // 
            // searchTextBox
            // 
            this.searchTextBox.Location = new System.Drawing.Point(172, 34);
            this.searchTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(343, 26);
            this.searchTextBox.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 20);
            this.label1.TabIndex = 18;
            this.label1.Text = "String to search:";
            // 
            // acceptButton
            // 
            this.acceptButton.Location = new System.Drawing.Point(284, 289);
            this.acceptButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(112, 35);
            this.acceptButton.TabIndex = 19;
            this.acceptButton.Text = "Find";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(405, 289);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(112, 35);
            this.cancelButton.TabIndex = 20;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // FindForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 348);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FindForm";
            this.ShowIcon = false;
            this.Text = "Find";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton allWorkingFoldersFilesRadioButton;
        private System.Windows.Forms.RadioButton allOpenFileRadioButton;
        private System.Windows.Forms.RadioButton openFileRadioButton;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
    }
}