namespace WatchGuardExceptionManager {
    partial class addExceptionsPasteListForm {
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
            this.addExceptionsPasteListLabel = new System.Windows.Forms.Label();
            this.addExceptionsAliasLabel = new System.Windows.Forms.Label();
            this.addExceptionsPasteAliasBox = new System.Windows.Forms.ComboBox();
            this.addExceptionsPasteListBox = new System.Windows.Forms.RichTextBox();
            this.addExceptionsPasteSubmit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // addExceptionsPasteListLabel
            // 
            this.addExceptionsPasteListLabel.AutoSize = true;
            this.addExceptionsPasteListLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addExceptionsPasteListLabel.Location = new System.Drawing.Point(231, 9);
            this.addExceptionsPasteListLabel.Name = "addExceptionsPasteListLabel";
            this.addExceptionsPasteListLabel.Size = new System.Drawing.Size(510, 46);
            this.addExceptionsPasteListLabel.TabIndex = 1;
            this.addExceptionsPasteListLabel.Text = "Add Exceptions - Paste List";
            // 
            // addExceptionsAliasLabel
            // 
            this.addExceptionsAliasLabel.AutoSize = true;
            this.addExceptionsAliasLabel.Location = new System.Drawing.Point(91, 70);
            this.addExceptionsAliasLabel.Name = "addExceptionsAliasLabel";
            this.addExceptionsAliasLabel.Size = new System.Drawing.Size(42, 17);
            this.addExceptionsAliasLabel.TabIndex = 2;
            this.addExceptionsAliasLabel.Text = "Alias:";
            // 
            // addExceptionsPasteAliasBox
            // 
            this.addExceptionsPasteAliasBox.FormattingEnabled = true;
            this.addExceptionsPasteAliasBox.Location = new System.Drawing.Point(139, 67);
            this.addExceptionsPasteAliasBox.Name = "addExceptionsPasteAliasBox";
            this.addExceptionsPasteAliasBox.Size = new System.Drawing.Size(328, 24);
            this.addExceptionsPasteAliasBox.TabIndex = 3;
            // 
            // addExceptionsPasteListBox
            // 
            this.addExceptionsPasteListBox.Location = new System.Drawing.Point(139, 118);
            this.addExceptionsPasteListBox.Name = "addExceptionsPasteListBox";
            this.addExceptionsPasteListBox.Size = new System.Drawing.Size(664, 384);
            this.addExceptionsPasteListBox.TabIndex = 4;
            this.addExceptionsPasteListBox.Text = "";
            // 
            // addExceptionsPasteSubmit
            // 
            this.addExceptionsPasteSubmit.Location = new System.Drawing.Point(423, 520);
            this.addExceptionsPasteSubmit.Name = "addExceptionsPasteSubmit";
            this.addExceptionsPasteSubmit.Size = new System.Drawing.Size(100, 23);
            this.addExceptionsPasteSubmit.TabIndex = 5;
            this.addExceptionsPasteSubmit.Text = "Submit";
            this.addExceptionsPasteSubmit.UseVisualStyleBackColor = true;
            this.addExceptionsPasteSubmit.Click += new System.EventHandler(this.addExceptionsPasteSubmit_Click);
            // 
            // addExceptionsPasteListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1042, 562);
            this.Controls.Add(this.addExceptionsPasteSubmit);
            this.Controls.Add(this.addExceptionsPasteListBox);
            this.Controls.Add(this.addExceptionsPasteAliasBox);
            this.Controls.Add(this.addExceptionsAliasLabel);
            this.Controls.Add(this.addExceptionsPasteListLabel);
            this.Name = "addExceptionsPasteListForm";
            this.Text = "addExceptionsPasteListForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label addExceptionsPasteListLabel;
        private System.Windows.Forms.Label addExceptionsAliasLabel;
        private System.Windows.Forms.ComboBox addExceptionsPasteAliasBox;
        private System.Windows.Forms.RichTextBox addExceptionsPasteListBox;
        private System.Windows.Forms.Button addExceptionsPasteSubmit;
    }
}