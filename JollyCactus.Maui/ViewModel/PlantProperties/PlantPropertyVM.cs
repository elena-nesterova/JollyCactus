using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JollyCactus.Maui.ViewModel.PlantProperties
{
    public abstract class PlantPropertyVM : BaseViewModel, ICloneable
    {
        public string Name { get; set; }

        public string ParentName { get; set; }

        public abstract Model.PlantPropertyType PropertyType { get; }

        public string Description { get; set; }

        public bool IsDisplay { get; set; }



        protected PlantPropertyVM(string name, string description, string parentName)
        {
            Name = name;
            Description = description;
            IsDisplay = true;
            ParentName = parentName;
        }

        /*protected PlantPropertyVM(PlantPropertyVM propertyVM)
        {
            Name = propertyVM.Name;            
            Description = propertyVM.Description;
            //Value = propertyVM.Value;
            IsDisplay = propertyVM.IsDisplay;
        }*/

        public abstract void UpdateFrom(PlantPropertyVM property);

        public abstract string AsPersistenceString();

        public abstract object Clone();
    }

    public abstract class PlantPropertyVM<T> : PlantPropertyVM
    {
        private T _value;

        //protected PlantPropertyVM(PlantPropertyVM propertyVM) : base(propertyVM)
        //{
        //}

        protected PlantPropertyVM(string name, string description, string parentName) : base(name, description, parentName)
        {
        }

        public T Value
        {
            get => _value;
            set
            {
                if (_value == null || !_value.Equals(value))
                {
                    _value = value;
                    IsChanged = true;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }


    }
}
