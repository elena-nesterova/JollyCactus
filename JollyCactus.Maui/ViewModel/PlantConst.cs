using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JollyCactus.Maui.ViewModel
{
    public static class PlantConst
    {
        [Flags]
        public enum SunlightFlags
        {
            None        = 0b_0000_0000,     // 0
            Lamplight   = 0b_0000_0001,     // 1
            Shadow      = 0b_0000_0010,     // 2
            SunShadow   = 0b_0000_0100,     // 4
            Sun         = 0b_0000_1000,     // 8
            DirectLight = 0b_0001_0000,     // 16
        }

        [Flags]
        public enum WateringFlags
        {
            None            = 0b_0000_0000,     // 0
            DryTolerate     = 0b_0000_0001,     // 1
            LittleWater     = 0b_0000_0010,     // 2
            AverageWater    = 0b_0000_0100,     // 4
            LoveWater       = 0b_0000_1000,     // 8
            WaterPlant      = 0b_0001_0000,     // 16
        }

        [Flags]
        public enum HumidityFlags
        {
            None        = 0b_0000_0000,     // 0
            DryAir      = 0b_0000_0001,     // 1
            Average     = 0b_0000_0010,     // 2
            HumidAir    = 0b_0000_0100,     // 4            
        }

    }
}
