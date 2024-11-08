using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JollyCactus.Maui.Views.PlantPropertyViews.TemplateSelectors
{
    class StateTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StateTemplate0 { get; set; }
        public DataTemplate StateTemplate1 { get; set; }
        public DataTemplate StateTemplate2 { get; set; }
        public DataTemplate StateTemplate3 { get; set; }
        public DataTemplate StateTemplate4 { get; set; }


        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var value = item as string;

            Debug.Assert(value != null);

            var vm = container.BindingContext as ViewModel.PlantProperties.PlantPropertyOneFromListVM;
            if (vm != null)
            {
                var allValues = vm.AllPossibleValues;
                if ((value != null) && (allValues.Count >= 5))
                {

                    switch (value)
                    {
                        case string str when str.Equals(allValues[0], StringComparison.InvariantCultureIgnoreCase):
                            return StateTemplate0;
                        case string str when str.Equals(allValues[1], StringComparison.InvariantCultureIgnoreCase):
                            return StateTemplate1;
                        case string str when str.Equals(allValues[2], StringComparison.InvariantCultureIgnoreCase):
                            return StateTemplate2;
                        case string str when str.Equals(allValues[3], StringComparison.InvariantCultureIgnoreCase):
                            return StateTemplate3;
                        case string str when str.Equals(allValues[4], StringComparison.InvariantCultureIgnoreCase):
                            return StateTemplate4;
                    }
                }
            }


            return StateTemplate4;
        }
    }
}
