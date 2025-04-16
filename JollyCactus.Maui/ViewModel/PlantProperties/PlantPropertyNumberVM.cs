
namespace JollyCactus.Maui.ViewModel.PlantProperties
{
    class PlantPropertyNumberVM : PlantPropertyVM<uint>
    {
        public PlantPropertyNumberVM(string name, string description, string parentName, string persistenceStringValue = "") :
            base(name, description, parentName)
        {
            Value = 1;
            if (!uint.TryParse(persistenceStringValue, out var value)) 
                Value = value;
            IsChanged = false;
        }

        public override Model.PlantPropertyType PropertyType => Model.PlantPropertyType.PlantPropertyNumber;


        public override void UpdateFrom(PlantPropertyVM property)
        {
            if (property is PlantPropertyNumberVM propTyped)
            {
                Value = propTyped.Value;
            }

        }

        public override string AsPersistenceString()
        {
            return Value.ToString();
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
