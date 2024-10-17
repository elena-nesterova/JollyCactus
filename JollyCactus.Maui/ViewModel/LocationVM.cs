using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JollyCactus.Maui.ViewModel
{
    internal class LocationVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }
               
        public List<PlantVM> Plants { get; set; }
    }
}
