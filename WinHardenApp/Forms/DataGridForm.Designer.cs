namespace WinHardenApp.Forms
{
    partial class DataGridForm
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
            this.csvDataGridView = new System.Windows.Forms.DataGridView();
            this.cancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.csvDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // csvDataGridView
            // 
            this.csvDataGridView.AllowUserToAddRows = false;
            this.csvDataGridView.AllowUserToDeleteRows = false;
            this.csvDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.csvDataGridView.Location = new System.Drawing.Point(12, 24);
            this.csvDataGridView.MultiSelect = false;
            this.csvDataGridView.Name = "csvDataGridView";
            this.csvDataGridView.ReadOnly = true;
            this.csvDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.csvDataGridView.Size = new System.Drawing.Size(1041, 758);
            this.csvDataGridView.TabIndex = 124;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(978, 806);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 126;
            this.cancelButton.Text = "Close";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // DataGridForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1073, 841);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.csvDataGridView);
            this.Name = "DataGridForm";
            this.Text = "Grid view";
            ((System.ComponentModel.ISupportInitialize)(this.csvDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView csvDataGridView;
        private System.Windows.Forms.Button cancelButton;
    }
}