using JollyCactus.Maui.ViewModel.PlantProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JollyCactus.Maui.Views.PropertyViews.Adaprers
{
    internal class PlantPropertyFlagsAdapter
    {
        public static PlantPropertyVM? GetPropertyToView(PlantPropertyVM propertyVM)
        {
            PlantPropertyCompositeVM? propToView = null;

            if (propertyVM is PlantPropertyCompositeVM composite)
            {
                propToView = new PlantPropertyCompositeVM(composite.Name, composite.Description);

                foreach (PlantPropertyVM subProp in composite.SubProperties)
                {
                    switch (subProp)
                    {
                        case PlantPropertyFlagsVM propFlags:
                            // TODO
                            break;

                        default:
                            propToView.SubProperties.Add((PlantPropertyVM)subProp.Clone());
                            break;

                    }
                }
            }

            return (PlantPropertyVM?)propToView;
        }
    }
}
