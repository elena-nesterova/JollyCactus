using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JollyCactus.Maui.ViewModel.PlantProperties
{
    public class PlantPropertyOneFromListVM : PlantPropertyVM<string>
    {
        public ObservableCollection<string> AllPossibleValues { get; private set; }
           

        //public ICommand SelectedValuesChangedCommand { get; set; }


        public PlantPropertyOneFromListVM(string name, string description, string persistenceStringValue = "") :
            base(name, description)
        {
            AllPossibleValues = new();
            
            List<string> allValues = null;
            
            if (Model.PlantPropertiesValues.PlantPropertiesValuesDict.ContainsKey(Name))
                allValues = Model.PlantPropertiesValues.PlantPropertiesValuesDict[Name];

            Debug.Assert(allValues != null);

            foreach (string val in allValues)
            {
                AllPossibleValues.Add(val);                
            }

            if (!string.IsNullOrEmpty(persistenceStringValue))
            {
                var val = persistenceStringValue.Trim();
               
                if(AllPossibleValues.Contains(val))
                    Value = val;                
            }

            if (string.IsNullOrEmpty(Value))
                Value = AllPossibleValues[AllPossibleValues.Count - 1];

            //SelectedValuesChangedCommand = new Command<object>(SelectedValuesChanged);
        }

        public override Model.PlantPropertyType PropertyType => Model.PlantPropertyType.PlantPropertyOneFromList;


        public override void UpdateFrom(PlantPropertyVM property)
        {
            if (property is PlantPropertyOneFromListVM propTyped)
            {
                if (AllPossibleValues.Contains(propTyped.Value))
                    Value = propTyped.Value;
            }
                
            OnPropertyChanged(nameof(Value));
        }

        public override string AsPersistenceString()
        {
            return Value;
        }

        public override object Clone()
        {
            var clone = new PlantPropertyOneFromListVM(Name, Description, Value);

            return clone;
        }

        public override string ToString()
        {
            return Value;
        }

       /* public void SelectedValuesChanged(object obj)
        {
            Value.Clear();
            foreach (var val in SelectedObjects)
            {
                if (val is PlantPropertyStringsFromListOneString valTyped)
                    Value.Add(valTyped);
            }

            OnPropertyChanged(nameof(Value));
        }*/
    }
}
