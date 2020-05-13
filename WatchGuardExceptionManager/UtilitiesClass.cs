using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Data;
using System.Windows.Forms;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Collections.Specialized;


namespace WatchGuardExceptionManager {
    class UtilitiesClass {


        private string myConnectionString = "";
        //public static int skipAD = 0;

        private static int _skipAD;
        public static int skipAD {
            get { return _skipAD; }
            set { _skipAD = value; }
        }


        public void setSkipAD (int value) {
            _skipAD = value;
        }

        public string CreateConnectionString() {
            string dbHost;
            string dbName;
            string dbUser;
            string dbPort;
            string dbPassword;
            string encryptedDbPassword;

            dbHost = Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbHost;
            dbName = Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbName;
            dbUser = Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbUser;
            dbPort = Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbPort;
            encryptedDbPassword = Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbPassword;
            dbPassword = Program.utilities.DecryptString(encryptedDbPassword);
            myConnectionString = "server=" + dbHost + ";Port=" + dbPort + ";database=" + dbName + ";uid=" + dbUser + ";pwd=" + dbPassword;

            return (myConnectionString);
        }




        public void SelectItemByValue(ComboBox cbo, string fieldName, string value) {
            DataRowView drv;
            for (int i = 0; i < cbo.Items.Count; i++) {
                drv = cbo.Items[i] as DataRowView;
                if (drv != null) {
                    if (drv.Row[fieldName].ToString() == value) {
                        cbo.SelectedIndex = i;
                        break;
                    }
                }
            }
        }




        public DataSet getADGroups(ADSettings myADSettings) {

            // first create the Data Table to store the results
            DataTable groupsTable = new DataTable("AD Groups");
            DataColumn dtColumn;
            DataRow myDataRow;
            DataSet groupsSet;

            // Create Name column  
            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(string);
            dtColumn.ColumnName = "Name";
            dtColumn.Caption = "Group Name";
            dtColumn.ReadOnly = false;
            dtColumn.Unique = true;
            // Add column to the DataColumnCollection.  
            groupsTable.Columns.Add(dtColumn);

            // Create Name column  
            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(string);
            dtColumn.ColumnName = "GroupDN";
            dtColumn.Caption = "Group DN";
            dtColumn.ReadOnly = false;
            dtColumn.Unique = true;
            // Add column to the DataColumnCollection.  
            groupsTable.Columns.Add(dtColumn);

            // Make id column the primary key column.    
            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = groupsTable.Columns["Name"];
            groupsTable.PrimaryKey = PrimaryKeyColumns;

            groupsSet = new DataSet();

            // Add custTable to the DataSet.    
            groupsSet.Tables.Add(groupsTable);
            groupsTable.DefaultView.Sort = "Name";

            string adDomain = myADSettings.ADDomain;
            string adUserBase = myADSettings.ADUserBase;
            string bindUsername = myADSettings.BindUsername;
            string encryptedBindPassword = myADSettings.BindPassword;
            string bindPassword = DecryptString(encryptedBindPassword);
            
            if (skipAD == 0) {
                try {
                    PrincipalContext ctx = new PrincipalContext(
                                              ContextType.Domain,
                                             adDomain,
                                             adUserBase,
                                             bindUsername,
                                             bindPassword);

                    // define a "query-by-example" principal - here, we search for a GroupPrincipal 
                    GroupPrincipal qbeGroup = new GroupPrincipal(ctx);

                    // create your principal searcher passing in the QBE principal    
                    PrincipalSearcher srch = new PrincipalSearcher(qbeGroup);
                    myDataRow = groupsTable.NewRow();
                    myDataRow["Name"] = " <SELECT> ";
                    myDataRow["GroupDN"] = "";
                    groupsTable.Rows.Add(myDataRow);

                    // find all matches
                    foreach (var found in srch.FindAll()) {
                        // do whatever here - "found" is of type "Principal" - it could be user, group, computer.....    
                        myDataRow = groupsTable.NewRow();
                        myDataRow["Name"] = found.Name;
                        myDataRow["GroupDN"] = found.DistinguishedName;
                        groupsTable.Rows.Add(myDataRow);
                        //Console.WriteLine("Name: " + found.Name + ",  GroupDN: " + found.DistinguishedName);

                    }
                } catch (Exception theException) {
                    MessageBox.Show("Error connecting to Active Directory!\n" + "Error: " + theException.Message);
                    skipAD = 1;
                }
            }

            return (groupsSet);
        }


