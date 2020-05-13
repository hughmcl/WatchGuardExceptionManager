namespace WatchGuardExceptionManager
{
    partial class AddUserForm
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
            this.addUserLabel = new System.Windows.Forms.Label();
            this.addUserUsernameLabel = new System.Windows.Forms.Label();
            this.addUserUsernameText = new System.Windows.Forms.TextBox();
            this.addUserAddButton = new System.Windows.Forms.Button();
            this.addUserCancelButton = new System.Windows.Forms.Button();
            this.addUserRoleLabel = new System.Windows.Forms.Label();
            this.addUserPasswordLabel = new System.Windows.Forms.Label();
            this.addUserPassword = new System.Windows.Forms.TextBox();
            this.addPasswordVerifyLabel = new System.Windows.Forms.Label();
            this.addPasswordVerify = new System.Windows.Forms.TextBox();
            this.addUserRoleSelect = new System.Windows.Forms.ComboBox();
            this.addUserFirstNameLabel = new System.Windows.Forms.Label();
            this.addUserSurnameLabel = new System.Windows.Forms.Label();
            this.addUserFirstName = new System.Windows.Forms.TextBox();
            this.addUserSurname = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // addUserLabel
            // 
            this.addUserLabel.AutoSize = true;
            this.addUserLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addUserLabel.Location = new System.Drawing.Point(290, 13);
            this.addUserLabel.Name = "addUserLabel";
            this.addUserLabel.Size = new System.Drawing.Size(186, 46);
            this.addUserLabel.TabIndex = 0;
            this.addUserLabel.Text = "Add User";
            // 
            // addUserUsernameLabel
            // 
            this.addUserUsernameLabel.CausesValidation = false;
            this.addUserUsernameLabel.Location = new System.Drawing.Point(58, 69);
            this.addUserUsernameLabel.Name = "addUserUsernameLabel";
            this.addUserUsernameLabel.Size = new System.Drawing.Size(77, 17);
            this.addUserUsernameLabel.TabIndex = 1;
            this.addUserUsernameLabel.Text = "Username:";
            // 
            // addUserUsernameText
            // 
            this.addUserUsernameText.Location = new System.Drawing.Point(142, 69);
            this.addUserUsernameText.Name = "addUserUsernameText";
            this.addUserUsernameText.Size = new System.Drawing.Size(236, 22);
            this.addUserUsernameText.TabIndex = 2;
            // 
            // addUserAddButton
            // 
            this.addUserAddButton.Location = new System.Drawing.Point(223, 390);
            this.addUserAddButton.Name = "addUserAddButton";
            this.addUserAddButton.Size = new System.Drawing.Size(75, 23);
            this.addUserAddButton.TabIndex = 13;
            this.addUserAddButton.Text = "Add User";
            this.addUserAddButton.UseVisualStyleBackColor = true;
            this.addUserAddButton.Click += new System.EventHandler(this.addUserAddButton_Click);
            // 
            // addUserCancelButton
            // 
            this.addUserCancelButton.Location = new System.Drawing.Point(392, 390);
            this.addUserCancelButton.Name = "addUserCancelButton";
            this.addUserCancelButton.Size = new System.Drawing.Size(75, 23);
            this.addUserCancelButton.TabIndex = 14;
            this.addUserCancelButton.Text = "Cancel";
            this.addUserCancelButton.UseVisualStyleBackColor = true;
            this.addUserCancelButton.Click += new System.EventHandler(this.addUserCancelButton_Click);
            // 
            // addUserRoleLabel
            // 
            this.addUserRoleLabel.AutoSize = true;
            this.addUserRoleLabel.Location = new System.Drawing.Point(94, 193);
            this.addUserRoleLabel.Name = "addUserRoleLabel";
            this.addUserRoleLabel.Size = new System.Drawing.Size(41, 17);
            this.addUserRoleLabel.TabIndex = 7;
            this.addUserRoleLabel.Text = "Role:";
            // 
            // addUserPasswordLabel
            // 
            this.addUserPasswordLabel.AutoSize = true;
            this.addUserPasswordLabel.Location = new System.Drawing.Point(62, 235);
            this.addUserPasswordLabel.Name = "addUserPasswordLabel";
            this.addUserPasswordLabel.Size = new System.Drawing.Size(73, 17);
            this.addUserPasswordLabel.TabIndex = 9;
            this.addUserPasswordLabel.Text = "Password:";
            // 
            // addUserPassword
            // 
            this.addUserPassword.Location = new System.Drawing.Point(142, 235);
            this.addUserPassword.Name = "addUserPassword";
            this.addUserPassword.PasswordChar = '*';
            this.addUserPassword.Size = new System.Drawing.Size(236, 22);
            this.addUserPassword.TabIndex = 10;
            this.addUserPassword.WordWrap = false;
            // 
            // addPasswordVerifyLabel
            // 
            this.addPasswordVerifyLabel.AutoSize = true;
            this.addPasswordVerifyLabel.Location = new System.Drawing.Point(22, 284);
            this.addPasswordVerifyLabel.Name = "addPasswordVerifyLabel";
            this.addPasswordVerifyLabel.Size = new System.Drawing.Size(113, 17);
            this.addPasswordVerifyLabel.TabIndex = 11;
            this.addPasswordVerifyLabel.Text = "Verify Password:";
            // 
            // addPasswordVerify
            // 
            this.addPasswordVerify.Location = new System.Drawing.Point(142, 284);
            this.addPasswordVerify.Name = "addPasswordVerify";
            this.addPasswordVerify.PasswordChar = '*';
            this.addPasswordVerify.Size = new System.Drawing.Size(236, 22);
            this.addPasswordVerify.TabIndex = 12;
            this.addPasswordVerify.WordWrap = false;
            // 
            // addUserRoleSelect
            // 
            this.addUserRoleSelect.FormattingEnabled = true;
            this.addUserRoleSelect.Location = new System.Drawing.Point(142, 193);
            this.addUserRoleSelect.Name = "addUserRoleSelect";
            this.addUserRoleSelect.Size = new System.Drawing.Size(236, 24);
            this.addUserRoleSelect.TabIndex = 8;
            // 
            // addUserFirstNameLabel
            // 
            this.addUserFirstNameLabel.AutoSize = true;
            this.addUserFirstNameLabel.Location = new System.Drawing.Point(55, 109);
            this.addUserFirstNameLabel.Name = "addUserFirstNameLabel";
            this.addUserFirstNameLabel.Size = new System.Drawing.Size(80, 17);
            this.addUserFirstNameLabel.TabIndex = 3;
            this.addUserFirstNameLabel.Text = "First Name:";
            // 
            // addUserSurnameLabel
            // 
            this.addUserSurnameLabel.AutoSize = true;
            this.addUserSurnameLabel.Location = new System.Drawing.Point(66, 148);
            this.addUserSurnameLabel.Name = "addUserSurnameLabel";
            this.addUserSurnameLabel.Size = new System.Drawing.Size(69, 17);
            this.addUserSurnameLabel.TabIndex = 5;
            this.addUserSurnameLabel.Text = "Surname:";
            // 
            // addUserFirstName
            // 
            this.addUserFirstName.Location = new System.Drawing.Point(142, 109);
            this.addUserFirstName.Name = "addUserFirstName";
            this.addUserFirstName.Size = new System.Drawing.Size(236, 22);
            this.addUserFirstName.TabIndex = 4;
            // 
            // addUserSurname
            // 
            this.addUserSurname.Location = new System.Drawing.Point(142, 148);
            this.addUserSurname.Name = "addUserSurname";
            this.addUserSurname.Size = new System.Drawing.Size(236, 22);
            this.addUserSurname.TabIndex = 6;
            // 
            // AddUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.addUserSurname);
            this.Controls.Add(this.addUserFirstName);
            this.Controls.Add(this.addUserSurnameLabel);
            this.Controls.Add(this.addUserFirstNameLabel);
            this.Controls.Add(this.addUserRoleSelect);
            this.Controls.Add(this.addPasswordVerify);
            this.Controls.Add(this.addPasswordVerifyLabel);
            this.Controls.Add(this.addUserPassword);
            this.Controls.Add(this.addUserPasswordLabel);
            this.Controls.Add(this.addUserRoleLabel);
            this.Controls.Add(this.addUserCancelButton);
            this.Controls.Add(this.addUserAddButton);
            this.Controls.Add(this.addUserUsernameText);
            this.Controls.Add(this.addUserUsernameLabel);
            this.Controls.Add(this.addUserLabel);
            this.Name = "AddUserForm";
            this.Text = "WatchGuard Proxy Exception Manager - Add User";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label addUserLabel;
        private System.Windows.Forms.Label addUserUsernameLabel;
        private System.Windows.Forms.TextBox addUserUsernameText;
        private System.Windows.Forms.Button addUserAddButton;
        private System.Windows.Forms.Button addUserCancelButton;
        private System.Windows.Forms.Label addUserRoleLabel;
        private System.Windows.Forms.Label addUserPasswordLabel;
        private System.Windows.Forms.TextBox addUserPassword;
        private System.Windows.Forms.Label addPasswordVerifyLabel;
        private System.Windows.Forms.TextBox addPasswordVerify;
        private System.Windows.Forms.ComboBox addUserRoleSelect;
        private System.Windows.Forms.Label addUserFirstNameLabel;
        private System.Windows.Forms.Label addUserSurnameLabel;
        private System.Windows.Forms.TextBox addUserFirstName;
        private System.Windows.Forms.TextBox addUserSurname;
    }
}