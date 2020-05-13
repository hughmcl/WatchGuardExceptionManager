namespace WatchGuardExceptionManager {
    partial class addAliasNameForm {
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
            this.addAliasNameGrid = new System.Windows.Forms.DataGridView();
            this.addAliasNameLabel = new System.Windows.Forms.Label();
            this.aliasAddSaveButton = new System.Windows.Forms.Button();
            this.AliasName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.addAliasNameGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // addAliasNameGrid
            // 
            this.addAliasNameGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.addAliasNameGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AliasName});
            this.addAliasNameGrid.Location = new System.Drawing.Point(3, 92);
            this.addAliasNameGrid.Name = "addAliasNameGrid";
            this.addAliasNameGrid.RowHeadersWidth = 51;
            this.addAliasNameGrid.RowTemplate.Height = 24;
            this.addAliasNameGrid.Size = new System.Drawing.Size(793, 357);
            this.addAliasNameGrid.TabIndex = 0;
            this.addAliasNameGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.addAliasNameGrid_CellContentClick);
            // 
            // addAliasNameLabel
            // 
            this.addAliasNameLabel.AutoSize = true;
            this.addAliasNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addAliasNameLabel.Location = new System.Drawing.Point(271, 9);
            this.addAliasNameLabel.Name = "addAliasNameLabel";
            this.addAliasNameLabel.Size = new System.Drawing.Size(189, 46);
            this.addAliasNameLabel.TabIndex = 1;
            this.addAliasNameLabel.Text = "Add Alias";
            // 
            // aliasAddSaveButton
            // 
            this.aliasAddSaveButton.Location = new System.Drawing.Point(315, 58);
            this.aliasAddSaveButton.Name = "aliasAddSaveButton";
            this.aliasAddSaveButton.Size = new System.Drawing.Size(94, 23);
            this.aliasAddSaveButton.TabIndex = 2;
            this.aliasAddSaveButton.Text = "Save";
            this.aliasAddSaveButton.UseVisualStyleBackColor = true;
            this.aliasAddSaveButton.Click += new System.EventHandler(this.aliasAddSaveButton_Click);
            // 
            // AliasName
            // 
            this.AliasName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AliasName.HeaderText = "Alias";
            this.AliasName.MinimumWidth = 6;
            this.AliasName.Name = "AliasName";
            // 
            // addAliasNameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.aliasAddSaveButton);
            this.Controls.Add(this.addAliasNameLabel);
            this.Controls.Add(this.addAliasNameGrid);
            this.Name = "addAliasNameForm";
            this.Text = "addAliasNameForm";
            ((System.ComponentModel.ISupportInitialize)(this.addAliasNameGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView addAliasNameGrid;
        private System.Windows.Forms.Label addAliasNameLabel;
        private System.Windows.Forms.Button aliasAddSaveButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn AliasName;
    }
}