        public string getGroupDNFromDB(string groupDNName) {
            string result = "";

            string sql;
            MySqlCommand cmd;
            try { 
                MySql.Data.MySqlClient.MySqlConnection dbConnection;
                myConnectionString = CreateConnectionString();

                dbConnection = new MySql.Data.MySqlClient.MySqlConnection {
                    ConnectionString = myConnectionString
                };
                dbConnection.Open();
                sql = "SELECT groupDN ";
                sql += "FROM adGroups ";
                sql += "WHERE groupType = @groupType ";
                cmd = new MySqlCommand(sql, dbConnection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@groupType", groupDNName);

                MySqlDataReader reader = cmd.ExecuteReader();

                try {
                    if (reader.Read()) {

                        // populate the user information structure with the user info
                        result = reader["groupDN"].ToString();
                        reader.Close();
                    }
                } catch (Exception ex2) {
                    reader.Close();
                    cmd.Dispose();
                    MessageBox.Show("Error: " + ex2);
                }
                cmd.Dispose();
            } catch (MySql.Data.MySqlClient.MySqlException ex) {
                Console.WriteLine("MySQL Connection error: " + ex.Message);
                MessageBox.Show("MySQL Connection error: " + ex.Message);
            }

            return (result);
        }



        public void updateGroupDNToDB(string groupDNName, string groupDNValue) {
            string sql;
            MySqlCommand cmd;
            try {
                MySql.Data.MySqlClient.MySqlConnection dbConnection;
                myConnectionString = CreateConnectionString();

                dbConnection = new MySql.Data.MySqlClient.MySqlConnection {
                    ConnectionString = myConnectionString
                };
                dbConnection.Open();
                sql = "INSERT INTO adGroups (groupType,groupDN) ";
                sql += " VALUES (@groupType, @groupDN) ";
                sql += "ON DUPLICATE KEY UPDATE groupDN=@groupDN ";
                cmd = new MySqlCommand(sql, dbConnection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@groupType", groupDNName);
                cmd.Parameters.AddWithValue("@groupDN", groupDNValue);

                try { 
                    MySqlDataReader reader = cmd.ExecuteReader();

                    reader.Close();
                    
                } catch (Exception ex2) {
                    cmd.Dispose();
                    MessageBox.Show("Error: " + ex2);
                }
                cmd.Dispose();
            } catch (MySql.Data.MySqlClient.MySqlException ex) {
                Console.WriteLine("MySQL Connection error: " + ex.Message);
                MessageBox.Show("MySQL Connection error: " + ex.Message);
            }

            return;
        }


        public void getFullExceptionsDS(DataSet myDS) {
            //DataSet myDS = new DataSet();
            string sql;
            MySqlCommand cmd;

            MySql.Data.MySqlClient.MySqlConnection dbConnection;
            myConnectionString = CreateConnectionString();

            dbConnection = new MySql.Data.MySqlClient.MySqlConnection {
                ConnectionString = myConnectionString
            };
            dbConnection.Open();
            sql = "SELECT an.Name AS AliasName, at.Type AS AliasType, ";
            sql += "ad.id as AliasDataID, ad.Data AS Data, ad.enabled AS EntryEnabled, ad.DeleteEntry as DeleteEntry, ";
            sql += "an.enabled AS CustomerEnabled ";
            sql += "FROM ((AliasData ad JOIN AliasName an ON ((ad.AliasName = an.id))) ";
            sql += "       JOIN AliasType at ON ((ad.AliasType = at.id))) ";
            sql += "ORDER BY an.Name,ad.Data ";
            cmd = new MySqlCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;

            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            sda.FillSchema(myDS, SchemaType.Source);
            myDS.Tables[0].Columns["EntryEnabled"].DataType = typeof(bool);
            myDS.Tables[0].Columns["DeleteEntry"].DataType = typeof(bool);
            sda.Fill(myDS);
            
            return;
        }



        public void getAliasNamesDS(DataSet myDS) {
            string sql;
            MySqlCommand cmd;

            MySql.Data.MySqlClient.MySqlConnection dbConnection;
            myConnectionString = CreateConnectionString();

            dbConnection = new MySql.Data.MySqlClient.MySqlConnection {
                ConnectionString = myConnectionString
            };
            dbConnection.Open();
            sql = "SELECT id as AliasNameID, Name AS AliasName, enabled AS customerEnabled, 0 AS DeleteAlias ";
            sql += "FROM AliasName ";
            sql += "ORDER BY AliasName ";
            cmd = new MySqlCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;

            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            sda.FillSchema(myDS, SchemaType.Source);
            myDS.Tables[0].Columns["AliasNameID"].DataType = typeof(int);
            myDS.Tables[0].Columns["AliasName"].DataType = typeof(string);
            myDS.Tables[0].Columns["customerEnabled"].DataType = typeof(bool);
            myDS.Tables[0].Columns["DeleteAlias"].DataType = typeof(bool);
            sda.Fill(myDS);

            return;
        }


        public void getAliasTypesDS(DataSet myDS) {
            string sql;
            MySqlCommand cmd;

            MySql.Data.MySqlClient.MySqlConnection dbConnection;
            myConnectionString = CreateConnectionString();

            dbConnection = new MySql.Data.MySqlClient.MySqlConnection {
                ConnectionString = myConnectionString
            };
            dbConnection.Open();
            sql = "SELECT id, Type as AliasType, wgImportType ";
            sql += "FROM AliasType ";
            sql += "ORDER BY AliasType ";
            cmd = new MySqlCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;

            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            sda.Fill(myDS);

            return;
        }



        public void buildCustomerDataGridView(UserInfoStruct myUserInfo, DataGridView theView) {
            theView.AutoGenerateColumns = false;
            DataGridViewTextBoxColumn textColumn;
            DataGridViewCheckBoxColumn checkboxColumn;

            //Add Columns
            textColumn = new DataGridViewTextBoxColumn();
            {
                textColumn.HeaderText = "Alias Name";
                textColumn.Name = "AliasName";
                textColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                textColumn.DataPropertyName = "AliasName";
                textColumn.ReadOnly = false;
            };
            theView.Columns.Add(textColumn);

            checkboxColumn = new DataGridViewCheckBoxColumn();
            {
                checkboxColumn.HeaderText = "Enabled";
                checkboxColumn.Name = "customerEnabled";
                checkboxColumn.DataPropertyName = "customerEnabled";
                checkboxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                checkboxColumn.FlatStyle = FlatStyle.Standard;
                checkboxColumn.FalseValue = 0;
                checkboxColumn.TrueValue = 1;
                checkboxColumn.IndeterminateValue = 1;
                checkboxColumn.ThreeState = false;
              
            }
            theView.Columns.Add(checkboxColumn);

            checkboxColumn = new DataGridViewCheckBoxColumn();
            {
                checkboxColumn.HeaderText = "Delete";
                checkboxColumn.Name = "DeleteAlias";
                checkboxColumn.DataPropertyName = "DeleteAlias";
                checkboxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                checkboxColumn.FlatStyle = FlatStyle.Standard;
                checkboxColumn.FalseValue = 0;
                checkboxColumn.TrueValue = 1;
                checkboxColumn.IndeterminateValue = 0;
                checkboxColumn.ThreeState = false;
            }
            theView.Columns.Add(checkboxColumn);

            textColumn = new DataGridViewTextBoxColumn();
            {
                textColumn.HeaderText = "AliasNameID";
                textColumn.Name = "AliasNameID";
                textColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                textColumn.DataPropertyName = "AliasNameID";
                textColumn.ReadOnly = true;
                textColumn.Visible = false;
            };
            theView.Columns.Add(textColumn);

            theView.AllowUserToAddRows = false;

            if (myUserInfo.PermissionLevelText.Equals("Read-Only User")) {
                theView.ReadOnly = true;
            }

            }




        public void buildAliasTypeDataGridView(UserInfoStruct myUserInfo, DataGridView theView) {
            theView.AutoGenerateColumns = false;

            //Add Columns
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            {
                column.HeaderText = "Alias Type";
                column.Name = "AliasType";
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                column.DataPropertyName = "AliasType";
                column.ReadOnly = false;
            };
            theView.Columns.Add(column);

            DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();
            {
                column2.HeaderText = "WG Type";
                column2.Name = "wgImportType";
                column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                column2.DataPropertyName = "wgImportType";
                column2.ReadOnly = false;
            };
            theView.Columns.Add(column2);


            DataGridViewTextBoxColumn column6 = new DataGridViewTextBoxColumn();
            {
                column6.HeaderText = "id";
                column6.Name = "id";
                column6.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                column6.DataPropertyName = "id";
                column6.ReadOnly = true;
                column6.Visible = false;
            };
            theView.Columns.Add(column6);
            
            if (myUserInfo.PermissionLevelText.Equals("Read-Only User")) {
                theView.ReadOnly = true;
            }

        }




        public void buildFullExceptionsDataGridView(UserInfoStruct myUserInfo, DataGridView theGrid) {
            theGrid.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn textColumn;
            DataGridViewCheckBoxColumn checkBoxColumn;

            //Add Columns
            textColumn = new DataGridViewTextBoxColumn();
            {
                textColumn.HeaderText = "Alias Name";
                textColumn.Name = "AliasName";
                textColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                textColumn.DataPropertyName = "AliasName";
                textColumn.ReadOnly = true;
            };
            theGrid.Columns.Add(textColumn);

            textColumn = new DataGridViewTextBoxColumn();
            {
                textColumn.HeaderText = "Alias Type";
                textColumn.Name = "AliasType";
                textColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                textColumn.DataPropertyName = "AliasType";
                textColumn.ReadOnly = true;
            };
            theGrid.Columns.Add(textColumn);

            textColumn = new DataGridViewTextBoxColumn();
            {
                textColumn.HeaderText = "Data";
                textColumn.Name = "Data";
                textColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                textColumn.DataPropertyName = "Data";
            };
            theGrid.Columns.Add(textColumn);

            checkBoxColumn = new DataGridViewCheckBoxColumn();
            {
                checkBoxColumn.HeaderText = "Enabled";
                checkBoxColumn.Name = "EntryEnabled";
                checkBoxColumn.DataPropertyName = "EntryEnabled";
                checkBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                checkBoxColumn.FlatStyle = FlatStyle.Standard;
                checkBoxColumn.FalseValue = 0;
                checkBoxColumn.TrueValue = 1;
                checkBoxColumn.IndeterminateValue = 1;
                checkBoxColumn.ThreeState = false;
            }
            theGrid.Columns.Add(checkBoxColumn);

            checkBoxColumn = new DataGridViewCheckBoxColumn();
            {
                checkBoxColumn.HeaderText = "Delete";
                checkBoxColumn.Name = "DeleteEntry";
                checkBoxColumn.DataPropertyName = "DeleteEntry";
                checkBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                checkBoxColumn.FlatStyle = FlatStyle.Standard;
                checkBoxColumn.FalseValue = 0;
                checkBoxColumn.TrueValue = 1;
                checkBoxColumn.IndeterminateValue = 0;
                checkBoxColumn.ThreeState = false;
            }
            theGrid.Columns.Add(checkBoxColumn);

            textColumn = new DataGridViewTextBoxColumn();
            {
                textColumn.HeaderText = "AliasDataID";
                textColumn.Name = "AliasDataID";
                textColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                textColumn.DataPropertyName = "AliasDataID";
                textColumn.ReadOnly = true;
                textColumn.Visible = false;
            };
            theGrid.Columns.Add(textColumn);

            checkBoxColumn = new DataGridViewCheckBoxColumn();
            {
                checkBoxColumn.HeaderText = "CustomerEnabled";
                checkBoxColumn.Name = "CustomerEnabled";
                checkBoxColumn.DataPropertyName = "CustomerEnabled";
                checkBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                checkBoxColumn.FlatStyle = FlatStyle.Standard;
                checkBoxColumn.FalseValue = 0;
                checkBoxColumn.TrueValue = 1;
                checkBoxColumn.IndeterminateValue = 1;
                checkBoxColumn.ThreeState = false;
                checkBoxColumn.Visible = false;
            }
            theGrid.Columns.Add(checkBoxColumn);

            theGrid.AllowUserToAddRows = false;

            if (myUserInfo.PermissionLevelText.Equals("Read-Only User")) {
                theGrid.ReadOnly = true;
            }

        }




        public void buildAddExceptionsDataGridView(DataGridView theView) {
            theView.AutoGenerateColumns = false;
            DataGridViewTextBoxColumn textColumn;
            DataGridViewComboBoxColumn comboColumn;




            comboColumn = new DataGridViewComboBoxColumn();
            {
                DataSet AliasNamesDS = new DataSet();
                Program.utilities.getAliasNamesDS(AliasNamesDS);

                comboColumn.HeaderText = "Alias Name";
                comboColumn.Name = "addExceptionsAliasName";
                comboColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                comboColumn.DataSource = AliasNamesDS.Tables[0];
                comboColumn.DisplayMember = "AliasName";
                comboColumn.ValueMember = "AliasNameID";
                comboColumn.DataPropertyName = "AliasNameID";
                comboColumn.ReadOnly = false;
            };
            theView.Columns.Add(comboColumn);

            comboColumn = new DataGridViewComboBoxColumn();
            {
                DataSet AliasTypesDS = new DataSet();
                Program.utilities.getAliasTypesDS(AliasTypesDS);

                comboColumn.HeaderText = "Alias Type";
                comboColumn.Name = "addExceptionsAliasType";
                comboColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                comboColumn.DataSource = AliasTypesDS.Tables[0];
                comboColumn.DisplayMember = "AliasType";
                comboColumn.ValueMember = "id";
                comboColumn.DataPropertyName = "id";
                comboColumn.ReadOnly = false;
            };
            theView.Columns.Add(comboColumn);

            textColumn = new DataGridViewTextBoxColumn();
            {
                textColumn.HeaderText = "Data";
                textColumn.Name = "addExceptionsData";
                textColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                textColumn.DataPropertyName = "Username";
                textColumn.ReadOnly = false;
            };
            theView.Columns.Add(textColumn);

        }




        public void buildLocalUsersDataGridView(DataGridView theView, DataSet roleDS) {
            theView.AutoGenerateColumns = false;
            DataGridViewTextBoxColumn textColumn;
            DataGridViewCheckBoxColumn checkboxColumn;
            DataGridViewComboBoxColumn comboColumn;

            //Add Columns
            textColumn = new DataGridViewTextBoxColumn();
            {
                textColumn.HeaderText = "Username";
                textColumn.Name = "Username";
                textColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                textColumn.DataPropertyName = "Username";
                textColumn.ReadOnly = false;
            };
            theView.Columns.Add(textColumn);

            textColumn = new DataGridViewTextBoxColumn();
            {
                textColumn.HeaderText = "First Name";
                textColumn.Name = "FirstName";
                textColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                textColumn.DataPropertyName = "FirstName";
                textColumn.ReadOnly = false;
            };
            theView.Columns.Add(textColumn);

            textColumn = new DataGridViewTextBoxColumn();
            {
                textColumn.HeaderText = "Surname";
                textColumn.Name = "Surname";
                textColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                textColumn.DataPropertyName = "Surname";
                textColumn.ReadOnly = false;
            };
            theView.Columns.Add(textColumn);

            comboColumn = new DataGridViewComboBoxColumn();
            {
                comboColumn.HeaderText = "Role";
                comboColumn.Name = "roleName";
                comboColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                comboColumn.DataSource = roleDS.Tables[0];
                comboColumn.DisplayMember = "roleName";
                comboColumn.ValueMember = "id";
                comboColumn.DataPropertyName = "roleID";
                comboColumn.ReadOnly = false;
            };
            theView.Columns.Add(comboColumn);

            checkboxColumn = new DataGridViewCheckBoxColumn();
            {
                checkboxColumn.HeaderText = "Locked";
                checkboxColumn.Name = "Locked";
                checkboxColumn.DataPropertyName = "Locked";
                checkboxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                checkboxColumn.FlatStyle = FlatStyle.Standard;
                checkboxColumn.FalseValue = 0;
                checkboxColumn.TrueValue = 1;
                checkboxColumn.IndeterminateValue = 1;
                checkboxColumn.ThreeState = false;
            }
            theView.Columns.Add(checkboxColumn);

            checkboxColumn = new DataGridViewCheckBoxColumn();
            {
                checkboxColumn.HeaderText = "Delete";
                checkboxColumn.Name = "DeleteEntry";
                checkboxColumn.DataPropertyName = "DeleteEntry";
                checkboxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                checkboxColumn.FlatStyle = FlatStyle.Standard;
                checkboxColumn.FalseValue = 0;
                checkboxColumn.TrueValue = 1;
                checkboxColumn.IndeterminateValue = 0;
                checkboxColumn.ThreeState = false;
            }
            theView.Columns.Add(checkboxColumn);


            textColumn = new DataGridViewTextBoxColumn();
            {
                textColumn.HeaderText = "userID";
                textColumn.Name = "id";
                textColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                textColumn.DataPropertyName = "id";
                textColumn.ReadOnly = false;
                textColumn.Visible = false;
            };
            theView.Columns.Add(textColumn);

            textColumn = new DataGridViewTextBoxColumn();
            {
                textColumn.HeaderText = "roleID";
                textColumn.Name = "roleID";
                textColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                textColumn.DataPropertyName = "roleID";
                textColumn.ReadOnly = false;
                textColumn.Visible = false;
            };
            theView.Columns.Add(textColumn);

            textColumn = new DataGridViewTextBoxColumn();
            {
                textColumn.HeaderText = "UserRoleID";
                textColumn.Name = "UserRoleID";
                textColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                textColumn.DataPropertyName = "UserRoleID";
                textColumn.ReadOnly = false;
                textColumn.Visible = false;
            };
            theView.Columns.Add(textColumn);


        }



        public void getUserRoleDS(DataSet myDS) {
            string sql;
            MySqlCommand cmd;

            MySql.Data.MySqlClient.MySqlConnection dbConnection;
            myConnectionString = CreateConnectionString();

            dbConnection = new MySql.Data.MySqlClient.MySqlConnection {
                ConnectionString = myConnectionString
            };
            dbConnection.Open();
            sql = "SELECT id, roleName ";
            sql += "FROM roles ";
            sql += "ORDER BY roleName ";
            cmd = new MySqlCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;

            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            sda.Fill(myDS);

            return;
        }



        public void getLocalUsersDS(DataSet myDS) {
            string sql;
            MySqlCommand cmd;

            MySql.Data.MySqlClient.MySqlConnection dbConnection;
            myConnectionString = CreateConnectionString();

            dbConnection = new MySql.Data.MySqlClient.MySqlConnection {
                ConnectionString = myConnectionString
            };
            dbConnection.Open();
            sql  = "SELECT u.id as id, u.Username, u.FirstName, u.Surname, u.password, u.locked,  ";
            sql += "r.id as roleID, r.roleName as RoleName, ur.id as UserRoleID,0 as DeleteEntry ";
            sql += "FROM users u INNER JOIN userRoles ur ON (u.id = ur.userID) ";
            sql += "INNER JOIN roles r ON (ur.roleID = r.id) ";
            sql += "ORDER BY Username ";
            cmd = new MySqlCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;

            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            sda.FillSchema(myDS, SchemaType.Source);
            myDS.Tables[0].Columns["DeleteEntry"].DataType = typeof(bool);
            sda.Fill(myDS);


            return;
        }


        public bool GetLoginAD(ref UserInfoStruct loginInfo, string theUsername, string thePassword) {
            LdapConnection connection;
            bool loggedIn = false;
            string userLine;
            string[] adServers;
            
            string bindUsername = Program.myAppSettings.myADSettings.BindUsername;
            string encryptedBindPassword = Program.myAppSettings.myADSettings.BindPassword;
            string bindPassword = DecryptString(encryptedBindPassword);
            string adDomain = Program.myAppSettings.myADSettings.ADDomain;
            string adServerPrimary = Program.myAppSettings.myADSettings.ADServerPrimary;
            string adServerSecondary = Program.myAppSettings.myADSettings.ADServerSecondary;
            string adPort = Program.myAppSettings.myADSettings.ADPort;
            string adUserBase = Program.myAppSettings.myADSettings.ADUserBase;
            string encryptedADAdministratorGroupDN = ADAdministratorGroupDN;
            string adAdministratorGroupDN = Program.utilities.DecryptString(encryptedADAdministratorGroupDN,846); 
            string encryptedADReadOnlyGroupDN = ADReadOnlyGroupDN;
            string adReadOnlyGroupDN = Program.utilities.DecryptString(encryptedADReadOnlyGroupDN,657);
            if (adServerSecondary == "") { adServerSecondary = adServerPrimary; }
            adServers = new string[2] { adServerPrimary+":"+adPort, adServerSecondary+ ":" + adPort };


            if (skipAD == 1) {
                return (false);
            } else {

                try {
                    if (theUsername == "") {
                        MessageBox.Show("Username cannot be null");
                    } else if (thePassword == "") {
                        MessageBox.Show("Password cannot be null");
                    } else {

                        var credentials = new NetworkCredential(bindUsername,
                                                                bindPassword,
                                                                adDomain);
                        var serverId = new LdapDirectoryIdentifier(adServers, false, false);

                        try {
                            connection = new LdapConnection(serverId, credentials);
                            connection.Bind();
                        } catch (Exception e1) {
                            skipAD = 1;
                            MessageBox.Show("exception: " + e1.Message);
                        }

                        if (skipAD == 0) {
                            PrincipalContext ctx;
                            // after authenticate Loading user details to data table
                            try {
                                ctx = new PrincipalContext(
                                                 ContextType.Domain,
                                                 adDomain,
                                                 adUserBase,
                                                 bindUsername,
                                                 bindPassword);
                                bool isValid = ctx.ValidateCredentials(theUsername, thePassword);
                                
                                UserPrincipal user = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, theUsername);
                                if (user == null) {

                                } else {
                                    DirectoryEntry up_User = (DirectoryEntry)user.GetUnderlyingObject();
                                    DirectorySearcher deSearch = new DirectorySearcher(up_User);
                                    SearchResultCollection results = deSearch.FindAll();
                                    ResultPropertyCollection rpc = results[0].Properties;
                                    DataTable dt = new DataTable();
                                    DataRow toInsert = dt.NewRow();
                                    dt.Rows.InsertAt(toInsert, 0);

                                    string stringInfo = "";
                                    foreach (string rp in rpc.PropertyNames) {
                                        if (rpc[rp][0].ToString() != "System.Byte[]") {
                                            dt.Columns.Add(rp.ToString(), typeof(System.String));

                                            foreach (DataRow row in dt.Rows) {
                                                row[rp.ToString()] = rpc[rp][0].ToString();
                                                userLine = rp.ToString() + " - " + rpc[rp][0].ToString() + " \n";
                                                stringInfo += userLine;
                                                //Console.Write(userLine);
                                            }
                                            

                                        }
                                    }
                                    //MessageBox.Show(stringInfo);
                                    loggedIn = true;

                                    loginInfo.Username = rpc["samaccountname"][0].ToString();
                                    loginInfo.FirstName = rpc["givenname"][0].ToString();
                                    loginInfo.LastName = rpc["sn"][0].ToString();
                                    loginInfo.FullName = rpc["displayname"][0].ToString();
                                    loginInfo.PermissionLevelID = 0;
                                    loginInfo.PermissionLevelText = "";
                                    loginInfo.locked = false;
                                    loginInfo.loginError = "";
                                    loginInfo.authenticatedBy = "";

                                    if (isUserinGroup(adAdministratorGroupDN, loginInfo.Username)) {
                                        loginInfo.PermissionLevelText = "Administrator";
                                        loginInfo.PermissionLevelID = 1;
                                        loginInfo.authenticatedBy = "Active Directory";
                                        //MessageBox.Show("In admin group!");
                                    } else if (isUserinGroup(adReadOnlyGroupDN, loginInfo.Username)) {
                                        loginInfo.PermissionLevelText = "Read-Only User";
                                        loginInfo.PermissionLevelID = 2;
                                        loginInfo.authenticatedBy = "Active Directory";
                                        //MessageBox.Show("In Read Only Group!");
                                    } else {
                                        loginInfo.PermissionLevelID = 0;
                                        loginInfo.PermissionLevelText = "";
                                        loginInfo.loginError = "User not permitted!";
                                        loggedIn = false;
                                    }
                                    stringInfo = "";

                                }
                            } catch (Exception ex2) {
                                MessageBox.Show("ex2 " + ex2.Message);
                            }
                        }


                    }
                } catch (LdapException lexc) {
                    String error = lexc.ServerErrorMessage;
                    string pp = error.Substring(76, 4);
                    string ppp = pp.Trim();

                    loggedIn = false;
                    if ("52e" == ppp) {
                        loginInfo.loginError = "Invalid Username or password.";
                    }
                    if ("775​" == ppp) {
                        loginInfo.loginError = "User account locked.";
                    }
                    if ("525​" == ppp) {
                        loginInfo.loginError = "User not found.";
                    }
                    if ("530" == ppp) {
                        loginInfo.loginError = "Not permitted to logon at this time.";
                    }
                    if ("531" == ppp) {
                        loginInfo.loginError = "Not permitted to logon at this workstation.";
                    }
                    if ("532" == ppp) {
                        loginInfo.loginError = "Password expired.";
                    }
                    if ("533​" == ppp) {
                        loginInfo.loginError = "Account disabled.";
                    }

                } catch (Exception exc) {
                    loggedIn = false;
                    loginInfo.loginError = "Invalid Username or password.\n";
                    loginInfo.loginError += exc.Message;
                } finally {
                    //tbUID.Text = "";
                    //tbPass.Text = "";

                }
            }

            
            return (loggedIn);
        }


        private bool isUserinGroup(string groupNameDN, string strUser) {
            bool foundUser = false;
            string bindUsername = Program.myAppSettings.myADSettings.BindUsername;
            string encryptedBindPassword = Program.myAppSettings.myADSettings.BindPassword;
            string bindPassword = DecryptString(encryptedBindPassword);
            string adServerPrimary = Program.myAppSettings.myADSettings.ADServerPrimary;
            string adServerSecondary = Program.myAppSettings.myADSettings.ADServerSecondary;
            string adPort = Program.myAppSettings.myADSettings.ADPort;
            string adUserBase = Program.myAppSettings.myADSettings.ADUserBase;
            string[] adServers = new string[2] { adServerPrimary + ":" + adPort, adServerSecondary + ":" + adPort };

            PrincipalContext ctx = new PrincipalContext(ContextType.Domain,
                                            adServerPrimary+":"+adPort,
                                            adUserBase,
                                            bindUsername,
                                            bindPassword);

            GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx,
                                                               IdentityType.DistinguishedName,
                                                               groupNameDN);

            if (grp != null) {
                foreach (Principal p in grp.GetMembers(true)) {
                    if (p.SamAccountName == strUser) {
                        foundUser = true;
                        //Console.WriteLine("Found User in group!");
                    }
                }
                grp.Dispose();
            }

            ctx.Dispose();
            return (foundUser);
        }



        




