using System.Collections.ObjectModel;
using JollyCactus.Maui.ViewModel.PlantProperties;

namespace JollyCactus.Maui.ViewModel
{
    public class PropertyGroupVM: BaseViewModel
    {
        public string GroupName { get; set; }

        public ObservableCollection<PlantPropertyVM> Properties { get; set; }
    }
}
