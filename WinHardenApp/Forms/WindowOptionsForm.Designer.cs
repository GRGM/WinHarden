namespace WinHardenApp.Forms
{
    partial class WindowOptionsForm
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.hideEmptyFilesCheckBox = new System.Windows.Forms.CheckBox();
            this.colorFilesCheckBox = new System.Windows.Forms.CheckBox();
            this.colorEveryoneCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(486, 170);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(112, 35);
            this.cancelButton.TabIndex = 36;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Location = new System.Drawing.Point(351, 171);
            this.acceptButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(112, 35);
            this.acceptButton.TabIndex = 35;
            this.acceptButton.Text = "Accept";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // hideEmptyFilesCheckBox
            // 
            this.hideEmptyFilesCheckBox.AutoSize = true;
            this.hideEmptyFilesCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.hideEmptyFilesCheckBox.Location = new System.Drawing.Point(12, 30);
            this.hideEmptyFilesCheckBox.Name = "hideEmptyFilesCheckBox";
            this.hideEmptyFilesCheckBox.Size = new System.Drawing.Size(229, 24);
            this.hideEmptyFilesCheckBox.TabIndex = 37;
            this.hideEmptyFilesCheckBox.Text = "Hide empty files in tree view";
            this.hideEmptyFilesCheckBox.UseVisualStyleBackColor = true;
            // 
            // colorFilesCheckBox
            // 
            this.colorFilesCheckBox.AutoSize = true;
            this.colorFilesCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.colorFilesCheckBox.Location = new System.Drawing.Point(12, 72);
            this.colorFilesCheckBox.Name = "colorFilesCheckBox";
            this.colorFilesCheckBox.Size = new System.Drawing.Size(186, 24);
            this.colorFilesCheckBox.TabIndex = 38;
            this.colorFilesCheckBox.Text = "Color files in tree view";
            this.colorFilesCheckBox.UseVisualStyleBackColor = true;
            // 
            // colorEveryoneCheckBox
            // 
            this.colorEveryoneCheckBox.AutoSize = true;
            this.colorEveryoneCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.colorEveryoneCheckBox.Location = new System.Drawing.Point(12, 114);
            this.colorEveryoneCheckBox.Name = "colorEveryoneCheckBox";
            this.colorEveryoneCheckBox.Size = new System.Drawing.Size(372, 36);
            this.colorEveryoneCheckBox.TabIndex = 39;
            this.colorEveryoneCheckBox.Text = "Color lines with Eveyone string";
            this.colorEveryoneCheckBox.UseVisualStyleBackColor = true;
            // 
            // WindowOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 236);
            this.Controls.Add(this.colorEveryoneCheckBox);
            this.Controls.Add(this.colorFilesCheckBox);
            this.Controls.Add(this.hideEmptyFilesCheckBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Name = "WindowOptionsForm";
            this.Text = "Window Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.CheckBox hideEmptyFilesCheckBox;
        private System.Windows.Forms.CheckBox colorFilesCheckBox;
        private System.Windows.Forms.CheckBox colorEveryoneCheckBox;
    }
}