        public bool ADMemberOf(ResultPropertyCollection rpc, string groupName) {

            string output = "";

            foreach (string myKey in rpc.PropertyNames) {
                string tab = "    ";
                output += myKey + " = ";
                foreach (Object myCollection in rpc[myKey]) {
                    output += (tab + myCollection)+"\n";
                }
            }
            MessageBox.Show(output);

            bool isMemberOf = rpc["memberof"].Contains(groupName);

            return (isMemberOf);
        }




        public void AddExceptionEntry(string aliasID, string aliasType, string aliasData) {
            string sql;
            MySqlCommand cmd;
            MySqlDataReader reader;


            MySql.Data.MySqlClient.MySqlConnection dbConnection;
            myConnectionString = CreateConnectionString();

            dbConnection = new MySql.Data.MySqlClient.MySqlConnection {
                ConnectionString = myConnectionString
            };
            dbConnection.Open();

            // start transaction so that if the group of inserts fails, we don't have issues with tables
            sql = "START TRANSACTION ";
            cmd = new MySqlCommand(sql, dbConnection);
            reader = cmd.ExecuteReader();
            reader.Close();
            cmd.Dispose();

            // first add user to users table
            sql = "INSERT INTO AliasData (AliasType, Data, AliasName, enabled) ";
            sql += "VALUES (@aliasType,@aliasData,@aliasName,1) ";
            sql += "ON DUPLICATE KEY UPDATE enabled=1 ";
            cmd = new MySqlCommand(sql, dbConnection);
            cmd.Parameters.AddWithValue("@aliasType", aliasType);
            cmd.Parameters.AddWithValue("@aliasData", aliasData);
            cmd.Parameters.AddWithValue("@aliasName", aliasID);
            
            reader = cmd.ExecuteReader();
            reader.Close();
            cmd.Dispose();

            
            // end transaction committing results
            sql = "COMMIT ";
            cmd = new MySqlCommand(sql, dbConnection);
            reader = cmd.ExecuteReader();
            reader.Close();
            reader.Dispose();

            dbConnection.Close();

        }


