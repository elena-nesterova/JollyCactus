using Microsoft.Maui.Storage;
using System.Xml.Linq;

namespace JollyCactus.Maui.Settings
{
    internal class JcAccountPaths
    {
        // TODO: to configuration file
        //public static string GetPropertiesDatabasePath() => CreateDBFullFileName("jolly_Cactus_Properties.db3");
        

        public static string GetSettingsFullFileName() => GetFullFileName("jolly_Cactus_Settings.json", null);

        public static string GetNewAccountDirectoryName() => string.Format("JollyCactus_Account_{0:ddMMyyyyHHmmss}", DateTime.Now);
        
        public static string GetNewDbFileName() => string.Format("JollyCactus_Database_{0:dd-MM-yyyy_HH-mm-ss-tt}.db3", DateTime.Now);
               
        public static string GetAccountDirectory(Account account) => GetFullFileName("", account.DiectoryName);

        public static string GetFullFileNameByName(string fileName, Account account) => GetFullFileName(fileName, account.DiectoryName);
        
        
        private static string GetFullFileName(string fileName, string? accountDir)
        {
            if (string.IsNullOrEmpty(accountDir))
                return Path.Combine(JcPaths.GetAppFilesPath(), fileName);

            return Path.Combine(JcPaths.GetAppFilesPath(), accountDir, fileName);
        }
    }
}
