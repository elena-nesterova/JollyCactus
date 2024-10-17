using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JollyCactus.Maui.Data
{
    internal interface IPlantPropertiesService
    {
        ObservableCollection<View> GetPropertyViews();
    }
}