        public void UpdateAliasNameData(string AliasNameID, string AliasName, string entryEnabled, string deleteAlias) {
            string sql;
            MySqlCommand cmd;
            MySqlDataReader reader;


            MySql.Data.MySqlClient.MySqlConnection dbConnection;
            myConnectionString = CreateConnectionString();

            dbConnection = new MySql.Data.MySqlClient.MySqlConnection {
                ConnectionString = myConnectionString
            };
            dbConnection.Open();

            // start transaction so that if the group of inserts fails, we don't have issues with tables
            sql = "START TRANSACTION ";
            cmd = new MySqlCommand(sql, dbConnection);
            reader = cmd.ExecuteReader();
            reader.Close();
            cmd.Dispose();

            if (deleteAlias == "1") {
                sql = "DELETE FROM AliasData ";
                sql += "WHERE AliasName=@aliasNameID ";
                cmd = new MySqlCommand(sql, dbConnection);
                cmd.Parameters.AddWithValue("@aliasNameID", AliasNameID);

                reader = cmd.ExecuteReader();
                reader.Close();
                cmd.Dispose();

                // first add user to users table
                sql = "DELETE FROM AliasName ";
                sql += "WHERE id=@aliasNameID ";
                cmd = new MySqlCommand(sql, dbConnection);
                cmd.Parameters.AddWithValue("@aliasNameID", AliasNameID);

                reader = cmd.ExecuteReader();
                reader.Close();
                cmd.Dispose();
            } else {
                if (AliasNameID == "-1") {
                    sql = "INSERT INTO AliasName (Name, enabled) ";
                    sql += "VALUES (@aliasName, @entryEnabled) ";
                    sql += "ON DUPLICATE KEY UPDATE Name=@aliasName ";
                    
                    cmd = new MySqlCommand(sql, dbConnection);
                    cmd.Parameters.AddWithValue("@aliasName", AliasName);
                    cmd.Parameters.AddWithValue("@entryEnabled", entryEnabled);

                    reader = cmd.ExecuteReader();
                    reader.Close();
                    cmd.Dispose();
                } else {
                    // first add user to users table
                    sql = "UPDATE AliasName SET Name=@aliasName, enabled=@entryEnabled ";
                    sql += "WHERE id=@aliasNameID ";
                    cmd = new MySqlCommand(sql, dbConnection);
                    cmd.Parameters.AddWithValue("@aliasNameID", AliasNameID);
                    cmd.Parameters.AddWithValue("@aliasName", AliasName);
                    cmd.Parameters.AddWithValue("@entryEnabled", entryEnabled);

                    reader = cmd.ExecuteReader();
                    reader.Close();
                    cmd.Dispose();
                }
            }


            // end transaction committing results
            sql = "COMMIT ";
            cmd = new MySqlCommand(sql, dbConnection);
            reader = cmd.ExecuteReader();
            reader.Close();
            reader.Dispose();

            dbConnection.Close();

        }



