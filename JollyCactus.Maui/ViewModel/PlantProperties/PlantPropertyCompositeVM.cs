using JollyCactus.Maui.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace JollyCactus.Maui.ViewModel.PlantProperties
{
    public class PlantPropertyCompositeVM : PlantPropertyVM
    {
        public ReadOnlyObservableCollection<PlantPropertyVM> SubProperties { get; }
        private ObservableCollection<PlantPropertyVM> _subProperties { get; set; }

        public PlantPropertyCompositeVM(string name, string description, string parentName) : base(name, description, parentName)
        {
            _subProperties = new();
            SubProperties = new(_subProperties);
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

        public void AddSubProperty(PlantPropertyVM subProp)
        {
            _subProperties.Add(subProp);
            subProp.PropertyChanged += OnSubPropertyChanged;
        }

        
        private void OnSubPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.PropertyName) && e.PropertyName.Equals(nameof(IsChanged)))
            {
                if ((_subProperties.Where(x=>x.IsChanged)).Any()) 
                    IsChanged = true;
                else
                    IsChanged = false;
                OnPropertyChanged(nameof(IsChanged));
            }                
        }
        
        public override object Clone()
        {
            PlantPropertyCompositeVM clone = new(Name, Description, ParentName);
                        
            foreach (PlantPropertyVM subProp in SubProperties)
            {
                clone.AddSubProperty((PlantPropertyVM)subProp.Clone());
            }
            return clone;
        }
    }
}
