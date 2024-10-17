﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JollyCactus.Maui.Views.PropertyViews
{
    internal class PropertyViewTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PropertyTemplateString { get; set; }
        public DataTemplate PropertyTemplateDate { get; set; }
        //public DataTemplate PropertyTemplatePic { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {

/* Unmerged change from project 'JollyCactus.Maui (net8.0-android)'
Before:
            var property = item as ViewModel.PlantPropertyVM;
            if (property != null)
After:
            var property = item as PlantPropertyVM;
            if (property != null)
*/
            var property = item as ViewModel.PlantProperties.PlantPropertyVM;
            if (property != null)
            {
                switch (property.PropertyType)
                {
                    case Model.PlantPropertyType.PlantPropertyString:
                        return this.PropertyTemplateString;
                        
                    case Model.PlantPropertyType.PlantPropertyDate: 
                        return this.PropertyTemplateDate;
                }
            }

            return this.PropertyTemplateString;
        }
    }
}
