using System.Data.Common;

namespace JollyCactus.Maui
{
    public partial class App : Application
    {
        static Data.LocalDbService _database;

        public App()
        {
            InitializeComponent();

            var db = new Data.LocalDbService(JollyCactusPaths.GetDatabasePath());

            if (db == null)
            {
                throw new System.Data.DataException();
            }
            _database = db;

            MainPage = new AppShell();
        }

        public static Data.LocalDbService Database
        {
            get
            {
                return _database;
            }
        }
    }
}
