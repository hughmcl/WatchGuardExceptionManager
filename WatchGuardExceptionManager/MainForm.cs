using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.IO;

namespace WatchGuardExceptionManager {
    public partial class MainForm : Form {

       
        public UserInfoStruct userInfo;
        public DataSet AliasTypesDS;
        public DataSet LocalUsersDS;
        public DataSet UserRoleDS;
        public DataSet AliasNamesDS;



        public MainForm(UserInfoStruct theUserInfo) {
            InitializeComponent();
            userInfo = theUserInfo;
            UserNameText.Text = userInfo.FullName;
            PermissionsText.Text = userInfo.PermissionLevelText;

            // set form permissions for read only users
            if (userInfo.PermissionLevelText.Equals("Read-Only User")) {
                dataTypesGrid.ReadOnly = true;
                usersGrid.ReadOnly = true;
                mainFormTabControl.TabPages.Remove(dbSettings);
                mainFormTabControl.TabPages.Remove(usersTab);
                mainFormTabControl.TabPages.Remove(adSettingsTab);
                exceptionsAddExceptionsButton.Visible = false;
                updateButton.Visible = false;
                aliasNameAddButton.Visible = false;
                customerUpdateButton.Visible = false;
                DataTypeUpdateButton.Visible = false;

            }

            // update the dbSettings Tab
            UpdateDBTab();
            UpdateADTab();
        }



        private void UpdateDBTab() {
            dbHostText.Text = Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbHost;
            dbPortText.Text = Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbPort;
            dbUsernameText.Text = Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbUser;
            dbNameText.Text = Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbName;
            dbPasswordText.Text = Program.utilities.DecryptString(Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbPassword);
            dbVerifyPasswordText.Text = Program.utilities.DecryptString(Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbPassword);
        }


        private void UpdateADTab() {  
            adDomainText.Text = Program.myAppSettings.myADSettings.ADDomain;
            adServerPrimaryText.Text = Program.myAppSettings.myADSettings.ADServerPrimary;
            adServerSecondaryText.Text = Program.myAppSettings.myADSettings.ADServerSecondary;
            adPortText.Text = Program.myAppSettings.myADSettings.ADPort.ToString();
            adBindUsernameText.Text = Program.myAppSettings.myADSettings.BindUsername;
            adBindPasswordText.Text = Program.utilities.DecryptString(Program.myAppSettings.myADSettings.BindPassword);
            adBindVerifyPasswordText.Text = Program.utilities.DecryptString(Program.myAppSettings.myADSettings.BindPassword);
            adBindUserBaseText.Text = Program.myAppSettings.myADSettings.ADUserBase;
        }




