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
    public partial class addAliasNameForm : Form {
        public addAliasNameForm() {
            InitializeComponent();
        }




        private void addAliasNameGrid_CellContentClick(object sender, DataGridViewCellEventArgs e) {

        }




        private void aliasAddSaveButton_Click(object sender, EventArgs e) {
            string AliasName;
            string AliasNameID = "-1";
            string entryEnabled = "1";
            string deleteAlias = "0";
            
            //Update button update dataset after insertion,upadtion or deletion
            DialogResult dr = MessageBox.Show("Are you sure to save Changes", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes) {

                foreach (DataGridViewRow datarow in addAliasNameGrid.Rows) {
                    if (datarow.Cells["AliasName"].Value != null) {
                        AliasName = datarow.Cells["AliasName"].Value.ToString();
                        
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

                }

            }

            this.Close();
        }
    }
}
