namespace WatchGuardExceptionManager {
    partial class AddExceptionsForm {
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
            this.components = new System.ComponentModel.Container();
            this.addExceptionsHeaderLabel = new System.Windows.Forms.Label();
            this.aliasNameBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.aliasTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.addExceptionsSaveButton = new System.Windows.Forms.Button();
            this.addExceptionsCancelButton = new System.Windows.Forms.Button();
            this.addExceptionsDataGrid = new System.Windows.Forms.DataGridView();
            this.addExceptionsImportListButton = new System.Windows.Forms.Button();
            this.addExceptionsPasteListButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.aliasNameBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aliasTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.addExceptionsDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // addExceptionsHeaderLabel
            // 
            this.addExceptionsHeaderLabel.AutoSize = true;
            this.addExceptionsHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addExceptionsHeaderLabel.Location = new System.Drawing.Point(420, 9);
            this.addExceptionsHeaderLabel.Name = "addExceptionsHeaderLabel";
            this.addExceptionsHeaderLabel.Size = new System.Drawing.Size(300, 46);
            this.addExceptionsHeaderLabel.TabIndex = 0;
            this.addExceptionsHeaderLabel.Text = "Add Exceptions";
            // 
            // addExceptionsSaveButton
            // 
            this.addExceptionsSaveButton.Location = new System.Drawing.Point(479, 58);
            this.addExceptionsSaveButton.Name = "addExceptionsSaveButton";
            this.addExceptionsSaveButton.Size = new System.Drawing.Size(116, 23);
            this.addExceptionsSaveButton.TabIndex = 2;
            this.addExceptionsSaveButton.Text = "Save";
            this.addExceptionsSaveButton.UseVisualStyleBackColor = true;
            this.addExceptionsSaveButton.Click += new System.EventHandler(this.addExceptionsSaveButton_Click);
            // 
            // addExceptionsCancelButton
            // 
            this.addExceptionsCancelButton.Location = new System.Drawing.Point(712, 57);
            this.addExceptionsCancelButton.Name = "addExceptionsCancelButton";
            this.addExceptionsCancelButton.Size = new System.Drawing.Size(114, 23);
            this.addExceptionsCancelButton.TabIndex = 3;
            this.addExceptionsCancelButton.Text = "Cancel";
            this.addExceptionsCancelButton.UseVisualStyleBackColor = true;
            this.addExceptionsCancelButton.Click += new System.EventHandler(this.addExceptionsCancelButton_Click);
            // 
            // addExceptionsDataGrid
            // 
            this.addExceptionsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.addExceptionsDataGrid.Location = new System.Drawing.Point(3, 96);
            this.addExceptionsDataGrid.Name = "addExceptionsDataGrid";
            this.addExceptionsDataGrid.RowHeadersWidth = 51;
            this.addExceptionsDataGrid.RowTemplate.Height = 24;
            this.addExceptionsDataGrid.Size = new System.Drawing.Size(1163, 477);
            this.addExceptionsDataGrid.TabIndex = 4;
            // 
            // addExceptionsImportListButton
            // 
            this.addExceptionsImportListButton.Location = new System.Drawing.Point(47, 57);
            this.addExceptionsImportListButton.Name = "addExceptionsImportListButton";
            this.addExceptionsImportListButton.Size = new System.Drawing.Size(167, 23);
            this.addExceptionsImportListButton.TabIndex = 5;
            this.addExceptionsImportListButton.Text = "Import List";
            this.addExceptionsImportListButton.UseVisualStyleBackColor = true;
            this.addExceptionsImportListButton.Click += new System.EventHandler(this.addExceptionsImportListButton_Click);
            // 
            // addExceptionsPasteListButton
            // 
            this.addExceptionsPasteListButton.Location = new System.Drawing.Point(271, 57);
            this.addExceptionsPasteListButton.Name = "addExceptionsPasteListButton";
            this.addExceptionsPasteListButton.Size = new System.Drawing.Size(155, 23);
            this.addExceptionsPasteListButton.TabIndex = 6;
            this.addExceptionsPasteListButton.Text = "Paste List";
            this.addExceptionsPasteListButton.UseVisualStyleBackColor = true;
            this.addExceptionsPasteListButton.Click += new System.EventHandler(this.addExceptionsPasteListButton_Click);
            // 
            // AddExceptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 585);
            this.Controls.Add(this.addExceptionsPasteListButton);
            this.Controls.Add(this.addExceptionsImportListButton);
            this.Controls.Add(this.addExceptionsDataGrid);
            this.Controls.Add(this.addExceptionsCancelButton);
            this.Controls.Add(this.addExceptionsSaveButton);
            this.Controls.Add(this.addExceptionsHeaderLabel);
            this.Name = "AddExceptionsForm";
            this.Text = "WatchGuard Proxy Exception Manager - Add Exceptions";
            this.Load += new System.EventHandler(this.AddExceptionsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.aliasNameBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aliasTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.addExceptionsDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label addExceptionsHeaderLabel;
        private System.Windows.Forms.BindingSource aliasTypeBindingSource;
        private System.Windows.Forms.BindingSource aliasNameBindingSource;
        private System.Windows.Forms.Button addExceptionsSaveButton;
        private System.Windows.Forms.Button addExceptionsCancelButton;
        private System.Windows.Forms.DataGridView addExceptionsDataGrid;
        private System.Windows.Forms.Button addExceptionsImportListButton;
        private System.Windows.Forms.Button addExceptionsPasteListButton;
    }
}