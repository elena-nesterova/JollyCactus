
using System.Diagnostics;
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
                    plantPropertyVM = new PlantPropertyStringVM(
                        propertyDefinition.Name, propertyDefinition.Description,
                        (modelProperty == null) ? propertyDefinition.DefaultValue : modelProperty.DBValue);
                    break;
                case Model.PlantPropertyType.PlantPropertyPicture:
                    plantPropertyVM = new PlantPropertyPictureVM(
                        propertyDefinition.Name, propertyDefinition.Description,
                        (modelProperty == null) ? propertyDefinition.DefaultValue : modelProperty.DBValue);
                    break;
                case Model.PlantPropertyType.PlantPropertyStringsFromList:
                    plantPropertyVM = new PlantPropertyStringsFromListVM(
                        propertyDefinition.Name, propertyDefinition.Description,
                        (modelProperty == null) ? "" : modelProperty.DBValue);
                    break;
                case Model.PlantPropertyType.PlantPropertyOneFromList:
                    plantPropertyVM = new PlantPropertyOneFromListVM(
                        propertyDefinition.Name, propertyDefinition.Description,
                        (modelProperty == null) ? "" : modelProperty.DBValue);
                    break;
            }
            Debug.Assert(plantPropertyVM != null);
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
