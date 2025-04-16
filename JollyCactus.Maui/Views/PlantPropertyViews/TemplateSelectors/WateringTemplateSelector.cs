using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JollyCactus.Maui.Views.PlantPropertyViews.TemplateSelectors
{
    class WateringTemplateSelector : DataTemplateSelector
    {
        public DataTemplate WateringTemplate0 { get; set; }
        public DataTemplate WateringTemplate1 { get; set; }
        public DataTemplate WateringTemplate2 { get; set; }
        public DataTemplate WateringTemplate3 { get; set; }
        public DataTemplate WateringTemplate4 { get; set; }


        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var value = item as string;            

            var vm = container.BindingContext as ViewModel.PlantProperties.PlantPropertyOneFromListVM;
            if (vm != null)
            {
                var allValues = vm.AllPossibleValues;
                if ((value != null) && (allValues.Count >= 5))
                {

                    switch (value)
                    {
                        case string str when str.Equals(allValues[0], StringComparison.InvariantCultureIgnoreCase):
                            return WateringTemplate0;
                        case string str when str.Equals(allValues[1], StringComparison.InvariantCultureIgnoreCase):
                            return WateringTemplate1;
                        case string str when str.Equals(allValues[2], StringComparison.InvariantCultureIgnoreCase):
                            return WateringTemplate2;
                        case string str when str.Equals(allValues[3], StringComparison.InvariantCultureIgnoreCase):
                            return WateringTemplate3;
                        case string str when str.Equals(allValues[4], StringComparison.InvariantCultureIgnoreCase):
                            return WateringTemplate4;
                    }
                }
            }


            return WateringTemplate4;
        }
    }
}
