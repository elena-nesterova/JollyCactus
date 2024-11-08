using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JollyCactus.Maui.ViewModel.PlantProperties
{
    public class PlantPropertyPictureVM : PlantPropertyVM<string?>
    {
        public PlantPropertyPictureVM(string name, string description, string persistenceStringValue = "") :
            base(name, description)
        {
            Value = null;
            if (!string.IsNullOrEmpty(persistenceStringValue))
                Value = persistenceStringValue;
        }

        public override Model.PlantPropertyType PropertyType => Model.PlantPropertyType.PlantPropertyPicture;


        public override void UpdateFrom(PlantPropertyVM property)
        {
            if (property is PlantPropertyPictureVM propTyped)
            {
                Value = propTyped.Value;
            }
        }

        public override string AsPersistenceString()
        {
            if (!string.IsNullOrEmpty(Value)) 
                return Value;
            return "";
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }

    }
   
}
