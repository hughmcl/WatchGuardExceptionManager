using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchGuardExceptionManager {
    public class MySQLDBSettings {

        private string _dbPassword;
        public string DbHost { get; set; }
        public string DbName { get; set; }
        public string DbUser { get; set; }
        public string DbPort { get; set; }
        
        public string DbPassword { 
            get { return _dbPassword; } 
            set {
                Int32 fieldCount = 3;
                String[] separator = { "," };
                String[] strlist = value.Split(separator, fieldCount, StringSplitOptions.RemoveEmptyEntries);
                if (strlist.Length == 3) {
                    _dbPassword = value;
                } else {
                    Program.hasEncryptedConfigPassword = 0;
                    _dbPassword = Program.utilities.EncryptString(value);
                }
            }
        }

    }
}
 