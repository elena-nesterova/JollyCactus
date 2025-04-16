using System.Collections.ObjectModel;
using System.Diagnostics;


namespace JollyCactus.Maui.ViewModel
{
    public enum PlantSortByValues
    {
        SortByNo = 0,
        SortByName = 1,
        SortByBotanicalName = 2,
        SortByFamily = 3, 
        SortByAdoptionDate = 4,
        SortByLocation = 5
    }

    public class JollyCactusVM : BaseViewModel
    {
        public Recomendations.Recomendation Recomendation { get; }
        public ObservableCollection<LocationVM> Locations { get; private set; }
        
        public ObservableCollection<PlantVM> Plants { get; private set; }

        private LocationVM? _selectedLocation = null;

        public bool IsLoaded => App.Database.IsConnected;


        public JollyCactusVM()
        {
            Recomendation = new();
            Locations = new ObservableCollection<LocationVM>();
            Plants = new ObservableCollection<PlantVM>();

        }             

        public async Task LoadLocations(bool reload = false)
        {
            if (Locations.Any() && !reload)
                return; 

            List<Model.Location> locations = await App.Database.GetLocationsAsync();
            Locations.Clear();

            int plantsAmount = 0;
            foreach (var item in locations)
            {
                plantsAmount += item.Plants.Count;
            }

            // for all elements
            var root = new LocationVM { IsRoot = true, Name = "--All--"};
            root.UpdateFromModel(plantsAmount);
            Locations.Add(root); 


            foreach (var item in locations)
            {
                var location = new LocationVM(item);
                Locations.Add(location);
            }
            OnPropertyChanged(nameof(Locations));

            if (SelectedLocation == null)
            {
                SelectedLocation = Locations.FirstOrDefault();
            }
            else
            {
                if (SelectedLocation.IsRoot)
                    SelectedLocation = Locations.FirstOrDefault();
                else
                {
                    var l = Locations.FirstOrDefault(x => x.Id == SelectedLocation.Id, null);

                    if (l == null)
                        SelectedLocation = Locations.FirstOrDefault();
                    else
                        SelectedLocation = l;
                }
            }
        }

        public LocationVM? SelectedLocation 
        { 
            get
            {               
                return _selectedLocation;
            }
            set
            {                
                if (value != null && _selectedLocation != value)
                {
                    if (value == null || value.Id == 0)
                        _selectedLocation = Locations.FirstOrDefault();
                    else
                        _selectedLocation = Locations.FirstOrDefault(x => x.Id == value.Id, null);

                    Task.Run(LoadPlants);

                    OnPropertyChanged(nameof(SelectedLocation));
                }
            }
        }

        public async Task SaveLocation(LocationVM location)
        {
            location.CommitChanges();
            if (location.Model == null)
                return;
            
            bool isCreate = location.Model.Id == 0;
            //if (!Locations.Where(i => location.Name.Equals(i.Name)).Any())
            {
                await App.Database.SaveLocationAsync(location.Model);
                location.UpdateFromModel();
                if (isCreate)
                {
                    Locations.Add(location);

                    UpdatePlantsAmount(Locations.First());
                    OnPropertyChanged(nameof(Locations));
                }
                //await LoadLocations();
            }
            OnPropertyChanged(nameof(Locations));
            
        }

        public async Task UpdateLocation(LocationVM location)
        {
            location.CommitChanges();
            if (location.Model != null)
            {
                await App.Database.SaveLocationAsync(location.Model);
                await LoadLocations();
            }
        }

        public async Task DeleteLocation(LocationVM location)
        {
            if (location.Model != null && !location.IsRoot)
            {
                await App.Database.DeleteLocationAsync(location.Model);
                Locations.Remove(location);

                UpdatePlantsAmount(Locations.First());

                OnPropertyChanged(nameof(Locations));
                //await LoadLocations();
            }
        }

