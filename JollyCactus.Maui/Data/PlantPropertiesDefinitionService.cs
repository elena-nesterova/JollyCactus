using JollyCactus.Maui.Model;
using JollyCactus.Maui.Settings;
using Microsoft.EntityFrameworkCore;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JollyCactus.Maui.Data
{
    internal class PlantPropertiesDefinitionService : DbContext
    {

        private static List<Model.PlantPropertyGroup> _properties = new();        

        private static List<Model.PlantPropertyGroup> GetFreshList()
        {
            var groups = new List<Model.PlantPropertyGroup>();

            Model.PlantPropertyDefinition property;


            var group = new Model.PlantPropertyGroup
            {
                GroupNumber = 0,
                Name = "Plant Information"
            };

            group.Properties.Add(new Model.PlantPropertyDefinition
            {
                Name = PlantPropertiesValues.PlantPropertyName,
                Description = "Nickname of the plant",
                DefaultValue = "Plant",

                Type = Maui.Model.PlantPropertyType.PlantPropertyString
            });

            group.Properties.Add(new Model.PlantPropertyDefinition
            {
                Name = PlantPropertiesValues.PlantPropertyBotanicalName,
                Description = "Oficial name of the plant (latin)",

                Type = Maui.Model.PlantPropertyType.PlantPropertyString
            });

            group.Properties.Add(new Model.PlantPropertyDefinition
            {
                Name = PlantPropertiesValues.PlantPropertyFamilyName,
                Description = "family of the plant",

                Type = Maui.Model.PlantPropertyType.PlantPropertyString
            });

            group.Properties.Add(new Model.PlantPropertyDefinition
            {
                Name = PlantPropertiesValues.PlantPropertyAmountName,
                Description = "how many plants",
                DefaultValue = "1",
                Type = Maui.Model.PlantPropertyType.PlantPropertyNumber
            });

            group.Properties.Add(new Model.PlantPropertyDefinition
            {
                Name = PlantPropertiesValues.PlantPropertyStateName,
                Description = "how does the plant feel",

                Type = Maui.Model.PlantPropertyType.PlantPropertyOneFromList
            });

            group.Properties.Add(new Model.PlantPropertyDefinition
            {
                Name = PlantPropertiesValues.PlantPropertyPictureName,
                Description = "pictures of the plant",

                Type = Maui.Model.PlantPropertyType.PlantPropertyPicture
            });
            
            group.Properties.Add(new Model.PlantPropertyDefinition
            {
                Name = PlantPropertiesValues.PlantPropertyAdoptionDateName,
                Description = "the date when the plant starts living with you",

                Type = Maui.Model.PlantPropertyType.PlantPropertyDate
            });

            group.Properties.Add(new Model.PlantPropertyDefinition
            {
                Name = PlantPropertiesValues.PlantPropertyNotesName,
                Description = "anything you want to remember",


                Type = Maui.Model.PlantPropertyType.PlantPropertyString
            });

            groups.Add(group);

            //==========================

            group = new Model.PlantPropertyGroup
            {
                GroupNumber = 0,
                Name = "Plant Care"
            };

            //=== sunlight

            property = new Model.PlantPropertyDefinition
            {
                Name = PlantPropertiesValues.PlantPropertySunlightName,
                Description = "How much sun does the plant need",               

            };

            property.SubProperties = new();
            property.SubProperties.Add(new Model.PlantPropertyDefinition
            {
                Name = PlantPropertiesValues.PlantPropertySunlightName,
                Description = "How much sun does the plant need",
                SubName = "summer",

                Type = Maui.Model.PlantPropertyType.PlantPropertyStringsFromList
            });
            property.SubProperties.Add(new Model.PlantPropertyDefinition
            {
                Name = PlantPropertiesValues.PlantPropertySunlightName,
                Description = "How much sun does the plant need",
                SubName = "winter",

                Type = Maui.Model.PlantPropertyType.PlantPropertyStringsFromList
            });
            group.Properties.Add(property);

            //=== watering

            property = new Model.PlantPropertyDefinition
            {
                Name = PlantPropertiesValues.PlantPropertyWateringName,
                Description = "How much sun does the plant need",

            };

            property.SubProperties = new();
            property.SubProperties.Add(new Model.PlantPropertyDefinition
            {
                Name = PlantPropertiesValues.PlantPropertyWateringName,
                Description = "How much water does the plant need",
                SubName = "summer",

                Type = Maui.Model.PlantPropertyType.PlantPropertyOneFromList
            });
            property.SubProperties.Add(new Model.PlantPropertyDefinition
            {
                Name = PlantPropertiesValues.PlantPropertyWateringName,
                Description = "How much water does the plant need",
                SubName = "winter",

                Type = Maui.Model.PlantPropertyType.PlantPropertyOneFromList
            });
            group.Properties.Add(property);

            groups.Add(group);

            return groups;
        }



        public static async Task<List<Model.PlantPropertyGroup>> GetPlantPropertiesAsync()
        {
            //await Init();
            if (_properties.Count > 0)
                return _properties;

            //await GetPropertiesFromDbAsync();
            //if (_properties.Count == 0)
                GetPropertiesDefault();

            return _properties;
        }

        private static void GetPropertiesDefault()
        {
            _properties = GetFreshList();
        }

        /*private async Task GetPropertiesFromDbAsync()
        {
            SQLiteAsyncConnection _connection;
            _connection = new SQLiteAsyncConnection(JollyCactusPaths.GetPropertiesDatabasePath());

            await _connection.CreateTableAsync<Model.PlantPropertyGroup>();
            _properties = await _connection.Table<Model.PlantPropertyGroup>().ToListAsync();            
        }*/
    }
}
