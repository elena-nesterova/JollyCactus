using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JollyCactus.Maui
{
    public class JcPaths
    {
        public static string DbExtension => ".db3";

        public static string GetAppFilesPath()
        {
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                return FileSystem.AppDataDirectory;
            }
            else if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                SQLitePCL.Batteries_V2.Init();
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "..",
                "Library");
            }

            return "";
        }

    }
}
