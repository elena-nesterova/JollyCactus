using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JollyCactus.Maui.Views.PlantPropertyViews.TemplateSelectors
{
    internal class LightTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LightTemplate0 { get; set; }
        public DataTemplate LightTemplate1 { get; set; }
        public DataTemplate LightTemplate2 { get; set; }
        public DataTemplate LightTemplate3 { get; set; }
        public DataTemplate LightTemplate4 { get; set; }

       
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            //var value = item as string;//ViewModel.PlantProperties.PlantPropertyStringsFromListVM;//ViewModel.PlantProperties.PlantPropertyStringsFromListOneString;
            var value = item as ViewModel.PlantProperties.PlantPropertyStringsFromListOneString;

            Debug.Assert(value != null);

            var vm = container.BindingContext as ViewModel.PlantProperties.PlantPropertyStringsFromListVM;
            if (vm != null)
            {
                var allValues = vm.AllPossibleValues;
                //var allValues =Model.PlantPropertiesValues.PlantPropertiesValuesDict[Model.PlantPropertiesValues.PlantPropertySunlightName];
                if (value != null)
                {

                    switch (value.StringValue)
                    {
                        case string str when str.Equals(allValues[0].StringValue, StringComparison.InvariantCultureIgnoreCase):
                            return LightTemplate0;
                        case string str when str.Equals(allValues[1].StringValue, StringComparison.InvariantCultureIgnoreCase):
                            return LightTemplate1;
                        case string str when str.Equals(allValues[2].StringValue, StringComparison.InvariantCultureIgnoreCase):
                            return LightTemplate2;
                        case string str when str.Equals(allValues[3].StringValue, StringComparison.InvariantCultureIgnoreCase):
                            return LightTemplate3;
                        case string str when str.Equals(allValues[4].StringValue, StringComparison.InvariantCultureIgnoreCase):
                            return LightTemplate4;
                    }
                }
            }
            

            return LightTemplate2;
        }
    }
}
