using Microsoft.EntityFrameworkCore;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;


namespace JollyCactus.Maui.Data
{
    internal class TemplatesDb : DbContext
    {
        private SQLiteAsyncConnection _connection;

        private string _dbFullDbFileName;

        public TemplatesDb(string dbFileFullName)
        {
            _dbFullDbFileName = dbFileFullName;

            Connect();
        }

        private void Connect()
        {
            _connection = new SQLiteAsyncConnection(_dbFullDbFileName, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            _connection.CreateTableAsync<Model.FamilyTemplate>().Wait();
            _connection.CreateTableAsync<Model.PlantTemplate>().Wait();
        }

        #region FamilyTemplates

        public async Task<List<Model.FamilyTemplate>> GetFamilyTemplatesAsync()
        {
            if (_connection != null)
                return await _connection.GetAllWithChildrenAsync<Model.FamilyTemplate>();
            return new List<Model.FamilyTemplate>();
        }
               

        public Task SaveFamilyTemplateAsync(Model.FamilyTemplate item)
        {
            if (_connection != null)
                return _connection.InsertOrReplaceWithChildrenAsync(item);
            return Task.CompletedTask;
        }

        public Task DeleteFamilyTemplateAsync(Model.FamilyTemplate item)
        {
            if (_connection != null)
                return _connection.DeleteAsync(item, recursive: true);
            return Task.CompletedTask;
        }

        #endregion // FamilyTemplates

        #region PlantTemplates

        public async Task<List<Model.PlantTemplate>> GetPlantTemplatesAsync()
        {
            if (_connection != null)
                return await _connection.GetAllWithChildrenAsync<Model.PlantTemplate>();
            return new List<Model.PlantTemplate>();
        }
        public Task<List<Model.PlantTemplate>> GetPlantTemplatesAsync(int familyId)
        {
            if (_connection != null)
                return _connection.GetAllWithChildrenAsync<Model.PlantTemplate>(i => i.FamilyId == familyId);
            return Task.FromResult(new List<Model.PlantTemplate>());
        }

        public async Task AddPlantTemplateAsync(Model.FamilyTemplate family, Model.PlantTemplate plant)
        {
            if (_connection == null)
                return;

            family.PlantTemplates.Add(plant);
            await _connection.InsertOrReplaceWithChildrenAsync(plant, recursive: true);
            await SaveFamilyTemplateAsync(family);
        }

        public async Task SavePlantAsync(Model.PlantTemplate plant)
        {
            if (_connection != null)                
                await _connection.InsertOrReplaceWithChildrenAsync(plant, recursive: true);
        }

        public async Task DeletePlantAsync(Model.PlantTemplate plant)
        {
            if (_connection != null)
                await _connection.DeleteAsync(plant, recursive: true);
        }

        #endregion //PlantTemplates
    }
}
