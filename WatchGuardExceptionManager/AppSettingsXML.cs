using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace WatchGuardExceptionManager {
    public class AppSettingsXML {
        public void XmlDataWriter(object obj, string filename) {
            XmlSerializer sr = new XmlSerializer(obj.GetType());
            TextWriter writer = new StreamWriter(filename);
            sr.Serialize(writer, obj);
            writer.Close();
        }

        public ApplicationSettings XmlDataReader(string filename) {
            ApplicationSettings obj = new ApplicationSettings();
            XmlSerializer xs = new XmlSerializer(typeof(ApplicationSettings));
            FileStream reader = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            obj = (ApplicationSettings)xs.Deserialize(reader);
            reader.Close();
            return obj;
        }
    }
}
