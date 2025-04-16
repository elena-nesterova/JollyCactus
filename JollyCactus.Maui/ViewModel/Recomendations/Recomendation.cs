using JollyCactus.Maui.Data;
using JollyCactus.Maui.Model;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;


namespace JollyCactus.Maui.ViewModel.Recomendations
{
    public class Recomendation : BaseViewModel
    {
        private TemplatesDb _db;
        //private bool _isInitialized = false;
        //public bool IsInitialized { get => _isInitialized; }

        //private List<FamilyTemplate> _familyTemplates = new List<FamilyTemplate>();
        //public ObservableCollection<string> Families { get; set; } = new ObservableCollection<string>();

        //public ObservableCollection<string> BotanicalNames { get; set; }

        private string _selectedFamily = string.Empty;
        private string _selectedBotanicalName = string.Empty;
/*
        public string SelectedFamily 
        {
            get => _selectedFamily; 
            set
            {
                if (_selectedFamily == value)
                    return;
                _selectedFamily = value;
                if (string.IsNullOrWhiteSpace(_selectedFamily))
                {
                    Task.Run(async () =>
                    {
                        var plants = await _db.GetPlantTemplatesAsync();
                        BotanicalNames = new(plants.Select(x => x.Name));
                    });
                }
                else if (Families.Contains(SelectedFamily))
                {
                    var family = _familyTemplates.FirstOrDefault(x => x.Name.Equals(SelectedFamily, StringComparison.InvariantCultureIgnoreCase));
                    if (family != null)
                    {
                        Task.Run(async () =>
                        {
                            var plants = await _db.GetPlantTemplatesAsync(family.Id);
                            BotanicalNames = new(plants.Select(x => x.Name));
                        });                        
                    }
                }
                else
                {
                    Task.Run(async () =>
                    {
                        FamilyTemplate family = new FamilyTemplate
                        {
                            Name = SelectedFamily
                        };
                        await _db.SaveFamilyTemplateAsync(family);
                        await Initialize();
                    });
                    
                    BotanicalNames = new ObservableCollection<string>();
                } 
                OnPropertyChanged(nameof(SelectedFamily));
            }
        }

        public string SelectedBotanicalName
        {
            get => _selectedBotanicalName;
            set
            {
                if (_selectedBotanicalName == value)
                    return;

                // TODO: Add logic to handle the selected botanical name
                if (BotanicalNames.Contains(value))
                {
                    // Handle the selected botanical name
                }
                else
                {
                    
                }

                _selectedBotanicalName = value;
                OnPropertyChanged(nameof(SelectedBotanicalName));
            }
        }

       */ 

        public Recomendation(string templateDbFullFileName = null)
        {
            if (string.IsNullOrEmpty(templateDbFullFileName))
                templateDbFullFileName = Path.Combine(JcPaths.GetAppFilesPath(), "Templates" + JcPaths.DbExtension);
            _db = new(templateDbFullFileName);
        }

        //public async Task Initialize()
        //{
        //    _familyTemplates = await _db.GetFamilyTemplatesAsync();/
        //    Families = new ObservableCollection<string>(_familyTemplates.Select(x => x.Name));
        //    _isInitialized = true;
        //}

        private async Task<List<string>?> GetFamilies()
        {
            var familyTemplates = await _db.GetFamilyTemplatesAsync();
            if (familyTemplates == null || familyTemplates.Count == 0)
                return null;
            var families = new List<string>(familyTemplates.Select(x => x.Name));
            return families;
        }

        private async Task<List<string>?> GetBotanicalNames()
        {
            var plantTemplates = await _db.GetPlantTemplatesAsync();
            if (plantTemplates == null || plantTemplates.Count == 0)
                return null;
            var botanicalNames = new List<string>(plantTemplates.Select(x => x.Name));
            return botanicalNames;
        }
        
        public async Task<List<string>?> GetPropertyOptionsList(PlantProperties.PlantPropertyVM property)
        {
            if (property == null)
                return null;
            switch (property.ParentName)
            {
                case string str when str.Equals(Model.PlantPropertiesValues.PlantPropertyFamilyName, StringComparison.InvariantCultureIgnoreCase):
                    return await GetFamilies();

                case string str when str.Equals(Model.PlantPropertiesValues.PlantPropertyBotanicalName, StringComparison.InvariantCultureIgnoreCase):
                    return await GetBotanicalNames();
            }
            return null;
        }

       
    }
}
