using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WatchGuardExceptionManager {
    public partial class AddExceptionsForm : Form {
        public AddExceptionsForm() {
            InitializeComponent();
        }

        private void AddExceptionsForm_Load(object sender, EventArgs e) {
            
            Program.utilities.buildAddExceptionsDataGridView(addExceptionsDataGrid);


        }

        private void addExceptionsSaveButton_Click(object sender, EventArgs e) {
            string aliasID;
            string aliasType;
            string aliasData;
            bool exceptionAdded = false;

            foreach (DataGridViewRow row in addExceptionsDataGrid.Rows) {
                aliasID = "";
                aliasType = "";
                aliasData = "";
                if (row.Cells["addExceptionsAliasName"].Value != null) {
                    aliasID = row.Cells["addExceptionsAliasName"].Value.ToString();
                }
                if (row.Cells["addExceptionsAliasType"].Value != null) {
                    aliasType = row.Cells["addExceptionsAliasType"].Value.ToString();
                }
                if (row.Cells["addExceptionsData"].Value != null) {
                    aliasData = row.Cells["addExceptionsData"].Value.ToString();
                }

                if ((aliasID == "") || (aliasType == "") || (aliasData == "")) {
                    if ((aliasID != "") || (aliasType != "") || (aliasData != "")) {
                        MessageBox.Show("Cannot have an empty field, all Fields need to be filled in!");
                    }
                } else {
                    Program.utilities.AddExceptionEntry(aliasID, aliasType, aliasData);
                    if (exceptionAdded == false) {
                        exceptionAdded = true;
                    }
                }
                
            }
            
            if (exceptionAdded == true) {
                this.Close();
            }

            return;
        }

        private void addExceptionsCancelButton_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void addExceptionsImportListButton_Click(object sender, EventArgs e) {
            
            Form addExceptionsImport = new addExceptionsImportListForm(addExceptionsDataGrid);
            addExceptionsImport.ShowDialog();

            //DataSet fullExceptionsDS = new DataSet();
            //Program.utilities.getFullExceptionsDS(fullExceptionsDS);
            //fullExceptionsGrid.DataSource = fullExceptionsDS.Tables[0];
            //fullExceptionsGrid.Update();
            //fullExceptionsGrid.Refresh();

        }

        private void addExceptionsPasteListButton_Click(object sender, EventArgs e) {
            Form addExceptionsPaste = new addExceptionsPasteListForm(addExceptionsDataGrid);
            addExceptionsPaste.ShowDialog();
        }
    }
}
