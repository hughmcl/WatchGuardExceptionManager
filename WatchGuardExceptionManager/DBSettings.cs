using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchGuardExceptionManager {
    public class DBSettings {
        public MySQLDBSettings myMySQLDBSettings;

        public DBSettings() {
            myMySQLDBSettings = new MySQLDBSettings();
        }

    }
}
