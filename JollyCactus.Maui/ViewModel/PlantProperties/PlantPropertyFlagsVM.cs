using JollyCactus.Maui.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JollyCactus.Maui.ViewModel.PlantProperties
{
    internal class PlantPropertyFlagsVM : PlantPropertyVM<int>
    {
        public override PlantPropertyType PropertyType => PlantPropertyType.PlantPropertyFlags;

        public PlantPropertyFlagsVM(string name, string description, string persistenceStringValue = "") :
           base(name, description)
        {
            Value = string.IsNullOrEmpty(persistenceStringValue) ? 0 : int.Parse(persistenceStringValue);
        }


        public override string AsPersistenceString()
        {
            return Value.ToString();
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }

        public override void UpdateFrom(PlantPropertyVM property)
        {
            if (property is PlantPropertyFlagsVM propTyped)
            {
                Value = propTyped.Value;
            }
        }
    }
}
