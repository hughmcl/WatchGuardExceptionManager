using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.IO;

namespace WatchGuardExceptionManager {
    public partial class SetUserPasswordForm : Form {
        private string myUsername;

        public SetUserPasswordForm(string username, string FullName) {
            InitializeComponent();
            myUsername = username;
            this.passwordUsernameText.Text = username;
            this.passwordFullNameText.Text = FullName;
        }

        private void SetUserPasswordForm_Load(object sender, EventArgs e) {

        }

        private void passwordUsernameLabel_Click(object sender, EventArgs e) {

        }

        private void usernameNameLabel_Click(object sender, EventArgs e) {

        }

        private void newPasswordLabel_Click(object sender, EventArgs e) {

        }

        private void verifyPasswordLabel_Click(object sender, EventArgs e) {

        }

        private void passwordUpdateButton_Click(object sender, EventArgs e) {
            string encryptedPassword;
            

            if (passwordField1.Text != passwordVerifyText.Text) {
                MessageBox.Show("Passwords don't match!");
            } else {
                String cs = @"server=10.1.30.29;database=WatchGuardExceptions;uid=WatchGuardUser;pwd=password";
                
                encryptedPassword = Program.utilities.EncryptString(passwordField1.Text);

                //Create SqlConnection
                MySql.Data.MySqlClient.MySqlConnection dbConnection = new MySql.Data.MySqlClient.MySqlConnection();
                dbConnection.ConnectionString = cs;
                dbConnection.Open();

                string sql = "UPDATE users SET password=@newpassword WHERE Username=@username ";
                MySqlCommand cmd = new MySqlCommand(sql, dbConnection);
                cmd.Parameters.AddWithValue("@username", myUsername);
                cmd.Parameters.AddWithValue("@newpassword", encryptedPassword);
                MySqlDataReader reader = cmd.ExecuteReader();
                dbConnection.Close();
                this.Close();

            }
        }


        



        private void passwordCancelButton_Click(object sender, EventArgs e) {
            this.Close();
        }



    }
}