        public void UpdateAliasData(string aliasDataID, string aliasData, string entryEnabled, string deleteEntry) {
            string sql;
            MySqlCommand cmd;
            MySqlDataReader reader;


            MySql.Data.MySqlClient.MySqlConnection dbConnection;
            myConnectionString = CreateConnectionString();

            dbConnection = new MySql.Data.MySqlClient.MySqlConnection {
                ConnectionString = myConnectionString
            };
            dbConnection.Open();

            // start transaction so that if the group of inserts fails, we don't have issues with tables
            sql = "START TRANSACTION ";
            cmd = new MySqlCommand(sql, dbConnection);
            reader = cmd.ExecuteReader();
            reader.Close();
            cmd.Dispose();

            if (deleteEntry == "1") {
                // first add user to users table
                sql = "DELETE FROM AliasData ";
                sql += "WHERE id=@aliasDataID ";
                cmd = new MySqlCommand(sql, dbConnection);
                cmd.Parameters.AddWithValue("@aliasDataID", aliasDataID);
                
                reader = cmd.ExecuteReader();
                reader.Close();
                cmd.Dispose();
            } else {
                // first add user to users table
                sql = "UPDATE AliasData SET Data=@aliasData, enabled=@entryEnabled ";
                sql += "WHERE id=@aliasDataID ";
                cmd = new MySqlCommand(sql, dbConnection);
                cmd.Parameters.AddWithValue("@aliasDataID", aliasDataID);
                cmd.Parameters.AddWithValue("@aliasData", aliasData);
                cmd.Parameters.AddWithValue("@entryEnabled", entryEnabled);

                reader = cmd.ExecuteReader();
                reader.Close();
                cmd.Dispose();
            }


            // end transaction committing results
            sql = "COMMIT ";
            cmd = new MySqlCommand(sql, dbConnection);
            reader = cmd.ExecuteReader();
            reader.Close();
            reader.Dispose();

            dbConnection.Close();

        }



