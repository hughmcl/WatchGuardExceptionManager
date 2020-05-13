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

namespace WatchGuardExceptionManager {
    public partial class AddUserForm : Form {
        DataGridView myUserGrid;

        public AddUserForm(DataGridView theGrid) {
            InitializeComponent();
            DataSet ds = Program.utilities.getRolesDataSet();
           
            this.addUserRoleSelect.DataSource = ds.Tables[0];
            this.addUserRoleSelect.DisplayMember = "roleName";
            myUserGrid = theGrid;
        }

        private void addUserCancelButton_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void addUserAddButton_Click(object sender, EventArgs e) {
            bool userAdded = false;
            string username = addUserUsernameText.Text;
            string password = addUserPassword.Text;
            string verifyPassword = addPasswordVerify.Text;
            string firstName = addUserFirstName.Text;
            string surName = addUserSurname.Text;
            DataRowView rowView = (DataRowView)addUserRoleSelect.SelectedItem;

            string userRoleName = rowView["roleName"].ToString();
            int userRoleID = Convert.ToInt32(rowView["id"].ToString());
            
            if (password != verifyPassword) {
                MessageBox.Show("Passwords don't match!");
            } else {
                userAdded = Program.utilities.addUser(username, firstName, surName, password, userRoleID);
                if (userAdded) {
                    myUserGrid.Refresh();

                    this.Close();
                } else {
                    MessageBox.Show("Error adding user!");
                }
            }
        }

        private void addUserRoleSelect_SelectedIndexChanged(object sender, EventArgs e) {

        }
    }
}
