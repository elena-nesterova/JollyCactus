using JollyCactus.Maui.Settings;


namespace JollyCactus.Maui
{
    public partial class App : Application
    {
        static Data.LocalDbService _database = null;

        public App()
        {
            InitializeComponent();
            
            

            MainPage = new AppShell();
        }

        public static Data.LocalDbService Database
        {
            get
            {
                if (_database == null)
                {
                    var jcSettings = Application.Current.MainPage.Handler.MauiContext.Services.GetService<JCSettings>();

                    var db = new Data.LocalDbService(jcSettings.CurrentDbFullFileName);

                    if (db == null)
                    {
                        throw new System.Data.DataException();
                    }

                    _database = db;
                }
                return _database;
            }
        }

    }
}
