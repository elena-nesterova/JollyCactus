using System.Collections.ObjectModel;
using System.Diagnostics;

namespace JollyCactus.Maui.ViewModel.PlantProperties
{
    public class PlantPropertyOneFromListVM : PlantPropertyVM<string>
    {
        public ObservableCollection<string> AllPossibleValues { get; private set; }
       
        public PlantPropertyOneFromListVM(string name, string description, string parentName, string persistenceStringValue = "") :
            base(name, description, parentName)
        {
            AllPossibleValues = new();
            
            List<string> allValues = null;
            
            if (Model.PlantPropertiesValues.PlantPropertiesValuesDict.ContainsKey(Name))
                allValues = Model.PlantPropertiesValues.PlantPropertiesValuesDict[Name];

            if (allValues == null)
                if (Model.PlantPropertiesValues.PlantPropertiesValuesDict.ContainsKey(ParentName))
                    allValues = Model.PlantPropertiesValues.PlantPropertiesValuesDict[ParentName];

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
                Value = AllPossibleValues[(AllPossibleValues.Count - 1)/2];

            IsChanged = false;
            
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
            var clone = new PlantPropertyOneFromListVM(Name, Description, ParentName, Value);

            return clone;
        }

        public override string ToString()
        {
            return Value;
        }       
    }
}