        public void UpdateUserData(string userID, string Username, string FirstName, string Surname, string roleID, string userRoleID, string locked, string deleteEntry) {
            string sql;
            MySqlCommand cmd;
            MySqlDataReader reader;
            int lockedInt;

            if (locked == "True") {
                lockedInt = 1;
            } else {
                lockedInt = 0;
            }


            MySql.Data.MySqlClient.MySqlConnection dbConnection;
            myConnectionString = CreateConnectionString();

            dbConnection = new MySql.Data.MySqlClient.MySqlConnection {
                ConnectionString = myConnectionString
            };
            dbConnection.Open();

            // start transaction so that if the group of inserts fails, we don't have issues with tables
            sql = "START TRANSACTION ";
            cmd = new MySqlCommand(sql, dbConnection);
            reader = cmd.ExecuteReader();
            reader.Close();
            cmd.Dispose();

            if (deleteEntry == "True") {
                // first delete user from user roles table
                sql = "DELETE FROM userRoles ";
                sql += "WHERE id=@userRoleID ";
                cmd = new MySqlCommand(sql, dbConnection);
                cmd.Parameters.AddWithValue("@userRoleID", userRoleID);

                reader = cmd.ExecuteReader();
                reader.Close();
                cmd.Dispose();
                
                // now delete from users table
                sql = "DELETE FROM users ";
                sql += "WHERE id=@userID ";
                cmd = new MySqlCommand(sql, dbConnection);
                cmd.Parameters.AddWithValue("@userID", userID);

                reader = cmd.ExecuteReader();
                reader.Close();
                cmd.Dispose();
            } else {
                // first update user roles table
                sql = "UPDATE userRoles SET roleID=@roleID ";
                sql += "WHERE id=@userRoleID ";
                cmd = new MySqlCommand(sql, dbConnection);
                cmd.Parameters.AddWithValue("@roleID", roleID);
                cmd.Parameters.AddWithValue("@userRoleID", userRoleID);
                
                reader = cmd.ExecuteReader();
                reader.Close();
                cmd.Dispose();

                sql = "UPDATE users SET Username=@Username, FirstName=@FirstName, ";
                sql += "Surname=@Surname, locked=@Locked ";
                sql += "WHERE id=@id ";
                cmd = new MySqlCommand(sql, dbConnection);
                cmd.Parameters.AddWithValue("@Username", Username);
                cmd.Parameters.AddWithValue("@FirstName", FirstName);
                cmd.Parameters.AddWithValue("@Surname", Surname);
                cmd.Parameters.AddWithValue("@id", userID);
                cmd.Parameters.AddWithValue("@Locked", lockedInt);

                reader = cmd.ExecuteReader();
                reader.Close();
                cmd.Dispose();
            }


            // end transaction committing results
            sql = "COMMIT ";
            cmd = new MySqlCommand(sql, dbConnection);
            reader = cmd.ExecuteReader();
            reader.Close();
            reader.Dispose();

            dbConnection.Close();
        }




