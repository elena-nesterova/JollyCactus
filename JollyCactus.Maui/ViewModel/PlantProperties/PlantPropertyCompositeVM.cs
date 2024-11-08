using JollyCactus.Maui.Model;
using System.Collections.ObjectModel;

namespace JollyCactus.Maui.ViewModel.PlantProperties
{
    public class PlantPropertyCompositeVM : PlantPropertyVM
    {
        public ObservableCollection<PlantPropertyVM> SubProperties { get; set; } = new();

        //public PlantPropertyCompositeVM(PlantPropertyVM propertyVM) : base(propertyVM)
        //{
        //}

        public PlantPropertyCompositeVM(string name, string description) : base(name, description)
        {
        }

        public override PlantPropertyType PropertyType => PlantPropertyType.PlantPropertyComposite;

        public override string AsPersistenceString()
        {
            throw new NotImplementedException();
        }

        public override void UpdateFrom(PlantPropertyVM property)
        {
            if (property is PlantPropertyCompositeVM propComp)
            {
                foreach (var subProp in SubProperties)
                {
                    var subSrcProp = propComp.SubProperties.FirstOrDefault(x => x.Name.Equals(subProp.Name), null);
                    if (subSrcProp != null)
                    {
                        subProp.UpdateFrom(subSrcProp);
                    }
                }
            }
        }

        public override object Clone()
        {
            PlantPropertyCompositeVM clone = (PlantPropertyCompositeVM)MemberwiseClone();

            clone.SubProperties = new();
            foreach (PlantPropertyVM subProp in SubProperties)
            {
                clone.SubProperties.Add((PlantPropertyVM)subProp.Clone());
            }
            return clone;
        }
    }
}
