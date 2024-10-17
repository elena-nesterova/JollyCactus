namespace JollyCactus.Maui.ViewModel.PlantProperties
{
    public class PlantPropertyDateVM : PlantPropertyVM<DateTime>
    {
        public PlantPropertyDateVM(string name, string description, string persistenceStringValue = "") :
            base(name, description)
        {
            Value = string.IsNullOrEmpty(persistenceStringValue) ? DateTime.Now : DateTime.Parse(persistenceStringValue);
        }

        public override Model.PlantPropertyType PropertyType => Model.PlantPropertyType.PlantPropertyDate;


        public override void UpdateFrom(PlantPropertyVM property)
        {
            if (property is PlantPropertyDateVM propTyped)
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
