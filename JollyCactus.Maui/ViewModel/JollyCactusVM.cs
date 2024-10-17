using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JollyCactus.Maui.ViewModel
{
    public class JollyCactusVM
    {
        public ObservableCollection<Model.Location> Locations { get; set; }
        public ObservableCollection<PlantVM> Plants { get; set; }
        //public ICommand AddLocationCommand { get; }
        //public ICommand DeleteLocationCommand { get; }
        //public ICommand UpdateLocationCommand { get; }
        //public ICommand AddPlantCommand { get; }

        public JollyCactusVM()
        {
            Locations = new ObservableCollection<Model.Location>();
            Plants = new ObservableCollection<PlantVM>();

            //AddLocationCommand = new Command(async () => await AddLocation());
            //DeleteLocationCommand = new Command<Model.Location>(async (location) => await DeleteLocation(location));
            //UpdateLocationCommand = new Command<Model.Location>(async (location) => await UpdateLocation(location));
            //AddPlantCommand = new Command<Model.Location, Model.Plant>(async (location, newPlant) => await AddPlant(location, newPlant));
        }

        public async Task LoadLocations()
        {
            List<Model.Location> locations = await App.Database.GetLocationsAsync();
            Locations.Clear();
            foreach (var item in locations)
            {
                Locations.Add(item);
            }
        }

        public async Task AddLocation(Model.Location location)
        {            
            await App.Database.SaveLocationAsync(location);
            await LoadLocations();
        }

        public async Task DeleteLocation(Model.Location location)
        {
            if (location != null)
            {
                await App.Database.DeleteLocationAsync(location);
                await LoadLocations();
            }
        }

        public async Task UpdateLocation(Model.Location location)
        {
            if (location != null)
            {               
                await App.Database.SaveLocationAsync(location);
                await LoadLocations();                
            }
        }

        public async Task LoadPlants()
        {
            List<Model.Plant> plants = await App.Database.GetPlantsAsync();
            Plants.Clear();
            foreach (var item in plants)
            {
                if (item.LocationId != 0)
                {
                    var location = await App.Database.GetLocationAsync(item.LocationId);
                    var plant = new PlantVM(location, item);
                    await plant.LoadPlant();
                    Plants.Add(plant);
                }
            }
        }
               

        /*
                async Task AddPlant(Model.Location location, Model.Plant newPlant)
                {
                    if (location != null)
                    {
                        location.Plants.Add(newPlant);
                        await App.Database.SaveLocationAsync(location);
                        await LoadLocations();                
                    }
                }*/
    }
}
