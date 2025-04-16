using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace JollyCactus.Maui.ViewModel.PlantProperties
{
    public class PlantPropertyStringsFromListOneString
    {
        public string StringValue { get; set; } = string.Empty;
        //public bool IsActive { get; set;  } = false;
    }


    public class PlantPropertyStringsFromListVM : PlantPropertyVM<ObservableCollection<PlantPropertyStringsFromListOneString>>
    {
        public ObservableCollection<PlantPropertyStringsFromListOneString> AllPossibleValues { get; private set; }

        public ObservableCollection<object> SelectedObjects { get; set; }

        public ICommand SelectedValuesChangedCommand { get; set; }
    

        public PlantPropertyStringsFromListVM(string name, string description, string parentName, string persistenceStringValue = "") :
            base(name, description, parentName)
        {           
            Value = new();
            AllPossibleValues = new();
            SelectedObjects = new();
            List<string>? allValues = null;

            if (Model.PlantPropertiesValues.PlantPropertiesValuesDict.ContainsKey(Name))
                allValues = Model.PlantPropertiesValues.PlantPropertiesValuesDict[Name];

            if (allValues == null)
                if (Model.PlantPropertiesValues.PlantPropertiesValuesDict.ContainsKey(ParentName))
                    allValues = Model.PlantPropertiesValues.PlantPropertiesValuesDict[ParentName];

            //Debug.Assert(allValues != null);

            if (allValues != null)

            {
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
                                SelectedObjects.Add((object)toValue);
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
            IsChanged = false;
        }

        public override Model.PlantPropertyType PropertyType => Model.PlantPropertyType.PlantPropertyStringsFromList;
                

        public override void UpdateFrom(PlantPropertyVM property)
        {
            Debug.WriteLine("JC: PlantPropertyStringsFromListOneString.UpdateFrom - " + ToString());

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
            Debug.WriteLine("JC: PlantPropertyStringsFromListOneString.Clone - " + ToString());

            var clone = new PlantPropertyStringsFromListVM(Name, Description, ParentName);
            
            foreach (var val in Value)
            {
                var toValue = clone.AllPossibleValues.FirstOrDefault(x => x.StringValue.Equals(val.StringValue), null);

                if (toValue != null)
                {
                    clone.Value.Add(toValue);
                    clone.SelectedObjects.Add((object)toValue);
                }
            }

            Debug.WriteLine("JC: PlantPropertyStringsFromListOneString.Clone end - " + clone.ToString());
            return clone;
        }

        public override string ToString()
        {
            if (Value.Count == 0)
                return string.Empty;

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
            IsChanged = true;
            OnPropertyChanged(nameof(Value));
        }
    }
}
