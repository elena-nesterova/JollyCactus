
using System.Diagnostics;

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
                        string.IsNullOrEmpty(propertyDefinition.SubName) ? propertyDefinition.Name : propertyDefinition.SubName, 
                        propertyDefinition.Description,
                        propertyDefinition.Name,
                        (modelProperty == null) ? "" : modelProperty.DBValue);
                    break;
                case Model.PlantPropertyType.PlantPropertyString:
                    plantPropertyVM = new PlantPropertyStringVM(
                        string.IsNullOrEmpty(propertyDefinition.SubName) ? propertyDefinition.Name : propertyDefinition.SubName, 
                        propertyDefinition.Description,
                        propertyDefinition.Name,
                        (modelProperty == null) ? propertyDefinition.DefaultValue : modelProperty.DBValue);
                    break;
                case Model.PlantPropertyType.PlantPropertyPicture:
                    plantPropertyVM = new PlantPropertyPictureVM(
                        string.IsNullOrEmpty(propertyDefinition.SubName) ? propertyDefinition.Name : propertyDefinition.SubName, 
                        propertyDefinition.Description,
                        propertyDefinition.Name,
                        (modelProperty == null) ? propertyDefinition.DefaultValue : modelProperty.DBValue);
                    break;
                case Model.PlantPropertyType.PlantPropertyStringsFromList:
                    plantPropertyVM = new PlantPropertyStringsFromListVM(
                        string.IsNullOrEmpty(propertyDefinition.SubName) ? propertyDefinition.Name : propertyDefinition.SubName, 
                        propertyDefinition.Description,
                        propertyDefinition.Name,
                        (modelProperty == null) ? "" : modelProperty.DBValue);
                    break;
                case Model.PlantPropertyType.PlantPropertyOneFromList:
                    plantPropertyVM = new PlantPropertyOneFromListVM(
                        string.IsNullOrEmpty(propertyDefinition.SubName) ? propertyDefinition.Name : propertyDefinition.SubName, 
                        propertyDefinition.Description,
                        propertyDefinition.Name,
                        (modelProperty == null) ? "" : modelProperty.DBValue);
                    break;
                case Model.PlantPropertyType.PlantPropertyNumber:
                    plantPropertyVM = new PlantPropertyNumberVM(
                        string.IsNullOrEmpty(propertyDefinition.SubName) ? propertyDefinition.Name : propertyDefinition.SubName,
                        propertyDefinition.Description,
                        propertyDefinition.Name,
                        (modelProperty == null) ? propertyDefinition.DefaultValue : modelProperty.DBValue);
                    break;
            }
            Debug.Assert(plantPropertyVM != null);
            return plantPropertyVM;
        }

        public static PlantPropertyCompositeVM GetPropertyCompositeVM(Model.PlantPropertyDefinition propertyDefinition)
        {
            PlantPropertyCompositeVM plantPropVM = new (propertyDefinition.Name, propertyDefinition.Description, propertyDefinition.Name);
            return plantPropVM;
        }
    }
}