        public bool GetLogin(ref UserInfoStruct loginInfo, string theUsername, string thePassword) {
            bool loggedIn = false;
            MySql.Data.MySqlClient.MySqlConnection dbConnection;

            try {
                //Create SqlConnection
                myConnectionString = CreateConnectionString();
                dbConnection = new MySql.Data.MySqlClient.MySqlConnection();
                dbConnection.ConnectionString = myConnectionString;
                dbConnection.Open();
                
                string sql = "SELECT * from UserInfo WHERE Username=@username ";
                MySqlCommand cmd = new MySqlCommand(sql, dbConnection);
                cmd.Parameters.AddWithValue("@username", theUsername);
                
                MySqlDataReader reader = cmd.ExecuteReader();

                try {
                    if (reader.Read()) {

                        // populate the user information structure with the user info
                        loginInfo.FirstName = reader["FirstName"].ToString();
                        loginInfo.LastName = reader["Surname"].ToString();
                        loginInfo.FullName = reader["FirstName"].ToString() + " " + reader["Surname"].ToString();
                        loginInfo.PermissionLevelText = reader["roleName"].ToString();
                        loginInfo.PermissionLevelID = Int32.Parse((reader["roleID"].ToString()));
                        loginInfo.locked = reader["locked"].Equals(true);
                        loginInfo.authenticatedBy = "local";
                        loginInfo.loginError = "";
                        string fullPassword = reader["password"].ToString();
                        dbConnection.Close();
                        reader.Close();
                        reader.Dispose();
                        cmd.Dispose();

                        if (loginInfo.locked == false) {
                            string OrigPassword = Program.utilities.DecryptString(fullPassword);
                            if (thePassword == OrigPassword) {
                                loggedIn = true;
                            } else {
                                loggedIn = false;
                            }
                        }
                    } else {
                        loginInfo.authenticatedBy = "";
                        loginInfo.loginError = "";
                        // close the database connection
                        dbConnection.Close();
                    }
                } catch (Exception ex2) {
                    MessageBox.Show("Error: " + ex2);
                }
            } catch (MySql.Data.MySqlClient.MySqlException ex) {
                loginInfo.authenticatedBy = "";
                loginInfo.loginError = "";
                Console.WriteLine("MySQL Connection error: " + ex.Message);
                MessageBox.Show("MySQL Connection error: " + ex.Message);
            }

            return (loggedIn);
        }



        public void setConfigDefaults() {
            if ((Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbHost == null) ||
                (Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbHost == "")) {
                Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbHost = "10.1.30.29";
                Program.triggerConfigSave = 1;
            }

            if ((Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbName == null) ||
                (Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbName == "")) {
                Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbName = "WatchGuardExceptions";
                Program.triggerConfigSave = 1;
            }

            if ((Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbUser == null) ||
                (Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbUser == "")) {
                Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbUser = "WatchGuardUser";
                Program.triggerConfigSave = 1;
            }

            if ((Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbPassword == null) ||
                (Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbPassword == "")) {
                Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbPassword = "Useit4good!";
                Program.triggerConfigSave = 1;
            }

            if ((Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbPort == null) ||
                (Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbPort == "")) {
                Program.myAppSettings.myDBSettings.myMySQLDBSettings.DbPort = "3306";
                Program.triggerConfigSave = 1;
            }



            if ((Program.myAppSettings.myADSettings.ADDomain == null) ||
                (Program.myAppSettings.myADSettings.ADDomain == "")) {
                Program.myAppSettings.myADSettings.ADDomain = "kajeet.local";
                Program.triggerConfigSave = 1;
            }

            if ((Program.myAppSettings.myADSettings.ADServerPrimary == null) ||
                (Program.myAppSettings.myADSettings.ADServerPrimary == "")) {
                Program.myAppSettings.myADSettings.ADServerPrimary = "10.101.16.53";
                Program.triggerConfigSave = 1;
            }

            if ((Program.myAppSettings.myADSettings.ADPort == null) ||
                (Program.myAppSettings.myADSettings.ADPort == "")) {
                Program.myAppSettings.myADSettings.ADPort = "389";
                Program.triggerConfigSave = 1;
            }

            if ((Program.myAppSettings.myADSettings.ADUserBase == null) ||
                (Program.myAppSettings.myADSettings.ADUserBase == "")) {
                Program.myAppSettings.myADSettings.ADUserBase = "dc=kajeet,dc=local";
                Program.triggerConfigSave = 1;
            }

            if ((Program.myAppSettings.myADSettings.BindUsername == null) ||
                (Program.myAppSettings.myADSettings.BindUsername == "")) {
                Program.myAppSettings.myADSettings.BindUsername = "administrator";
                Program.triggerConfigSave = 1;
            }

            if ((Program.myAppSettings.myADSettings.BindPassword == null) ||
                (Program.myAppSettings.myADSettings.BindPassword == "")) {
                Program.myAppSettings.myADSettings.BindPassword = "administrator";
                Program.triggerConfigSave = 1;
            }

        }


        public string EncryptString(string OrigPassword) {
            int myIterations = 726;
            return (EncryptString(OrigPassword, myIterations));
        }


        public string EncryptString(string OrigPassword, int myIterations) {
            string pwd1 = "ppPassw05r";
            string encryptedPassword;
            byte[] edata1;
            string saltString;
            string ivString;

            // Create a byte array to hold the random value. 
            byte[] salt1 = new byte[8];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider()) {
                // Fill the array with a random value.
                rngCsp.GetBytes(salt1);
            }
            saltString = Convert.ToBase64String(salt1);

            //data1 can be a string or contents of a file.
            string data1 = OrigPassword;

            //The default iteration count is 1000 so the two methods use the same iteration count.
            try {
                Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(pwd1, salt1, myIterations);

                // Encrypt the data.
                TripleDES encAlg = TripleDES.Create();
                encAlg.Key = k1.GetBytes(16);
                ivString = Convert.ToBase64String(encAlg.IV);
                MemoryStream encryptionStream = new MemoryStream();
                CryptoStream encrypt = new CryptoStream(encryptionStream, encAlg.CreateEncryptor(), CryptoStreamMode.Write);
                byte[] utfD1 = new System.Text.UTF8Encoding(false).GetBytes(data1);

                encrypt.Write(utfD1, 0, utfD1.Length);
                encrypt.FlushFinalBlock();
                encrypt.Close();
                edata1 = encryptionStream.ToArray();
                k1.Reset();
                encryptedPassword = Convert.ToBase64String(edata1);

            } catch (Exception e) {
                Console.WriteLine("Error: ", e);
                encryptedPassword = "";
                ivString = "";
            }
            //Console.WriteLine(saltString + "," + ivString + "," + encryptedPassword);

            return (saltString + "," + ivString + "," + encryptedPassword);
        }



        public string DecryptString(string EncryptedPassword, string saltString, string ivString) {
            int myIterations = 726;
            return (DecryptString(EncryptedPassword, saltString, ivString, myIterations));
        }


