
namespace JollyCactus.Maui.Model
{
    public static class PlantPropertiesValues
    {
        public static readonly string PlantPropertyName = "name";

        public static readonly string PlantPropertyBotanicalName = "botanical name";

        public static readonly string PlantPropertyFamilyName = "family";

        public static readonly string PlantPropertyNotesName = "note";

        public static readonly string PlantPropertyPictureName = "picture";
                
        public static readonly string PlantPropertyAdoptionDateName = "adoption date";

        public static readonly string PlantPropertyStateName = "state";

        public static readonly string PlantPropertySunlightName = "sunlight";

        public static readonly string PlantPropertyWateringName = "watering";

        public static readonly string PlantPropertyAmountName = "amount";

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
            },

            [PlantPropertyWateringName] = new()
            {
                "airplant",
                "lowwateruse",
                "moderatewateruse",
                "highwateruse",
                "waterplant"
            },
        };

       

        

    }
}