using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JollyCactus.Maui
{
    internal class JollyCactusPaths
    {
        public static string GetPropertiesDatabasePath() => CreateDBPath("jolly_Cactus_Properties.db3");


        public static string GetDatabasePath() => CreateDBPath("jolly_Cactus_db.db3");
        

        private static string CreateDBPath(string dbName)
        {
            string dbPath = "";


            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                dbPath = Path.Combine(FileSystem.AppDataDirectory, dbName);
            }
            else if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                SQLitePCL.Batteries_V2.Init();
                dbPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "..",
                    "Library",
                    dbName);
            }

            return dbPath;
        }
    }
}
