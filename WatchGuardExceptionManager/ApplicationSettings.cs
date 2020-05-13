using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchGuardExceptionManager {
    public class ApplicationSettings {

        public DBSettings myDBSettings;
        public ADSettings myADSettings;

        public ApplicationSettings() {
            myDBSettings = new DBSettings();
            myADSettings = new ADSettings();
        }

    }
}
