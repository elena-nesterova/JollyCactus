using JollyCactus.Maui.Model;
using JollyCactus.Maui.ViewModel.PlantProperties;
using System.Collections.ObjectModel;

namespace JollyCactus.Maui.Views.PropertyViews.Adaprers
{
    public class PlantPropertyFlagsToView : ViewModel.PlantProperties.PlantPropertyVM
    {
        public int Value { get; }

        public ObservableCollection<PlantPropertyFlag> Values { get; set; } = new();

        public override PlantPropertyType PropertyType => PlantPropertyType.PlantPropertyFlags;

        public PlantPropertyFlagsToView(string name, string description, int value = 0) :
           base(name, description)
        {
            Value = value;
        }

        public override string AsPersistenceString()
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override void UpdateFrom(PlantPropertyVM property)
        {
            throw new NotImplementedException();
        }
    }
}
