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
    public partial class addExceptionsImportListForm : Form {
        DataGridView myView;

        public addExceptionsImportListForm(DataGridView theView) {
            InitializeComponent();

            myView = theView;


            DataSet AliasNamesDS = new DataSet();
            Program.utilities.getAliasNamesDS(AliasNamesDS);

            addExceptionsImportAliasBox.DataSource = AliasNamesDS.Tables[0];
            addExceptionsImportAliasBox.DisplayMember = "AliasName";
            addExceptionsImportAliasBox.ValueMember = "AliasNameID";
        }

        private void addExceptionsListSelectFileButton_Click(object sender, EventArgs e) {
            string line;
            string fileName;
            int linecount = 0;
            System.IO.StreamReader file;
            string ipRangePattern = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\-\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";
            string ipCidrPattern = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\/\d{1,3}$";
            string ipPlainPattern = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";
            string desktopFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Match mRange;
            Match mCidr;
            Match mPlain;
            DataRowView adminIDRow;
            DataGridViewRow newRow;
            adminIDRow = addExceptionsImportAliasBox.SelectedItem as DataRowView;
            int aliasID = Convert.ToInt32(adminIDRow.Row["AliasNameID"].ToString());
            int aliasTypeID;

            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "Select Import File";
            openDialog.Filter = "Text Files (*.txt;*.csv)|*.txt;*.csv" + "|" +
                                "All Files (*.*)|*.*";
            openDialog.InitialDirectory = desktopFilePath;
            
            if (openDialog.ShowDialog() == DialogResult.OK) {
                fileName = openDialog.FileName;

                file = new System.IO.StreamReader(fileName);
                while ((line = file.ReadLine()) != null) {
                    line = Regex.Replace(line, @"\s+", "");

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

                file.Close();
                System.Console.WriteLine("There were {0} lines.", linecount);
                myView.Update();
                myView.Refresh();
                this.Close();
            }
        }
    }
}
