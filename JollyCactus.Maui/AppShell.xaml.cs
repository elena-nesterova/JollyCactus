using JollyCactus.Maui.Views;

namespace JollyCactus.Maui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            //Routing.RegisterRoute(nameof(LocationTablePage), typeof(LocationTablePage));
            //Routing.RegisterRoute(nameof(PlantTablePage), typeof(PlantTablePage));
            //Routing.RegisterRoute(nameof(LocationPage), typeof(LocationPage));
            //Routing.RegisterRoute(nameof(PlantTablePage) + "/" + nameof(PlantPage), typeof(PlantPage));
            //Routing.RegisterRoute(nameof(ModifyPropertyPage), typeof(ModifyPropertyPage));
        }
    }
}
