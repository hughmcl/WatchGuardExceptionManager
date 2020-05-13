using System.Configuration;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.DirectoryServices;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Globalization;

public struct UserInfoStruct
{
    public string Username;
    public string FirstName;
    public string LastName;
    public string FullName;
    public string PermissionLevelText;
    public int PermissionLevelID;
    public bool locked;
    public string loginError;
    public string authenticatedBy;

};




namespace WatchGuardExceptionManager {

    [AttributeUsage(AttributeTargets.Assembly)]
    internal class BuildDateAttribute : Attribute {
        public BuildDateAttribute(string value) {
            DateTime = DateTime.ParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        public DateTime DateTime { get; }
    }


    static class Program {

        public static UtilitiesClass utilities;

        public static AppSettingsXML myAppSettingsXML;

        public static ApplicationSettings myAppSettings;

        public static DataSet myADAdminGroups;
        public static DataSet myADReadOnlyGroups;
        public static BindingSource myADAdminGroupsBS;
        public static BindingSource myADReadOnlyGroupsBS;


        public static int hasEncryptedConfigPassword = 1;
        public static int triggerConfigSave = 0;
        public static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string configFilename = localAppData+ "\\WatchGuardExceptionManager\\WatchGuardExceptionManagerConfig.xml";
        //public static DateTime buildDateTime;
        public static string buildDateTime;
        public static string programName = "WatchGuard Exception Manager";
        public static string writtenBy = "Hugh McLenaghan";
        public static string buildVersion;
        public static string dateTimeString;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread] 
        static void Main() {
            int exitApp = 0;
            
            utilities = new UtilitiesClass(); 
            myAppSettingsXML = new AppSettingsXML();
            myAppSettings = new ApplicationSettings();

           

            utilities.setSkipAD(0);

            buildDateTime = Properties.Resources.BuildDate.ToString();
            dateTimeString = Properties.Resources.VersionString.ToString();
            buildVersion = "1.1.2." + dateTimeString;


            // read config
            if (File.Exists(configFilename)) {
                myAppSettings = myAppSettingsXML.XmlDataReader(configFilename);

                Program.utilities.setConfigDefaults();

                if ((hasEncryptedConfigPassword == 0) || (triggerConfigSave == 1)) {
                    // we'll save the new config file with encrypted password
                    myAppSettingsXML.XmlDataWriter(myAppSettings, configFilename);
                }

                myADAdminGroups = utilities.getADGroups(myAppSettings.myADSettings);
                myADAdminGroupsBS = new BindingSource();
                myADAdminGroupsBS.DataSource = myADAdminGroups.Tables["AD Groups"];
                myADReadOnlyGroups = utilities.getADGroups(myAppSettings.myADSettings);
                myADReadOnlyGroupsBS = new BindingSource();
                myADReadOnlyGroupsBS.DataSource = myADReadOnlyGroups.Tables["AD Groups"];

                utilities.ADAdministratorGroupDN = utilities.getGroupDNFromDB("ADAdministratorGroupDN");
                utilities.ADReadOnlyGroupDN = utilities.getGroupDNFromDB("ADReadOnlyGroupDN");

            } else {
                MessageBox.Show("Application Config File not found!\n"+configFilename);
                exitApp = 1;
                System.Windows.Forms.Application.Exit();
            }

            if (exitApp == 0) {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new loginForm());
            }
        }

        public static DateTime GetLinkerDateTime(this Assembly assembly, TimeZoneInfo tzi = null) {
            // Constants related to the Windows PE file format.
            const int PE_HEADER_OFFSET = 60;
            const int LINKER_TIMESTAMP_OFFSET = 8;

            // Discover the base memory address where our assembly is loaded
            var entryModule = assembly.ManifestModule;
            var hMod = Marshal.GetHINSTANCE(entryModule);
            if (hMod == IntPtr.Zero - 1) throw new Exception("Failed to get HINSTANCE.");

            // Read the linker timestamp
            var offset = Marshal.ReadInt32(hMod, PE_HEADER_OFFSET);
            var secondsSince1970 = Marshal.ReadInt32(hMod, offset + LINKER_TIMESTAMP_OFFSET);

            // Convert the timestamp to a DateTime
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);
            var dt = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tzi ?? TimeZoneInfo.Local);
            return dt;
        }

        private static DateTime GetBuildDate(Assembly assembly) {
            const string BuildVersionMetadataPrefix = "+build";

            var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            if (attribute?.InformationalVersion != null) {
                var value = attribute.InformationalVersion;
                var index = value.IndexOf(BuildVersionMetadataPrefix);
                if (index > 0) {
                    value = value.Substring(index + BuildVersionMetadataPrefix.Length);
                    if (DateTime.TryParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result)) {
                        return result;
                    }
                }
            }

            

            return default;
        }

    }
}
