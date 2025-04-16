using Microsoft.EntityFrameworkCore;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System.Diagnostics;

namespace JollyCactus.Maui.Data
{
    public class LocalDbService: DbContext
    {        
        private SQLiteAsyncConnection _connection;
                
        private List<Model.PlantPropertyGroup> _propertyGroupsList = new();
        private Dictionary<string, Model.PlantPropertyDefinition> _propertiesList = new();

        private string _dbFullDbFileName;

        public bool IsConnected => _connection != null;

        public LocalDbService(string dbFileFullName)
        {
            _dbFullDbFileName = dbFileFullName;

            Connect();
        }

        private void Connect()
        {
            _connection = new SQLiteAsyncConnection(_dbFullDbFileName, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            _connection.CreateTableAsync<Model.Settings>().Wait();
            _connection.CreateTableAsync<Model.Location>().Wait();
            _connection.CreateTableAsync<Model.Plant>().Wait();
            _connection.CreateTableAsync<Model.PlantProperty>().Wait();
            _connection.CreateTableAsync<Model.PlantPropertyGroup>().Wait();
            _connection.CreateTableAsync<Model.PlantPropertyDefinition>().Wait();         

        }

        public async Task ClearAll()
        {
            if (_connection == null)
                return;
            await _connection.DeleteAllAsync<Model.Location>();
            await _connection.DeleteAllAsync<Model.Plant>();
            await _connection.DeleteAllAsync<Model.PlantProperty>();
            await _connection.DeleteAllAsync<Model.PlantPropertyGroup>();
            await _connection.DeleteAllAsync<Model.Settings>();
        }

        public async Task CloseDatabase()
        {            
            if (_connection != null)
            {
                var closingCon = _connection;
                _connection = null;
                await closingCon.CloseAsync();
            }         
        }

        public async Task ChangeDatabase(string dbFullDbFileName)
        {
            if (dbFullDbFileName == _dbFullDbFileName)
                return;

            await CloseDatabase();

            if (!string.IsNullOrEmpty(dbFullDbFileName))
            {
                _dbFullDbFileName = dbFullDbFileName;
                Connect();
            }

        }     

        #region location

        public async Task<List<Model.Location>> GetLocationsAsync()
        {
            if (_connection != null)
                return await _connection.GetAllWithChildrenAsync<Model.Location>();         
            return new List<Model.Location>();
        }

        private Task<Model.Location> GetLocationAsync(int id)
        {
            Debug.Assert(_connection != null, "JC: GetLocationAsync: _connection is null");
            return _connection.GetWithChildrenAsync<Model.Location>(id);           
        }

        public Task SaveLocationAsync(Model.Location item)
        {
            return _connection.InsertOrReplaceWithChildrenAsync(item);
        }

        public Task DeleteLocationAsync(Model.Location item)
        {
            Debug.Assert(_connection != null, "JC: DeleteLocationAsync: _connection is null");
            return _connection.DeleteAsync(item, recursive: true);
        }

        #endregion //location

        #region plant
        public async Task<List<Model.Plant>> GetPlantsAsync()
        {
            if (_connection != null)
                return await _connection.GetAllWithChildrenAsync<Model.Plant>();
            return new List<Model.Plant>();
        }
        public Task<List<Model.Plant>> GetPlantsAsync(int locationId)
        {
            return  _connection.GetAllWithChildrenAsync<Model.Plant>(i => i.LocationId == locationId);  
        }        

        public async Task AddPlantAsync(Model.Location location, Model.Plant plant)
        {
            location.Plants.Add(plant);            
            await _connection.InsertOrReplaceWithChildrenAsync(plant, recursive: true);
            await SaveLocationAsync(location);            
        }

        public Task SavePlantAsync(Model.Plant plant)
        {            
            return _connection.InsertOrReplaceWithChildrenAsync(plant, recursive: true);
        }

        public Task DeletePlantAsync(Model.Plant plant)
        {
            //TODO: does it delete from location also?
            return _connection.DeleteAsync(plant, recursive: true);
        }

        public async Task MovePlantAsync(Model.Plant plant, Model.Location fromLocation, Model.Location toLocation)
        {
            if (plant.LocationId == toLocation.Id)
                return;

            if (plant.LocationId != 0)
            {
                Model.Location oldLocation = fromLocation;
                if (plant.LocationId != fromLocation.Id)                 
                    oldLocation = await GetLocationAsync(plant.LocationId);
                                
                oldLocation.Plants.Remove(oldLocation.Plants.First(p=>p.Id == plant.Id));
                await SaveLocationAsync(oldLocation);
            }
            toLocation.Plants.Add(plant);
            await SaveLocationAsync(toLocation);

            await SavePlantAsync(plant);
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
