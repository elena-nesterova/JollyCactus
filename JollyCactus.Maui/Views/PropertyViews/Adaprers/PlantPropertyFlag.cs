using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JollyCactus.Maui.Views.PropertyViews.Adaprers
{
    public class PlantPropertyFlag
    {
        public int FlagValue { get; }

        public string Description { get; } = "";

        public string Resource { get; } = "";

        public bool IsEnabled { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }
}
