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

            var group = new Model.PlantPropertyGroup
            {
                GroupNumber = 0,
                Name = "Plant Information"
            };

            group.Properties.Add(new Model.PlantPropertyDefinition
            {
                Name = "name",
                Description = "Nickname of the plant",
                DefaultValue = "Plant",

                Type = Maui.Model.PlantPropertyType.PlantPropertyString
            });

            group.Properties.Add(new Model.PlantPropertyDefinition
            {
                Name = "botanical name",
                Description = "Oficial name of the plant (latin)",

                Type = Maui.Model.PlantPropertyType.PlantPropertyString
            });

            group.Properties.Add(new Model.PlantPropertyDefinition
            {
                Name = "family",
                Description = "family of the plant",

                Type = Maui.Model.PlantPropertyType.PlantPropertyString
            });

            group.Properties.Add(new Model.PlantPropertyDefinition
            {
                Name = "state",
                Description = "how does the plant feel",

                Type = Maui.Model.PlantPropertyType.PlantPropertyOneFromList
            });

            group.Properties.Add(new Model.PlantPropertyDefinition
            {
                Name = "picture",
                Description = "",

                Type = Maui.Model.PlantPropertyType.PlantPropertyPicture
            });

            group.Properties.Add(new Model.PlantPropertyDefinition
            {
                Name = "gallery",
                Description = "pictures of the plant",

                Type = Maui.Model.PlantPropertyType.PlantPropertyPicture
            });
            

            group.Properties.Add(new Model.PlantPropertyDefinition
            {
                Name = "adoption date",
                Description = "the date when the plant starts living with you",

                Type = Maui.Model.PlantPropertyType.PlantPropertyDate
            });

            group.Properties.Add(new Model.PlantPropertyDefinition
            {
                Name = "note",
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

            group.Properties.Add(new Model.PlantPropertyDefinition
            {
                Name = "sunlight",
                Description = "How much sun does the plant need",

                Type = Maui.Model.PlantPropertyType.PlantPropertyStringsFromList
            });
            
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

        private async Task GetPropertiesFromDbAsync()
        {
            SQLiteAsyncConnection _connection;
            _connection = new SQLiteAsyncConnection(JollyCactusPaths.GetPropertiesDatabasePath());

            await _connection.CreateTableAsync<Model.PlantPropertyGroup>();
            _properties = await _connection.Table<Model.PlantPropertyGroup>().ToListAsync();            
        }
    }
}
