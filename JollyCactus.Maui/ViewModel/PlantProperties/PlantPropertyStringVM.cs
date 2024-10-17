namespace JollyCactus.Maui.ViewModel.PlantProperties
{
    public class PlantPropertyStringVM : PlantPropertyVM<string>
    {
        public PlantPropertyStringVM(string name, string description, string persistenceStringValue = "") :
            base(name, description)
        {
            Value = persistenceStringValue;
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
