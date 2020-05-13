using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace WatchGuardExceptionManager {
    public partial class addExceptionsPasteListForm : Form {

        DataGridView myView;


        public addExceptionsPasteListForm(DataGridView theView) {
            InitializeComponent();

            myView = theView;


            DataSet AliasNamesDS = new DataSet();
            Program.utilities.getAliasNamesDS(AliasNamesDS);

            addExceptionsPasteAliasBox.DataSource = AliasNamesDS.Tables[0];
            addExceptionsPasteAliasBox.DisplayMember = "AliasName";
            addExceptionsPasteAliasBox.ValueMember = "AliasNameID";
        }


        private void addExceptionsPasteSubmit_Click(object sender, EventArgs e) {
            int linecount = 0;
            string line;
            string ipRangePattern = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\-\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";
            string ipCidrPattern = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\/\d{1,3}$";
            string ipPlainPattern = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";
            Match mRange;
            Match mCidr;
            Match mPlain;
            DataRowView adminIDRow;
            DataGridViewRow newRow;
            
            adminIDRow = addExceptionsPasteAliasBox.SelectedItem as DataRowView;
            int aliasID = Convert.ToInt32(adminIDRow.Row["AliasNameID"].ToString());
            int aliasTypeID;

            string[] lines = addExceptionsPasteListBox.Lines;
            foreach(string dataline in lines) { 
                line = Regex.Replace(dataline, @"\s+", "");

                if (line != "") {
                    mRange = Regex.Match(line, ipRangePattern);
                    mCidr = Regex.Match(line, ipCidrPattern);
                    mPlain = Regex.Match(line, ipPlainPattern);

                    if (mRange.Success) {
                        aliasTypeID = 4;
                        //MessageBox.Show("Alias: "+aliasID+" -> Found IP Range! " + mRange.Value);
                    } else if (mCidr.Success) {
                        aliasTypeID = 3;
                        //MessageBox.Show("Alias: " + aliasID + " -> Found IP CIDR! " + mCidr.Value);
                    } else if (mPlain.Success) {
                        aliasTypeID = 1;
                        //MessageBox.Show("Alias: " + aliasID + " -> Found IP! " + mPlain.Value);
                    } else {
                        aliasTypeID = 2;
                        //MessageBox.Show("Alias: " + aliasID + " -> Assuming FQDN: " + line);
                    }

                    newRow = (DataGridViewRow)myView.Rows[0].Clone();
                    newRow.Cells[0].Value = aliasID;
                    newRow.Cells[1].Value = aliasTypeID;
                    newRow.Cells[2].Value = line;
                    myView.Rows.Add(newRow);

                    System.Console.WriteLine(line);
                    linecount++;
                }
            }

            System.Console.WriteLine("There were {0} lines.", linecount);
            myView.Update();
            myView.Refresh();
            this.Close();
        }
    }
}
