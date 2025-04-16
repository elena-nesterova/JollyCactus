using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JollyCactus.Maui.Views.PlantPropertyViews.TemplateSelectors
{
    internal class PropertyTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PropertyTemplateString { get; set; }
        public DataTemplate PropertyTemplateDate { get; set; }

        public DataTemplate PropertyTemplatePicture { get; set; }

        public DataTemplate PropertyTemplateStringsFromListSunlight { get; set; }
        public DataTemplate PropertyTemplateStringsFromListDefault { get; set; }

        public DataTemplate PropertyTemplateOneFromListState { get; set; }

        public DataTemplate PropertyTemplateOneFromListWatering { get; set; }

        public DataTemplate PropertyTemplateOneFromListDefault { get; set; }

        public DataTemplate PropertyTemplateNumber { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var property = item as ViewModel.PlantProperties.PlantPropertyVM;
            if (property != null)
            {
                switch (property.PropertyType)
                {
                    case Model.PlantPropertyType.PlantPropertyString:
                        return PropertyTemplateString;

                    case Model.PlantPropertyType.PlantPropertyDate:
                        return PropertyTemplateDate;

                    case Model.PlantPropertyType.PlantPropertyPicture:
                        return PropertyTemplatePicture;

                    case Model.PlantPropertyType.PlantPropertyNumber:
                        return PropertyTemplateNumber;

                    case Model.PlantPropertyType.PlantPropertyStringsFromList:
                        switch (property.ParentName)
                        {
                            case string str when str.Equals(Model.PlantPropertiesValues.PlantPropertySunlightName, StringComparison.InvariantCultureIgnoreCase):
                                return PropertyTemplateStringsFromListSunlight;
                        }
                        return PropertyTemplateStringsFromListDefault;

                    case Model.PlantPropertyType.PlantPropertyOneFromList:
                        switch (property.ParentName)
                        {
                            case string str when str.Equals(Model.PlantPropertiesValues.PlantPropertyStateName, StringComparison.InvariantCultureIgnoreCase):
                                return PropertyTemplateOneFromListState;

                            case string str when str.Equals(Model.PlantPropertiesValues.PlantPropertyWateringName, StringComparison.InvariantCultureIgnoreCase):
                                return PropertyTemplateOneFromListWatering;
                        }
                        return PropertyTemplateOneFromListDefault;

                }
            }

            return PropertyTemplateString;
        }
    }
}
