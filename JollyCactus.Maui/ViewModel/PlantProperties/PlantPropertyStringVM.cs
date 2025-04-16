namespace JollyCactus.Maui.ViewModel.PlantProperties
{
    public class PlantPropertyStringVM : PlantPropertyVM<string>
    {
        public PlantPropertyStringVM(string name, string description, string parentName, string persistenceStringValue = "") :
            base(name, description, parentName)
        {
            Value = persistenceStringValue;
            IsChanged = false;
        }

        public override Model.PlantPropertyType PropertyType => Model.PlantPropertyType.PlantPropertyString;


        public override void UpdateFrom(PlantPropertyVM property)
        {
            if (property is PlantPropertyStringVM propTyped)
            {
                Value = propTyped.Value;
            }
        }

        public override string AsPersistenceString()
        {
            return Value;
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }

    }
}