        public async Task AddPlant(PlantVM plant)
        {
            if (false == await plant.UpdatePlant())
            {
                await App.Database.AddPlantAsync(plant.Location.Model, plant.Model);
                
                if (plant.Location == SelectedLocation || SelectedLocation.IsRoot)
                {
                    Plants.Add(plant);
                    OnPropertyChanged(nameof(Plants));
                }
                UpdatePlantsAmount();
            }
        }

        public async Task DeletePlant(PlantVM plant)
        {
            if (plant.Model.Id != 0)
                await App.Database.DeletePlantAsync(plant.Model);
            
            if (plant.Location == SelectedLocation || SelectedLocation.IsRoot)
            {
                Plants.Remove(plant);
                OnPropertyChanged(nameof(Plants));
            }
            UpdatePlantsAmount();
        }

        private void UpdatePlantsAmount(LocationVM? location = null)
        {
            if (location == null)
            {
                int plantsAmount = 0;
                foreach (var l in Locations)
                {
                    if (!l.IsRoot)
                    { 
                        l.UpdateFromModel();
                        plantsAmount += l.PlantsCount;
                    }
                }
                Locations.First().UpdateFromModel(plantsAmount);
            }
            else if (location.IsRoot)
            {
                int plantsAmount = 0;
                foreach (var l in Locations)
                {
                    if (!l.IsRoot)
                    {
                        plantsAmount += l.PlantsCount;
                    }
                }
                Locations.First().UpdateFromModel(plantsAmount);
            }
            else
            {
                location.UpdateFromModel();
            }
        }

        private void PlantsSortBy(PlantSortByValues by)
        {
            List<PlantVM> list = Plants.ToList();
            switch (by)
            {
                case PlantSortByValues.SortByName:
                    list.Sort((l, r) => l.Name.CompareTo(r.Name));
                    break;
                case PlantSortByValues.SortByFamily:
                    list.Sort((l, r) => l.Family.CompareTo(r.Family));
                    break;
                case PlantSortByValues.SortByBotanicalName:
                    list.Sort((l, r) => l.BotanicalName.CompareTo(r.BotanicalName));
                    break;
                case PlantSortByValues.SortByAdoptionDate:
                    list.Sort((l, r) => l.AdoptionDate.CompareTo(r.AdoptionDate));
                    break;
                case PlantSortByValues.SortByLocation:
                    list.Sort((l, r) => l.LocationName.CompareTo(r.LocationName));
                    break;
                default:
                    break;
            }
            
            Plants = new ObservableCollection<PlantVM>(list);
            OnPropertyChanged(nameof(Plants));
        }

        public async Task PlantsSortByAsync(PlantSortByValues by)
        {
            await Task.Run(() => PlantsSortBy(by));
        }

        private async Task LoadPlants()
        {
            await LoadLocations();

            //SelectedLocation = location;
            Debug.Assert(SelectedLocation != null);

            if (SelectedLocation == null)
                return;

            List<Model.Plant> plants;
            if (SelectedLocation.IsRoot)
                plants = await App.Database.GetPlantsAsync();
            else
            {
                plants = await App.Database.GetPlantsAsync(SelectedLocation.Id);
                
            }

            Plants.Clear();
            foreach (var item in plants)
            {
                if (item.LocationId != 0)
                {
                    var loc = Locations.FirstOrDefault(x => x.Id == item.LocationId, null);
                    Debug.Assert(loc != null);

                    var plant = new PlantVM(loc, item);
                    await plant.LoadPlant();
                    Plants.Add(plant);
                }
            }
            OnPropertyChanged(nameof(Plants));
        }

        

        public async Task ClearDatabase()
        {
            await App.Database.ClearAll();
            await LoadLocations(true);
        }

        public async Task CloseDbConnection()
        {
            await App.Database.CloseDatabase();
            await LoadLocations(true);
            OnPropertyChanged(nameof(IsLoaded));
        }

        public async Task LoadDatabase(string newDbFullFileName)
        {
            await App.Database.ChangeDatabase(newDbFullFileName);
            
            await LoadLocations(true);

            OnPropertyChanged(nameof(IsLoaded));

        }
    }
}
