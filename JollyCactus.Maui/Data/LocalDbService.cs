using Microsoft.EntityFrameworkCore;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

namespace JollyCactus.Maui.Data
{
    public class LocalDbService: DbContext
    {        
        private readonly SQLiteAsyncConnection _connection;

        //private List<Model.Plant> _plants = new();


        private List<Model.PlantPropertyGroup> _propertyGroupsList = new();
        private Dictionary<string, Model.PlantPropertyDefinition> _propertiesList = new();

        // public DbSet<Location.Location> Locations { get; set; }
        //public DbSet<Plant.Plant> Plants { get; set; }

        public LocalDbService(string dbPath)
        {
            _connection = new SQLiteAsyncConnection(dbPath);
            _connection.CreateTableAsync<Model.Location>().Wait();
            _connection.CreateTableAsync<Model.Plant>().Wait();
            _connection.CreateTableAsync<Model.PlantProperty>().Wait();
            _connection.CreateTableAsync<Model.PlantPropertyGroup>().Wait();
            _connection.CreateTableAsync<Model.PlantPropertyDefinition>().Wait();

        }

        public async Task ClearAll()
        {
            await _connection.DeleteAllAsync<Model.Location>();
            await _connection.DeleteAllAsync<Model.Plant>();
            await _connection.DeleteAllAsync<Model.PlantProperty>();
        }

        #region location

        public async Task<List<Model.Location>> GetLocationsAsync()
        {
            return await _connection.GetAllWithChildrenAsync<Model.Location>();
           // return await _connection.Table<Model.Location>().ToListAsync();
        }

        public Task<Model.Location> GetLocationAsync(int id)
        {
            return _connection.GetWithChildrenAsync<Model.Location>(id);
            //await Init();
            //return await _connection.Table<Model.Location>().
            //    Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task/*<int>*/ SaveLocationAsync(Model.Location item)
        {
            return _connection.InsertOrReplaceWithChildrenAsync(item);
            //await Init();
            //if (item.Id != 0)
            //    return await _connection.UpdateAsync(item);
            //else
            //    return await _connection.InsertAsync(item);
        }

        public Task DeleteLocationAsync(Model.Location item)
        {
            //await Init();
            return _connection.DeleteAsync(item, recursive: true);
        }

        #endregion //location

        #region plant
        public Task<List<Model.Plant>> GetPlantsAsync()
        {
            return _connection.GetAllWithChildrenAsync<Model.Plant>();
            
            /*if (_plants.Count > 0)
                return _plants;

            _plants = await _connection.Table<Model.Plant>().ToListAsync();
            return _plants;*/
        }
        /*public async Task<List<Model.Plant>> GetPlantsInLocationAsync(Model.Location location)
        {            
            return await _connection.Table<Model.Plant>().
                Where(i => i.LocationId == location.Id).ToListAsync();
        }*/

        /*public Task<Model.Plant> GetPlantAsync(int id)
        {
            return _connection.GetWithChildrenAsync<Model.Plant>(id);
            //await Init();
            //return await _connection.Table<Model.Plant>().
            //    Where(i => i.Id == id).FirstOrDefaultAsync();
        }*/

        //public Task<List<Model.PlantProperty>> GetPlantsPropertiesAsync(Model.Plant plant)
        //{
        //   return _connection.GetAllWithChildrenAsync<Model.PlantProperty>(plant);
        //}

        public async Task AddPlantAsync(Model.Location location, Model.Plant plant)
        {
            location.Plants.Add(plant);            
            await _connection.InsertOrReplaceWithChildrenAsync(plant, recursive: true);
            await SaveLocationAsync(location);
            //await Init();
            //if (item.Id != 0)
            //    return await _connection.UpdateAsync(item);
            // else
            //     return await _connection.InsertAsync(item);
        }

        public Task SavePlantAsync(Model.Plant item)
        {            
            return _connection.InsertOrReplaceWithChildrenAsync(item, recursive: true);
            //await Init();
            //if (item.Id != 0)
            //    return await _connection.UpdateAsync(item);
           // else
           //     return await _connection.InsertAsync(item);
        }

        public Task DeletePlantAsync(Model.Plant item)
        {
            //TODO: does it delete from location also?
            return _connection.DeleteAsync(item, recursive: true);
        }

        #endregion //plant

        #region properties

        public async Task<Dictionary<string, Model.PlantPropertyDefinition>> GetPlantPropertiesList()
        {
            
            if (_propertiesList.Count > 0)
                return _propertiesList;

            var list = await _connection.GetAllWithChildrenAsync<Model.PlantPropertyDefinition>();

            if (list != null)
            {
                foreach ( var item in list)
                {
                    _propertiesList.Add(item.Name, item);
                }
            }
            return _propertiesList;
        }

        public async Task<List<Model.PlantPropertyGroup>> GetPlantPropertyGroupsListAsync()
        {

            if (_propertyGroupsList.Count > 0)
                return _propertyGroupsList;

            _propertyGroupsList = await PlantPropertiesDefinitionService.GetPlantPropertiesAsync();
                //await _connection.GetAllWithChildrenAsync<Model.PlantPropertyGroup>();
            
            return _propertyGroupsList;
        }

        #endregion //properties
    }
}