        public string DecryptString(string EncryptedPassword, string saltString, string ivString, int myIterations) {
            string pwd1 = "ppPassw05r";
            byte[] edata1;
            byte[] iv;
            string data2;


            // Create a byte array to hold the random value. 
            byte[] salt1 = Convert.FromBase64String(saltString);
            edata1 = Convert.FromBase64String(EncryptedPassword);
            iv = Convert.FromBase64String(ivString);

            //The default iteration count is 1000 so the two methods use the same iteration count.
            try {
                Rfc2898DeriveBytes k2 = new Rfc2898DeriveBytes(pwd1, salt1, myIterations);

                TripleDES decAlg = TripleDES.Create();
                decAlg.Key = k2.GetBytes(16);
                decAlg.IV = iv;
                MemoryStream decryptionStreamBacking = new MemoryStream();
                CryptoStream decrypt = new CryptoStream(decryptionStreamBacking, decAlg.CreateDecryptor(), CryptoStreamMode.Write);
                decrypt.Write(edata1, 0, edata1.Length);
                decrypt.Flush();
                decrypt.Close();
                k2.Reset();
                data2 = new UTF8Encoding(false).GetString(decryptionStreamBacking.ToArray());

            } catch (Exception e) {
                Console.WriteLine("Error: ", e);
                data2 = "";
            }

            return (data2);
        }


        public string DecryptString(string FullEncryptedPassword) {
            int myIterations = 726;
            return (DecryptString(FullEncryptedPassword, myIterations));
        }

        public string DecryptString(string FullEncryptedPassword, int myIterations) {

            Int32 fieldCount = 3;
            String[] separator = { "," };
            String[] strlist = FullEncryptedPassword.Split(separator, fieldCount, StringSplitOptions.RemoveEmptyEntries);
            string salt = strlist[0];
            string password = strlist[2];
            string iv = strlist[1];
            string OrigPassword = Program.utilities.DecryptString(password, salt, iv, myIterations);
            
            return (OrigPassword);
        }



        public DataSet getRolesDataSet() {
            MySql.Data.MySqlClient.MySqlConnection dbConnection;
            myConnectionString = CreateConnectionString();

            dbConnection = new MySql.Data.MySqlClient.MySqlConnection {
                ConnectionString = myConnectionString
            };
            dbConnection.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter adapter = new MySqlDataAdapter( "SELECT id,roleName from roles ",dbConnection);
            adapter.Fill(ds);
            adapter.Dispose();
            dbConnection.Close();

            return (ds);
        }



        public bool addUser(string username, string firstName, string surName, string password, int userRoleID) {
            bool userAdded = false;
            string sql;
            string encryptedPassword;
            MySqlCommand cmd;
            MySqlDataReader reader;

            encryptedPassword = EncryptString(password);

            MySql.Data.MySqlClient.MySqlConnection dbConnection;
            myConnectionString = CreateConnectionString();

            dbConnection = new MySql.Data.MySqlClient.MySqlConnection {
                ConnectionString = myConnectionString
            };
            dbConnection.Open();

            // start transaction so that if the group of inserts fails, we don't have issues with tables
            sql = "START TRANSACTION ";
            cmd = new MySqlCommand(sql, dbConnection);
            reader = cmd.ExecuteReader();
            reader.Close();
            cmd.Dispose();

            // first add user to users table
            sql = "INSERT INTO users (Username, FirstName, Surname, password, locked) ";
            sql += "VALUES (@username,@firstname,@surname,@password,0) ";
            cmd = new MySqlCommand(sql, dbConnection);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@firstname", firstName);
            cmd.Parameters.AddWithValue("@surname", surName);
            cmd.Parameters.AddWithValue("@password", encryptedPassword);

            reader = cmd.ExecuteReader();
            reader.Close();
            cmd.Dispose();

            if (reader.RecordsAffected > 0) {
                sql  = "SELECT id,username FROM users  ";
                sql += "WHERE username=@username ";
                cmd = new MySqlCommand(sql, dbConnection);
                cmd.Parameters.AddWithValue("@username", username);
                reader = cmd.ExecuteReader();
                if (reader.Read()) {
                    if (reader["id"] != null) {
                        int userIDDB = Convert.ToInt32(reader["id"].ToString());
                        reader.Close();
                        sql  = "INSERT INTO userRoles (userID, roleID) ";
                        sql += "VALUES (@userID, @roleID) ";
                        cmd = new MySqlCommand(sql, dbConnection);
                        cmd.Parameters.AddWithValue("@userID", userIDDB);
                        cmd.Parameters.AddWithValue("@roleID", userRoleID);
                        reader = cmd.ExecuteReader();
                        if (reader.RecordsAffected > 0) {
                            reader.Close();
                            reader.Dispose();
                            cmd.Dispose();

                            // end transaction committing results
                            sql = "COMMIT ";
                            cmd = new MySqlCommand(sql, dbConnection);
                            reader = cmd.ExecuteReader();
                            reader.Close();
                            reader.Dispose();

                            userAdded = true;
                        
                        } else {
                            reader.Close();
                            reader.Dispose();
                        }

                    }
                }
                
                cmd.Dispose();

            }
            dbConnection.Close();

            return (userAdded);
        }




        public void CreateFullExceptionList(DataSet ds) {
            
            MySql.Data.MySqlClient.MySqlConnection dbConnection;
            myConnectionString = CreateConnectionString();

            dbConnection = new MySql.Data.MySqlClient.MySqlConnection {
                ConnectionString = myConnectionString
            };
            dbConnection.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT Type,Data FROM WatchGuardFullExceptionList ", dbConnection);
            adapter.Fill(ds);
            adapter.Dispose();

            return;
        }


        public void CreateFullExceptionListCSV(DataSet myDS) {
            string sql;
            
            MySql.Data.MySqlClient.MySqlConnection dbConnection;
            myConnectionString = CreateConnectionString();

            dbConnection = new MySql.Data.MySqlClient.MySqlConnection {
                ConnectionString = myConnectionString
            };
            dbConnection.Open();
            sql  = "SELECT CustomerName as AliasName, AliasType, Data, ";
            sql += "CustomerEnabled as enabled ";
            sql += "FROM FullExceptionList ";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, dbConnection);
            adapter.Fill(myDS);
            adapter.Dispose();

            return;
        }




        private string _ADAdministratorGroupDN;
        public string ADAdministratorGroupDN {
            get { return _ADAdministratorGroupDN; }
            set {
                Int32 fieldCount = 3;
                String[] separator = { "," };
                String[] strlist = value.Split(separator, fieldCount, StringSplitOptions.RemoveEmptyEntries);

                if ((strlist.Length == 3) && (!(value.Contains(",DC")))) {
                    _ADAdministratorGroupDN = value;
                } else {
                    Program.hasEncryptedConfigPassword = 0;
                    _ADAdministratorGroupDN = Program.utilities.EncryptString(value, 846);
                }
            }
        }


        private string _ADReadOnlyGroupDN;
        public string ADReadOnlyGroupDN {
            get { return _ADReadOnlyGroupDN; }
            set {
                Int32 fieldCount = 3;
                String[] separator = { "," };
                String[] strlist = value.Split(separator, fieldCount, StringSplitOptions.RemoveEmptyEntries);

                if ((strlist.Length == 3) && (!(value.Contains(",DC")))) {
                    _ADReadOnlyGroupDN = value;
                } else {
                    Program.hasEncryptedConfigPassword = 0;
                    _ADReadOnlyGroupDN = Program.utilities.EncryptString(value, 657);
                }
            }
        }

    }
}
