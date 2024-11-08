
namespace JollyCactus.Maui.Model
{
    public static class PlantPropertiesValues
    {
        public static readonly string PlantPropertyName = "name";

        public static readonly string PlantPropertyPictureName = "picture";

        public static readonly string PlantPropertyGalleryName = "gallery";

        public static readonly string PlantPropertyStateName = "state";

        public static readonly string PlantPropertySunlightName = "sunlight";

        public static readonly Dictionary<string, List<string>> PlantPropertiesValuesDict = new()
        {
            [PlantPropertySunlightName] = new() 
            {
                "fullshade",
                "dappledsun",
                "partialshade",
                "partialsun",
                "fullsun"
            },

            [PlantPropertyStateName] = new()
            {
                "almostdead",
                "bad",
                "ok",
                "fine",
                "excellent"
            }
        };

       

        

    }
}