
using System.Reflection.Metadata;

namespace JollyCactus.Maui.ViewModel.PlantProperties
{
    public class PlantPropertyVMFactory
    {
        public static PlantPropertyVM? GetPropertyVM(
            Model.PlantPropertyDefinition propertyDefinition, Model.PlantProperty? modelProperty)
        {
            PlantPropertyVM? plantPropertyVM = null;

            switch (propertyDefinition.Type)
            {
                case Model.PlantPropertyType.PlantPropertyDate:
                    plantPropertyVM = new PlantPropertyDateVM(
                        propertyDefinition.Name, propertyDefinition.Description,
                        (modelProperty == null) ? "" : modelProperty.DBValue);
                    break;
                case Model.PlantPropertyType.PlantPropertyString:
                case Model.PlantPropertyType.PlantPropertyPicture:
                    plantPropertyVM = new PlantPropertyStringVM(
                        propertyDefinition.Name, propertyDefinition.Description,
                        (modelProperty == null) ? propertyDefinition.DefaultValue : modelProperty.DBValue);
                    break;
            }

            return plantPropertyVM;
        }

        public static PlantPropertyCompositeVM GetPropertyCompositeVM(Model.PlantPropertyDefinition propertyDefinition)
        {
            PlantPropertyCompositeVM plantPropVM = new PlantPropertyCompositeVM(
                        propertyDefinition.Name, propertyDefinition.Description);
            return plantPropVM;
        }
    }
}
