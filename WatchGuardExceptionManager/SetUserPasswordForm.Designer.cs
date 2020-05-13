namespace WatchGuardExceptionManager
{
    partial class SetUserPasswordForm
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
            this.changePasswordLabel = new System.Windows.Forms.Label();
            this.newPasswordLabel = new System.Windows.Forms.Label();
            this.verifyPasswordLabel = new System.Windows.Forms.Label();
            this.passwordUsernameLabel = new System.Windows.Forms.Label();
            this.usernameNameLabel = new System.Windows.Forms.Label();
            this.passwordUpdateButton = new System.Windows.Forms.Button();
            this.passwordCancelButton = new System.Windows.Forms.Button();
            this.passwordUsernameText = new System.Windows.Forms.Label();
            this.passwordFullNameText = new System.Windows.Forms.Label();
            this.passwordField1 = new System.Windows.Forms.TextBox();
            this.passwordVerifyText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // changePasswordLabel
            // 
            this.changePasswordLabel.AutoSize = true;
            this.changePasswordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changePasswordLabel.Location = new System.Drawing.Point(81, 20);
            this.changePasswordLabel.Name = "changePasswordLabel";
            this.changePasswordLabel.Size = new System.Drawing.Size(346, 46);
            this.changePasswordLabel.TabIndex = 0;
            this.changePasswordLabel.Text = "Change Password";
            // 
            // newPasswordLabel
            // 
            this.newPasswordLabel.AutoSize = true;
            this.newPasswordLabel.Location = new System.Drawing.Point(57, 162);
            this.newPasswordLabel.Name = "newPasswordLabel";
            this.newPasswordLabel.Size = new System.Drawing.Size(104, 17);
            this.newPasswordLabel.TabIndex = 1;
            this.newPasswordLabel.Text = "New Password:";
            this.newPasswordLabel.Click += new System.EventHandler(this.newPasswordLabel_Click);
            // 
            // verifyPasswordLabel
            // 
            this.verifyPasswordLabel.AutoSize = true;
            this.verifyPasswordLabel.Location = new System.Drawing.Point(48, 200);
            this.verifyPasswordLabel.Name = "verifyPasswordLabel";
            this.verifyPasswordLabel.Size = new System.Drawing.Size(113, 17);
            this.verifyPasswordLabel.TabIndex = 2;
            this.verifyPasswordLabel.Text = "Verify Password:";
            this.verifyPasswordLabel.Click += new System.EventHandler(this.verifyPasswordLabel_Click);
            // 
            // passwordUsernameLabel
            // 
            this.passwordUsernameLabel.AutoSize = true;
            this.passwordUsernameLabel.Location = new System.Drawing.Point(84, 86);
            this.passwordUsernameLabel.Name = "passwordUsernameLabel";
            this.passwordUsernameLabel.Size = new System.Drawing.Size(77, 17);
            this.passwordUsernameLabel.TabIndex = 3;
            this.passwordUsernameLabel.Text = "Username:";
            this.passwordUsernameLabel.Click += new System.EventHandler(this.passwordUsernameLabel_Click);
            // 
            // usernameNameLabel
            // 
            this.usernameNameLabel.AutoSize = true;
            this.usernameNameLabel.Location = new System.Drawing.Point(86, 120);
            this.usernameNameLabel.Name = "usernameNameLabel";
            this.usernameNameLabel.Size = new System.Drawing.Size(75, 17);
            this.usernameNameLabel.TabIndex = 4;
            this.usernameNameLabel.Text = "Full Name:";
            this.usernameNameLabel.Click += new System.EventHandler(this.usernameNameLabel_Click);
            // 
            // passwordUpdateButton
            // 
            this.passwordUpdateButton.Location = new System.Drawing.Point(146, 257);
            this.passwordUpdateButton.Name = "passwordUpdateButton";
            this.passwordUpdateButton.Size = new System.Drawing.Size(75, 23);
            this.passwordUpdateButton.TabIndex = 5;
            this.passwordUpdateButton.Text = "Update";
            this.passwordUpdateButton.UseVisualStyleBackColor = true;
            this.passwordUpdateButton.Click += new System.EventHandler(this.passwordUpdateButton_Click);
            // 
            // passwordCancelButton
            // 
            this.passwordCancelButton.Location = new System.Drawing.Point(306, 257);
            this.passwordCancelButton.Name = "passwordCancelButton";
            this.passwordCancelButton.Size = new System.Drawing.Size(75, 23);
            this.passwordCancelButton.TabIndex = 6;
            this.passwordCancelButton.Text = "Cancel";
            this.passwordCancelButton.UseVisualStyleBackColor = true;
            this.passwordCancelButton.Click += new System.EventHandler(this.passwordCancelButton_Click);
            // 
            // passwordUsernameText
            // 
            this.passwordUsernameText.AutoSize = true;
            this.passwordUsernameText.Location = new System.Drawing.Point(168, 86);
            this.passwordUsernameText.Name = "passwordUsernameText";
            this.passwordUsernameText.Size = new System.Drawing.Size(133, 17);
            this.passwordUsernameText.TabIndex = 7;
            this.passwordUsernameText.Text = "passwordUsername";
            // 
            // passwordFullNameText
            // 
            this.passwordFullNameText.AutoSize = true;
            this.passwordFullNameText.Location = new System.Drawing.Point(171, 120);
            this.passwordFullNameText.Name = "passwordFullNameText";
            this.passwordFullNameText.Size = new System.Drawing.Size(154, 17);
            this.passwordFullNameText.TabIndex = 8;
            this.passwordFullNameText.Text = "passwordFullNameText";
            // 
            // passwordField1
            // 
            this.passwordField1.Location = new System.Drawing.Point(174, 162);
            this.passwordField1.Name = "passwordField1";
            this.passwordField1.PasswordChar = '*';
            this.passwordField1.Size = new System.Drawing.Size(223, 22);
            this.passwordField1.TabIndex = 9;
            this.passwordField1.UseSystemPasswordChar = true;
            this.passwordField1.WordWrap = false;
            // 
            // passwordVerifyText
            // 
            this.passwordVerifyText.Location = new System.Drawing.Point(174, 200);
            this.passwordVerifyText.Name = "passwordVerifyText";
            this.passwordVerifyText.PasswordChar = '*';
            this.passwordVerifyText.Size = new System.Drawing.Size(223, 22);
            this.passwordVerifyText.TabIndex = 10;
            // 
            // SetUserPasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 319);
            this.Controls.Add(this.passwordVerifyText);
            this.Controls.Add(this.passwordField1);
            this.Controls.Add(this.passwordFullNameText);
            this.Controls.Add(this.passwordUsernameText);
            this.Controls.Add(this.passwordCancelButton);
            this.Controls.Add(this.passwordUpdateButton);
            this.Controls.Add(this.usernameNameLabel);
            this.Controls.Add(this.passwordUsernameLabel);
            this.Controls.Add(this.verifyPasswordLabel);
            this.Controls.Add(this.newPasswordLabel);
            this.Controls.Add(this.changePasswordLabel);
            this.Name = "SetUserPasswordForm";
            this.Text = "Change Password";
            this.Load += new System.EventHandler(this.SetUserPasswordForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label changePasswordLabel;
        private System.Windows.Forms.Label newPasswordLabel;
        private System.Windows.Forms.Label verifyPasswordLabel;
        private System.Windows.Forms.Label passwordUsernameLabel;
        private System.Windows.Forms.Label usernameNameLabel;
        private System.Windows.Forms.Button passwordUpdateButton;
        private System.Windows.Forms.Button passwordCancelButton;
        private System.Windows.Forms.Label passwordUsernameText;
        private System.Windows.Forms.Label passwordFullNameText;
        private System.Windows.Forms.TextBox passwordField1;
        private System.Windows.Forms.TextBox passwordVerifyText;
    }
}