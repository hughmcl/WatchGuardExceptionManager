using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace WatchGuardExceptionManager {
    public partial class aboutForm : Form {
        public aboutForm() {
            InitializeComponent();
            //label1.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();
            aboutProgramNameText.Text = Program.programName; 
            aboutWrittenByText.Text = Program.writtenBy;
            aboutBuildDateText.Text = Program.buildDateTime;
            aboutBuildVersionText.Text = Program.buildVersion;


        }

        private void aboutOKButton_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
