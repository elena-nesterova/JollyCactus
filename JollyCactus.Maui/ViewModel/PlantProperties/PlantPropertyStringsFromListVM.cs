using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static SQLite.SQLite3;

namespace JollyCactus.Maui.ViewModel.PlantProperties
{
    public class PlantPropertyStringsFromListOneString
    {
        public string StringValue { get; set; } = "";
        //public bool IsActive { get; set;  } = false;
    }


    //public class PlantPropertyStringsFromListVM : PlantPropertyVM<ObservableCollection<string>>//<PlantPropertyStringsFromListOneString>>
    public class PlantPropertyStringsFromListVM : PlantPropertyVM<ObservableCollection<PlantPropertyStringsFromListOneString>>
    {
        //public ObservableCollection<string> AllPossibleValues { get; set; }
        public ObservableCollection<PlantPropertyStringsFromListOneString> AllPossibleValues { get; private set; }

        public ObservableCollection<object> SelectedObjects { get; set; }

        public ICommand SelectedValuesChangedCommand { get; set; }
    

        public PlantPropertyStringsFromListVM(string name, string description, string persistenceStringValue = "") :
            base(name, description)
        {           
            Value = new();
            AllPossibleValues = new();
            SelectedObjects = new();
            List<string> allValues = null;

            if (Model.PlantPropertiesValues.PlantPropertiesValuesDict.ContainsKey(Name))
                allValues = Model.PlantPropertiesValues.PlantPropertiesValuesDict[Name];

            Debug.Assert(allValues != null);

            foreach (string val in allValues)
            {
                //AllPossibleValues.Add(val);
                AllPossibleValues.Add(new PlantPropertyStringsFromListOneString() { StringValue = val});
            }

            if (!string.IsNullOrEmpty(persistenceStringValue))
            {
                var values = persistenceStringValue.Split(',', StringSplitOptions.TrimEntries);
                if (values != null)
                {
                    foreach (string val in values)
                    {
                        var toValue = AllPossibleValues.FirstOrDefault(x => x.StringValue.Equals(val), null);
                        if (toValue != null)
                        {
                            Value.Add(toValue);
                            SelectedObjects.Add(toValue);
                        }
                        //if(AllPossibleValues.Contains(val))
                        //    Value.Add(val);

                    }
                    /*foreach (var val in Value)
                    {
                        val.IsActive = values.Any(x => x.Equals(val.StringValue));                        
                    }*/
                }
            }

            SelectedValuesChangedCommand = new Command<object>(SelectedValuesChanged);
        }

        public override Model.PlantPropertyType PropertyType => Model.PlantPropertyType.PlantPropertyStringsFromList;
                

        public override void UpdateFrom(PlantPropertyVM property)
        {
            if (property is PlantPropertyStringsFromListVM propTyped)
            {
                Value.Clear();
                SelectedObjects.Clear();
                foreach (var val in propTyped.Value)
                {
                    var toValue = AllPossibleValues.FirstOrDefault(x => x.StringValue.Equals(val.StringValue), null);
                    if (toValue != null)
                    {
                        Value.Add(toValue);
                        SelectedObjects.Add(toValue);
                    }
                    //Value.Add(val);
                }
            }
            OnPropertyChanged(nameof(Value));
        }

        public override string AsPersistenceString()
        {
            //string persistenceString = string.Join(",", Value);
            string persistenceString = string.Join(",", Value.Select(x => x.StringValue).ToList());
            return persistenceString;
        }

        public override object Clone()
        {
            var clone = new PlantPropertyStringsFromListVM(Name, Description);

            
            foreach (var val in Value)
            {
                var toValue = clone.AllPossibleValues.FirstOrDefault(x => x.StringValue.Equals(val.StringValue), null);

                if (toValue != null)
                {
                    clone.Value.Add(toValue);
                    clone.SelectedObjects.Add(toValue);
                }
            }
            
            return clone;
        }

        public override string ToString()
        {
            //return string.Join(",", Value);
            return string.Join(",", Value.Select(x => x.StringValue).ToList());
        }

        public void SelectedValuesChanged(object obj)
        {
            Value.Clear();
            foreach (var val in SelectedObjects)
            {
                if (val is PlantPropertyStringsFromListOneString valTyped)
                    Value.Add(valTyped);
            }
            
            OnPropertyChanged(nameof(Value));
        }
    }
}
