using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchGuardExceptionManager {
    public class ADSettings {

        public string BindUsername { get; set; }

        private string _bindPassword;
        public string BindPassword {
            get { return _bindPassword; }
            set {
                Int32 fieldCount = 3;
                String[] separator = { "," };
                String[] strlist = value.Split(separator, fieldCount, StringSplitOptions.RemoveEmptyEntries);

                if (strlist.Length == 3) {
                    _bindPassword = value;
                } else {
                    Program.hasEncryptedConfigPassword = 0;
                    _bindPassword = Program.utilities.EncryptString(value);
                }
            }
        }

        public string ADDomain { get; set; }

        public string ADServerPrimary { get; set; }

        public string ADServerSecondary { get; set; }


        public string ADPort { get; set; }

        public string ADUserBase { get; set; }


    }
}