        private void Form1_Load(object sender, EventArgs e) {


            this.fullExceptionsGrid.RowPrePaint
               += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.fullExceptionsGrid_RowPrePaint);
            this.customerDataGrid.RowPrePaint
               += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.customerDataGrid_RowPrePaint);
            this.usersGrid.RowPrePaint
               += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.usersGrid_RowPrePaint);

            this.adAdminGroupDN.DataSource = Program.myADAdminGroupsBS;
            this.adAdminGroupDN.DisplayMember = Program.myADAdminGroups.Tables["AD Groups"].Columns["Name"].ToString();
            this.adAdminGroupDN.ValueMember = Program.myADAdminGroups.Tables["AD Groups"].Columns["GroupDN"].ToString();
            Program.utilities.SelectItemByValue(adAdminGroupDN, "GroupDN", Program.utilities.DecryptString(Program.utilities.ADAdministratorGroupDN, 846));

            this.adReadOnlyGroupDN.DataSource = Program.myADReadOnlyGroupsBS;
            this.adReadOnlyGroupDN.DisplayMember = Program.myADReadOnlyGroups.Tables["AD Groups"].Columns["Name"].ToString();
            this.adReadOnlyGroupDN.ValueMember = Program.myADReadOnlyGroups.Tables["AD Groups"].Columns["GroupDN"].ToString();
            Program.utilities.SelectItemByValue(adReadOnlyGroupDN, "GroupDN", Program.utilities.DecryptString(Program.utilities.ADReadOnlyGroupDN, 657));

            // populate AliasTypes gridview
            Program.utilities.buildAliasTypeDataGridView(userInfo, dataTypesGrid);
            AliasTypesDS = new DataSet();
            Program.utilities.getAliasTypesDS(AliasTypesDS);
            dataTypesGrid.DataSource = AliasTypesDS.Tables[0];

            // populate customers(Alias Names) gridview
            Program.utilities.buildCustomerDataGridView(userInfo, customerDataGrid);
            AliasNamesDS = new DataSet();
            Program.utilities.getAliasNamesDS(AliasNamesDS);
            customerDataGrid.DataSource = AliasNamesDS.Tables[0];

            // populate local users gridview
            UserRoleDS = new DataSet();
            Program.utilities.getUserRoleDS(UserRoleDS);
            Program.utilities.buildLocalUsersDataGridView(usersGrid, UserRoleDS);
            LocalUsersDS = new DataSet();
            Program.utilities.getLocalUsersDS(LocalUsersDS);
            usersGrid.DataSource = LocalUsersDS.Tables[0];

            // build fullExceptionsGrid gridView
            Program.utilities.buildFullExceptionsDataGridView(userInfo, this.fullExceptionsGrid);
            DataSet fullExceptionsDS = new DataSet();
            Program.utilities.getFullExceptionsDS(fullExceptionsDS);
            fullExceptionsGrid.DataSource = fullExceptionsDS.Tables[0];

        }




        private void FilterBox_TextChanged(object sender, EventArgs e) {
            BindingSource bs = new BindingSource();
            bs.DataSource = fullExceptionsGrid.DataSource;
            bs.Filter = "[AliasName] Like '%" + FilterBox.Text + "%'";
            bs.Filter += "OR [Data] Like '%" + FilterBox.Text + "%'";
            fullExceptionsGrid.DataSource = bs;
        }




        private void customerFilterTextBox_TextChanged(object sender, EventArgs e) {
            BindingSource bs = new BindingSource();
            bs.DataSource = customerDataGrid.DataSource;
            bs.Filter = "[AliasName] Like '%" + customerFilterTextBox.Text + "%'";
            customerDataGrid.DataSource = bs;
        }




        private void fullExceptionsGrid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e) {
            
            if (fullExceptionsGrid.Rows[e.RowIndex].Cells["DeleteEntry"].Value != null
                  && fullExceptionsGrid.Rows[e.RowIndex].Cells["DeleteEntry"].Value.Equals(true)) {
                fullExceptionsGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
            } else {
                if (fullExceptionsGrid.Rows[e.RowIndex].Cells["customerEnabled"].Value != null
                    && fullExceptionsGrid.Rows[e.RowIndex].Cells["customerEnabled"].Value.Equals(false)) {
                    fullExceptionsGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Cyan;
                } else if (fullExceptionsGrid.Rows[e.RowIndex].Cells["EntryEnabled"].Value != null
                           && fullExceptionsGrid.Rows[e.RowIndex].Cells["EntryEnabled"].Value.Equals(false)) {
                    fullExceptionsGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                } else if (fullExceptionsGrid.Rows[e.RowIndex].Cells["EntryEnabled"].Value != null
                  && fullExceptionsGrid.Rows[e.RowIndex].Cells["EntryEnabled"].Value.Equals(true)) {
                    fullExceptionsGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            }
            
        }




        private void usersGrid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e) {
            if (usersGrid.Rows[e.RowIndex].Cells["DeleteEntry"].Value != null
                  && usersGrid.Rows[e.RowIndex].Cells["DeleteEntry"].Value.Equals(true)) {
                usersGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
            } else {
                if (usersGrid.Rows[e.RowIndex].Cells["Locked"].Value != null
                  && usersGrid.Rows[e.RowIndex].Cells["Locked"].Value.Equals(true)) {
                    usersGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                } else if (usersGrid.Rows[e.RowIndex].Cells["Locked"].Value != null
                  && usersGrid.Rows[e.RowIndex].Cells["Locked"].Value.Equals(false)) {
                    usersGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }



        private void customerDataGrid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e) {

            if (customerDataGrid.Rows[e.RowIndex].Cells["DeleteAlias"].Value != null
                   && customerDataGrid.Rows[e.RowIndex].Cells["DeleteAlias"].Value.Equals(true)) {
                customerDataGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
            } else {
                if (customerDataGrid.Rows[e.RowIndex].Cells["customerEnabled"].Value != null
                  && customerDataGrid.Rows[e.RowIndex].Cells["customerEnabled"].Value.Equals(false)) {
                    customerDataGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                } else if (customerDataGrid.Rows[e.RowIndex].Cells["customerEnabled"].Value != null
                      && customerDataGrid.Rows[e.RowIndex].Cells["customerEnabled"].Value.Equals(true)) {
                    customerDataGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }



        private void updateButton_Click(object sender, EventArgs e) {
            //Update button update dataset after insertion,upadtion or deletion
            DialogResult dr = MessageBox.Show("Are you sure to save Changes", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes) {

                int aliasDataIDInt;
                string aliasDataID;
                string aliasData;
                string entryEnabled;
                string deleteEntry;
                bool entryEnabledBool;
                bool deleteEntryBool;
                bool exceptionAdded = false;

                
                DataTable dt = (DataTable)(fullExceptionsGrid.DataSource);
                foreach (DataRow datarow in dt.GetChanges().Rows) {
                    aliasDataIDInt = datarow.Field<int>("AliasDataID");
                    aliasDataID = Convert.ToString(aliasDataIDInt);
                    aliasData = datarow.Field<string>("Data");
                    if (!datarow.IsNull("EntryEnabled")) {
                        entryEnabledBool = datarow.Field<bool>("EntryEnabled");
                    } else {
                        entryEnabledBool = false;
                    }
                    entryEnabled = Convert.ToString(Convert.ToInt32(entryEnabledBool));
                    if (!datarow.IsNull("DeleteEntry")) {
                        deleteEntryBool = datarow.Field<bool>("DeleteEntry");
                    } else {
                        deleteEntryBool = false;
                    }
                    deleteEntry = Convert.ToString(Convert.ToInt32(deleteEntryBool));

                    if ((aliasDataID == "") || (aliasData == "") || (entryEnabled == "")) {
                        if ((aliasDataID != "") || (aliasData != "") || (entryEnabled != "")) {
                            MessageBox.Show("Cannot have an empty field, all Fields need to be filled in!");
                        }
                    } else {
                        Program.utilities.UpdateAliasData(aliasDataID, aliasData, entryEnabled, deleteEntry);
                        if (exceptionAdded == false) {
                            exceptionAdded = true;
                        }
                    }

                }

                DataSet fullExceptionsDS = new DataSet();
                Program.utilities.getFullExceptionsDS(fullExceptionsDS);
                fullExceptionsGrid.DataSource = fullExceptionsDS.Tables[0];
                fullExceptionsGrid.Update();
                fullExceptionsGrid.Refresh();
            }
        }




        private void DataTypeUpdateButton_Click(object sender, EventArgs e) {
            //Update button update dataset after insertion,upadtion or deletion
            DialogResult dr = MessageBox.Show("Are you sure to save Changes", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes) {
                BindingSource bs = new BindingSource();
                bs.DataSource = dataTypesGrid.DataSource;
                dataTypesGrid.DataSource = bs;
                bs.ResetBindings(true);
                dataTypesGrid.Update();
                dataTypesGrid.Refresh();
            }
        }




        private void UserUpdateButton_Click(object sender, EventArgs e) {
            string Username;
            string FirstName;
            string Surname;
            string userID;
            int userIDInt;
            string roleID;
            int roleIDInt;
            string userRoleID;
            int userRoleIDInt;
            string deleteEntry = "0";
            bool deleteEntryBool;
            string locked = "0";
            bool lockedBool;


            DialogResult dr = MessageBox.Show("Are you sure to save Changes", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes) {

                DataTable dt = (DataTable)(usersGrid.DataSource);
                foreach (DataRow datarow in dt.GetChanges().Rows) {
                    userIDInt = datarow.Field<int>("id");
                    userID = Convert.ToString(userIDInt);
                    Username = datarow.Field<string>("Username");
                    FirstName = datarow.Field<string>("FirstName");
                    Surname = datarow.Field<string>("Surname");
                    roleIDInt = datarow.Field<int>("roleID");
                    roleID = Convert.ToString(roleIDInt);
                    userRoleIDInt = datarow.Field<int>("UserRoleID");
                    userRoleID = Convert.ToString(userRoleIDInt);
                    lockedBool = datarow.Field<bool>("Locked");
                    locked = Convert.ToString(lockedBool);


                    if (!datarow.IsNull("Locked")) {
                        lockedBool = datarow.Field<bool>("Locked");
                    } else {
                        lockedBool = false;
                    }
                    locked = Convert.ToString(lockedBool);

                    if (!datarow.IsNull("DeleteEntry")) {
                        deleteEntryBool = datarow.Field<bool>("DeleteEntry");
                    } else {
                        deleteEntryBool = false;
                    }
                    deleteEntry = Convert.ToString(deleteEntryBool);

                    if ((userID == "") || (Username == "") || (FirstName == "") || (Surname == "") || (roleID == "") || (userRoleID == "")) {
                        if ((userID != "") || (Username != "") || (FirstName != "") || (Surname != "") || (roleID != "") || (userRoleID != "")) {
                            MessageBox.Show("Cannot have an empty field, all Fields need to be filled in!");
                        }
                    } else {
                        Program.utilities.UpdateUserData(userID,Username,FirstName,Surname,roleID,userRoleID, locked, deleteEntry);
                        //if (exceptionAdded == false) {
                        //    exceptionAdded = true;
                        //}
                    }

                }

                LocalUsersDS = new DataSet();
                Program.utilities.getLocalUsersDS(LocalUsersDS);
                usersGrid.DataSource = LocalUsersDS.Tables[0];
                usersGrid.Update();
                usersGrid.Refresh();
            }
        }




        private void setUserPasswordButton_Click(object sender, EventArgs e) {
            Int32 selectedCellCount = usersGrid.GetCellCount(DataGridViewElementStates.Selected);
            List<int> selectedRows = new List<int>();

            string Username;
            string FullName;
            int rowIndex;
            if (selectedCellCount > 0) {
                
                for (int i = 0; i < selectedCellCount; i++) {
                    rowIndex = Convert.ToInt32(usersGrid.SelectedCells[i].RowIndex.ToString());
                    
                    if (!(selectedRows.Contains(rowIndex))) {
                        selectedRows.Add(rowIndex);
                    }
                }

                // now parse through distinct rows and set password for users
                foreach (int myRowIndex in selectedRows) {
                    if (usersGrid.Rows[myRowIndex].Cells["Username"].Value != null) {
                        Username = usersGrid.Rows[myRowIndex].Cells["Username"].Value.ToString();
                        FullName = usersGrid.Rows[myRowIndex].Cells["FirstName"].Value.ToString() + " " + usersGrid.Rows[myRowIndex].Cells["Surname"].Value.ToString();

                        Form pwForm = new SetUserPasswordForm(Username, FullName);
                        pwForm.Show();

                    }
                }

            }
        }



        private void addUserButton_Click(object sender, EventArgs e) {
            Form addUser = new AddUserForm(usersGrid);
            addUser.ShowDialog();

            LocalUsersDS = new DataSet();
            Program.utilities.getLocalUsersDS(LocalUsersDS);
            usersGrid.DataSource = LocalUsersDS.Tables[0];
            usersGrid.Update();
            usersGrid.Refresh();
        }



        private void createFullExceptionListToolStripMenuItem_Click(object sender, EventArgs e) {
            string line;
            DataSet myDS = new DataSet();

            string desktopFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string defaultFileName = "ProxyExceptions_All_Carriers-"+ DateTime.Now.ToString("yyyy-MM-dd")+".txt";

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Exception List|*.txt";
            saveFileDialog1.Title = "Save Full Exception List";
            saveFileDialog1.InitialDirectory = desktopFilePath;
            saveFileDialog1.FileName = defaultFileName;
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "") {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                
                switch (saveFileDialog1.FilterIndex) {
                    case 1:
                        Program.utilities.CreateFullExceptionList(myDS);
                        var sw = new StreamWriter(fs);

                        foreach (DataRow dr in myDS.Tables[0].Rows) {
                            line = dr["Type"].ToString() + "," + dr["Data"].ToString();
                            sw.WriteLine(line);
                        }
                        sw.Flush();
                        sw.Close();
                        sw.Dispose();
                        break;
                }

                fs.Close();
                fs.Dispose();
            }
            myDS.Dispose();
        }




        private void logoutToolStripMenuItem_Click(object sender, EventArgs e) {
            loginForm TheLoginForm = new loginForm();
            TheLoginForm.Show();
            userInfo.FirstName = "";
            userInfo.LastName = "";
            userInfo.Username = "";
            userInfo.PermissionLevelText = "";
            userInfo.PermissionLevelID = 0;
            userInfo.locked = true;
            this.Close();
        }




        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            userInfo.FirstName = "";
            userInfo.LastName = "";
            userInfo.Username = "";
            userInfo.PermissionLevelText = "";
            userInfo.PermissionLevelID = 0;
            userInfo.locked = true;
            System.Windows.Forms.Application.Exit();
        }



        private void dbSaveButton_Click(object sender, EventArgs e) {
            if (dbPasswordText.Text == dbVerifyPasswordText.Text) {
                Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbHost = dbHostText.Text;
                Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbPort = dbPortText.Text;
                Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbUser = dbUsernameText.Text;
                Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbName = dbNameText.Text;
                Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbPassword = Program.utilities.EncryptString(dbPasswordText.Text);
                
                Program.myAppSettingsXML.XmlDataWriter(Program.myAppSettings, Program.configFilename);
                MessageBox.Show("Settings saved.");
            } else {
                MessageBox.Show("Passwords don't match!");
            }
        }



        private void dbResetButton_Click(object sender, EventArgs e) {
            UpdateDBTab();
        }



        private void adSaveButton_Click(object sender, EventArgs e) {
            DataRowView drv;
            string tempValue;

            if (adBindPasswordText.Text == adBindVerifyPasswordText.Text) {
                Program.myAppSettings.myADSettings.ADDomain = adDomainText.Text;
                Program.myAppSettings.myADSettings.ADServerPrimary = adServerPrimaryText.Text;
                Program.myAppSettings.myADSettings.ADServerSecondary = adServerSecondaryText.Text;
                Program.myAppSettings.myADSettings.ADPort = adPortText.Text;
                Program.myAppSettings.myADSettings.BindUsername = adBindUsernameText.Text;
                Program.myAppSettings.myADSettings.BindPassword = Program.utilities.EncryptString(adBindPasswordText.Text);
                Program.myAppSettings.myADSettings.ADUserBase = adBindUserBaseText.Text;
                
                drv = adAdminGroupDN.SelectedItem as DataRowView;
                tempValue = drv.Row ["GroupDN"].ToString();
                Program.utilities.ADAdministratorGroupDN = tempValue;
                Program.utilities.updateGroupDNToDB("ADAdministratorGroupDN", Program.utilities.ADAdministratorGroupDN);

                drv = adReadOnlyGroupDN.SelectedItem as DataRowView;
                tempValue = drv.Row["GroupDN"].ToString();
                Program.utilities.ADReadOnlyGroupDN = tempValue;
                Program.utilities.updateGroupDNToDB("ADReadOnlyGroupDN", Program.utilities.ADReadOnlyGroupDN);

                Program.myAppSettingsXML.XmlDataWriter(Program.myAppSettings, Program.configFilename);
                MessageBox.Show("Settings saved.");
            } else {
                MessageBox.Show("Passwords don't match!");
            }
        }



        private void adResetButton_Click(object sender, EventArgs e) {
            UpdateADTab();
        }



        private void customerUpdateButton_Click(object sender, EventArgs e) {
            string AliasName;
            bool entryEnabledBool;
            string entryEnabled;
            bool deleteAliasBool;
            string deleteAlias;
            int AliasNameIDInt; 
            string AliasNameID;


            //Update button update dataset after insertion,upadtion or deletion
            DialogResult dr = MessageBox.Show("Are you sure to save Changes", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes) {

                DataTable dt = (DataTable)(customerDataGrid.DataSource);
                foreach (DataRow datarow in dt.GetChanges().Rows) {
                    AliasName = datarow.Field<string>("AliasName");
                    if (datarow.IsNull("AliasNameID")) {
                        AliasNameIDInt = -1;
                    } else {
                        AliasNameIDInt = datarow.Field<int>("AliasNameID");
                    }
                    AliasNameID = Convert.ToString(AliasNameIDInt);

                    
                    if (!datarow.IsNull("customerEnabled")) {
                        entryEnabledBool = datarow.Field<bool>("customerEnabled");
                    } else {
                        entryEnabledBool = true;
                    }
                    entryEnabled = Convert.ToString(Convert.ToInt32(entryEnabledBool));

                    if (!datarow.IsNull("DeleteAlias")) {
                        deleteAliasBool = datarow.Field<bool>("DeleteAlias");
                    } else {
                        deleteAliasBool = false;
                    }
                    deleteAlias = Convert.ToString(Convert.ToInt32(deleteAliasBool));

                    if ((AliasName == "")) {
                        if ((AliasName != "")) {
                            MessageBox.Show("Cannot have an empty field, all Fields need to be filled in!");
                        }
                    } else {
                        Program.utilities.UpdateAliasNameData(AliasNameID, AliasName, entryEnabled, deleteAlias);
                        //if (exceptionAdded == false) {
                        //    exceptionAdded = true;
                        //}
                    }

                }

                AliasNamesDS = new DataSet();
                Program.utilities.getAliasNamesDS(AliasNamesDS);
                customerDataGrid.DataSource = AliasNamesDS.Tables[0];
                customerDataGrid.Update();
                customerDataGrid.Refresh();

                DataSet fullExceptionsDS = new DataSet();
                Program.utilities.getFullExceptionsDS(fullExceptionsDS);
                fullExceptionsGrid.DataSource = fullExceptionsDS.Tables[0];
                fullExceptionsGrid.Update();
                fullExceptionsGrid.Refresh();
            }
        }




        private void exceptionsAddExceptionsButton_Click(object sender, EventArgs e) {
            Form addExceptions = new AddExceptionsForm();
            addExceptions.ShowDialog();

            //Program.utilities.buildFullExceptionsDataGridView(this.fullExceptionsGrid);
            DataSet fullExceptionsDS = new DataSet();
            Program.utilities.getFullExceptionsDS(fullExceptionsDS);
            fullExceptionsGrid.DataSource = fullExceptionsDS.Tables[0];
            fullExceptionsGrid.Update();
            fullExceptionsGrid.Refresh();
        }




        private void createCSVExceptionListToolStripMenuItem_Click(object sender, EventArgs e) {
            string line;
            DataSet myDS = new DataSet();

            string desktopFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string defaultFileName = "ProxyExceptions_CSV-" + DateTime.Now.ToString("yyyy-MM-dd") + ".csv";

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "CSV Exception List|*.txt";
            saveFileDialog1.Title = "Save Full Exception List as CSV";
            saveFileDialog1.InitialDirectory = desktopFilePath;
            saveFileDialog1.FileName = defaultFileName;
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "") {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();

                switch (saveFileDialog1.FilterIndex) {
                    case 1:
                        Program.utilities.CreateFullExceptionListCSV(myDS);
                        var sw = new StreamWriter(fs);

                        line = "\"Alias Name\",\"Alias Type\",\"Data\",\"enabled\"";
                        sw.WriteLine(line);

                        foreach (DataRow dr in myDS.Tables[0].Rows) {
                            line = "\"" + dr["AliasName"].ToString() + "\",\"" + dr["AliasType"].ToString() + "\",\"";
                            line += dr["Data"].ToString()+"\","+dr["enabled"].ToString();
                            sw.WriteLine(line);
                        }
                        sw.Flush();
                        sw.Close();
                        sw.Dispose();
                        break;
                }

                fs.Close();
                fs.Dispose();
            }
            myDS.Dispose();
        }




        private void adGetADGroups_Click(object sender, EventArgs e) {

            ADSettings tempADSettings;
            tempADSettings = new ADSettings();

            tempADSettings.ADDomain = adDomainText.Text;
            tempADSettings.ADServerPrimary = adServerPrimaryText.Text;
            tempADSettings.ADServerSecondary = adServerSecondaryText.Text;
            tempADSettings.ADPort = adPortText.Text;
            tempADSettings.BindUsername = adBindUsernameText.Text;
            tempADSettings.BindPassword = adBindPasswordText.Text;
            tempADSettings.ADUserBase = adBindUserBaseText.Text;

            
            Program.utilities.setSkipAD(0);
            Console.WriteLine("Calling GetADGroups for Admins!");
            Program.myADAdminGroups = Program.utilities.getADGroups(tempADSettings);
            Program.myADAdminGroupsBS = new BindingSource();
            Program.myADAdminGroupsBS.DataSource = Program.myADAdminGroups.Tables["AD Groups"];
            Console.WriteLine("Calling GetADGroups for Read Only!"); 
            Program.myADReadOnlyGroups = Program.utilities.getADGroups(tempADSettings);
            Program.myADReadOnlyGroupsBS = new BindingSource();
            Program.myADReadOnlyGroupsBS.DataSource = Program.myADReadOnlyGroups.Tables["AD Groups"];

            this.adAdminGroupDN.DataSource = Program.myADAdminGroupsBS;
            this.adAdminGroupDN.DisplayMember = Program.myADAdminGroups.Tables["AD Groups"].Columns["Name"].ToString();
            this.adAdminGroupDN.ValueMember = Program.myADAdminGroups.Tables["AD Groups"].Columns["GroupDN"].ToString();
            Program.utilities.SelectItemByValue(adAdminGroupDN, "GroupDN", Program.utilities.DecryptString(Program.utilities.ADAdministratorGroupDN, 846));
            this.adAdminGroupDN.Refresh();

            this.adReadOnlyGroupDN.DataSource = Program.myADReadOnlyGroupsBS;
            this.adReadOnlyGroupDN.DisplayMember = Program.myADReadOnlyGroups.Tables["AD Groups"].Columns["Name"].ToString();
            this.adReadOnlyGroupDN.ValueMember = Program.myADReadOnlyGroups.Tables["AD Groups"].Columns["GroupDN"].ToString();
            Program.utilities.SelectItemByValue(adReadOnlyGroupDN, "GroupDN", Program.utilities.DecryptString(Program.utilities.ADReadOnlyGroupDN, 657));
            this.adReadOnlyGroupDN.Refresh();

        }




        private void aliasNameAddButton_Click(object sender, EventArgs e) {
            Form addAliasName = new addAliasNameForm();
            addAliasName.ShowDialog();
            DataSet AliasNameDS;

            AliasNameDS = new DataSet();
            Program.utilities.getAliasNamesDS(AliasNameDS);
            customerDataGrid.DataSource = AliasNameDS.Tables[0];
            customerDataGrid.Update();
            customerDataGrid.Refresh();

            DataSet fullExceptionsDS = new DataSet();
            Program.utilities.getFullExceptionsDS(fullExceptionsDS);
            fullExceptionsGrid.DataSource = fullExceptionsDS.Tables[0];
            fullExceptionsGrid.Update();
            fullExceptionsGrid.Refresh();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            Form aboutForm = new aboutForm();
            aboutForm.ShowDialog();
        }
    }
}
