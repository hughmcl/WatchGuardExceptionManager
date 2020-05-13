namespace WatchGuardExceptionManager {
    partial class addExceptionsImportListForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.addExceptionsAliasLabel = new System.Windows.Forms.Label();
            this.addExceptionsImportAliasBox = new System.Windows.Forms.ComboBox();
            this.addExceptionsListSelectFileButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(115, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(520, 46);
            this.label1.TabIndex = 0;
            this.label1.Text = "Add Exceptions - Import List";
            // 
            // addExceptionsAliasLabel
            // 
            this.addExceptionsAliasLabel.AutoSize = true;
            this.addExceptionsAliasLabel.Location = new System.Drawing.Point(50, 86);
            this.addExceptionsAliasLabel.Name = "addExceptionsAliasLabel";
            this.addExceptionsAliasLabel.Size = new System.Drawing.Size(42, 17);
            this.addExceptionsAliasLabel.TabIndex = 1;
            this.addExceptionsAliasLabel.Text = "Alias:";
            // 
            // addExceptionsImportAliasBox
            // 
            this.addExceptionsImportAliasBox.FormattingEnabled = true;
            this.addExceptionsImportAliasBox.Location = new System.Drawing.Point(99, 86);
            this.addExceptionsImportAliasBox.Name = "addExceptionsImportAliasBox";
            this.addExceptionsImportAliasBox.Size = new System.Drawing.Size(328, 24);
            this.addExceptionsImportAliasBox.TabIndex = 2;
            // 
            // addExceptionsListSelectFileButton
            // 
            this.addExceptionsListSelectFileButton.Location = new System.Drawing.Point(266, 141);
            this.addExceptionsListSelectFileButton.Name = "addExceptionsListSelectFileButton";
            this.addExceptionsListSelectFileButton.Size = new System.Drawing.Size(210, 23);
            this.addExceptionsListSelectFileButton.TabIndex = 3;
            this.addExceptionsListSelectFileButton.Text = "Select File";
            this.addExceptionsListSelectFileButton.UseVisualStyleBackColor = true;
            this.addExceptionsListSelectFileButton.Click += new System.EventHandler(this.addExceptionsListSelectFileButton_Click);
            // 
            // addExceptionsImportListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 252);
            this.Controls.Add(this.addExceptionsListSelectFileButton);
            this.Controls.Add(this.addExceptionsImportAliasBox);
            this.Controls.Add(this.addExceptionsAliasLabel);
            this.Controls.Add(this.label1);
            this.Name = "addExceptionsImportListForm";
            this.Text = "addExceptionsImportListForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label addExceptionsAliasLabel;
        private System.Windows.Forms.ComboBox addExceptionsImportAliasBox;
        private System.Windows.Forms.Button addExceptionsListSelectFileButton;
    }
}