using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;

namespace WatchGuardExceptionManager {
    public partial class loginForm : Form {

        public UserInfoStruct myUserInfo;


        public loginForm() {
            InitializeComponent();
        }


        private void loginButton_Click(object sender, EventArgs e) {
            bool loggedIn = false;

            Program.utilities.getADGroups(Program.myAppSettings.myADSettings);

            if (usernameField.Text == "" || passwordField.Text == "") {
                MessageBox.Show("Please provide UserName and Password");
                return;
            }

            loggedIn = Program.utilities.GetLoginAD(ref myUserInfo, usernameField.Text, passwordField.Text);
            if (!loggedIn) {
                loggedIn = Program.utilities.GetLogin(ref myUserInfo, usernameField.Text, passwordField.Text);
                myUserInfo.loginError += "Also failed local auth!";
            }

            if (loggedIn) {
            
                // hide the login form
                this.Hide();

                // create and show the main form and pass the userInfo to it
                MainForm fm = new MainForm(myUserInfo);
                fm.Show();
            } else {
                if (myUserInfo.locked == true) {
                    MessageBox.Show("Login Failed!  Account Locked!\n"+myUserInfo.loginError);
                } else {
                    MessageBox.Show("Login Failed!\n"+myUserInfo.loginError);
                }
            }
        }

        private void loginExitButton_Click(object sender, EventArgs e) {
            System.Windows.Forms.Application.Exit();
        }
    }
}
