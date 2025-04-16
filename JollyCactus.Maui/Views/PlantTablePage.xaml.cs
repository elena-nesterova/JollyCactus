using JollyCactus.Maui.ViewModel;
using JollyCactus.Maui.Views.Location;
using System.Diagnostics;

namespace JollyCactus.Maui.Views;

public partial class PlantTablePage : ContentPage
{
    private readonly JollyCactusVM _viewModel;
    private readonly Data.PropertySharedService _propertySharedService;

    public PlantTablePage()
	{
		InitializeComponent();

        _propertySharedService = new Data.PropertySharedService();
        _propertySharedService.OnPropertyChanged += OnPlantPropertyChanged;

        var viewModel = Application.Current.MainPage.Handler.MauiContext.Services.GetService<JollyCactusVM>();

        Debug.Assert(viewModel != null);
                
        _viewModel = (JollyCactusVM)viewModel;        

        BindingContext = _viewModel;        
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel != null)
        {
            await _viewModel.LoadLocations();
            //await _viewModel.LoadPlants();
        }
        
    }

    private async void OnSortClicked(object sender, EventArgs e)
    {
        string action = await App.Current.MainPage.DisplayActionSheet("Sort by", "Cancel", null, "by name", "by location", "by botanical name", "by family", "by adoption date");

        PlantSortByValues sortBy = PlantSortByValues.SortByNo;
        switch (action)
        {
            case "by name":
                sortBy = PlantSortByValues.SortByName;
                break;
            case "by location":
                sortBy = PlantSortByValues.SortByLocation;
                break;
            case "by botanical name":
                sortBy = PlantSortByValues.SortByBotanicalName;
                break;
            case "by family":
                sortBy = PlantSortByValues.SortByFamily;
                break;
            case "by adoption date":
                sortBy = PlantSortByValues.SortByAdoptionDate;
                break;
        }

        if (sortBy != PlantSortByValues.SortByNo)
            await _viewModel.PlantsSortByAsync(sortBy);
    }

    private async void OnAddPlantClicked(object sender, EventArgs e)
    {
        if (_viewModel.Locations.Count < 2)
        {
            await DisplayAlert("Add plant", "Cannot add a plant. You do not have any locations. You need to create a location first", "OK");            
        }
        else
        {    
            var location = (_viewModel.SelectedLocation.IsRoot) 
                ?  _viewModel.Locations.First(x=>!x.IsRoot)
                : _viewModel.SelectedLocation;

            if (location == null)
            {
                await DisplayAlert("Add plant", "Cannot add a plant. You need to choose a location first", "OK");
            }
            else
            {
                await Navigation.PushAsync(new PlantPage(location, null));
            }
        }
    }

    private async void OnPlantTapped(object sender, EventArgs e)
    {
        if (sender is BindableObject bsender && bsender.BindingContext is PlantVM plant)
        {
            await Navigation.PushAsync(new PlantPage(_viewModel.SelectedLocation, plant));
        }
    }

    private async void OnDeletePlantClicked(object sender, EventArgs e)
    {        
        if (sender is BindableObject bsender && bsender.BindingContext is PlantVM plant)
        {
            var res = await DisplayAlert("Delete the plant?", "Do you really want to delete plant " + plant.Name + "?", "Yes", "No");

            if (res == true)
            {
                await _viewModel.DeletePlant(plant);
                //await _viewModel.LoadLocations();//LoadPlants();
            }
        }        
    }

    private async void OnPlantPropertyChanged(string propertyName)
    {        
        if (propertyName.Equals("Location", StringComparison.CurrentCultureIgnoreCase))
        {
            var sharedContext = _propertySharedService.GetAndRemoveValue<LocationSharedContext>(propertyName);

            if (sharedContext != null && sharedContext.Location != null && sharedContext.Plant != null)
            {
                await sharedContext.Plant.MoveToAsync(sharedContext.Location);
                //TODO OnPropertyChanged(nameof());
            }
            return;
        }
        
    }

    private async void OnMovePlantClicked(object sender, EventArgs e)
    {
        if (sender is BindableObject bsender && bsender.BindingContext is PlantVM plant)
        {
            string propertyName = "Location";
            
            if (_propertySharedService.GetValue<LocationSharedContext>(propertyName) != null)
                return;

            _propertySharedService.Add<LocationSharedContext>(
                propertyName, new LocationSharedContext(plant.Location, plant));

            await Navigation.PushAsync(
                new LocationChoosePage(propertyName, _propertySharedService));
        }

    }

}