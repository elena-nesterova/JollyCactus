
namespace JollyCactus.Maui.Views;

public partial class LocationTablePage : ContentPage
{
    //private readonly Data.LocalDbService _dbService;
    private readonly ViewModel.JollyCactusVM _viewModel;

    public LocationTablePage(ViewModel.JollyCactusVM viewModel)
	{
        
        InitializeComponent();

        _viewModel = viewModel;

        BindingContext = _viewModel;        

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel != null)
        {
            await _viewModel.LoadLocations();
        }
    }

    private async void OnRemoveAllClicked(object sender, EventArgs e)
    {
        var res = await DisplayAlert("Delete all?", "Do you really want to delete all locations and plants?", "Yes, delete all", "No, please, do not delete");

        if (res == true)
        {
            await App.Database.ClearAll();
            if (_viewModel != null)
            {
                await _viewModel.LoadLocations();
            }
        }
    }

    private async void OnAddLocationClicked(object sender, EventArgs e)
    {
        //Shell.Current.GoToAsync(nameof(AddLocationPage));
        await Navigation.PushAsync(new LocationPage(_viewModel, null));
        //lvLocations.ItemsSource = await _dbService.GetLocationsAsync();
    }

    private async void OnAddPlantClicked(object sender, EventArgs e)
    {
        Model.Location? location = ((Button)sender).BindingContext as Model.Location;
        if (location != null)
            await Navigation.PushAsync(new PlantPage(location, null));        
    }

    private async void OnEditLocationClicked(object sender, EventArgs e)
    {
        Model.Location? location = ((Button)sender).BindingContext as Model.Location;
        if (location != null)
        {
            await Navigation.PushAsync(new LocationPage(_viewModel, location));
        }

        //Shell.Current.GoToAsync(nameof(AddLocationPage));
        //var location = (Model.Location)lvLocations.SelectedItem;


        //Model.Location location = await _dbService.GetLocationAsync((int)(((Button)sender).BindingContext));
        //await Navigation.PushAsync(new LocationPage(_dbService, location));
        //lvLocations.ItemsSource = await _dbService.GetLocationsAsync();
    }

    private async void OnDeleteLocationClicked(object sender, EventArgs e)
    {
        Model.Location? location = ((Button)sender).BindingContext as Model.Location;
        if (location != null)
        {
            var res = await DisplayPromptAsync("Delete location", "Delete location " + location.Name + "?");

            if (res != null)
            {
                await _viewModel.DeleteLocation(location);
            }
        }
        //    Model.Location location = await _dbService.GetLocationAsync((int)(((Button)sender).BindingContext));

        //var res = await DisplayPromptAsync("Delete location", "Delete location " + location.Name + "?");

        //if (res != null)
        //{
        //    await _dbService.DeleteLocationAsync(location);
           
        //}
        //lvLocations.ItemsSource = await _dbService.GetLocationsAsync();
    }

    private void lvLocationsItemTapped(object sender, ItemTappedEventArgs e)
    {
        /*var location = (Model.Location)e.Item;
        var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");

        switch (action)
        {
            case "Edit":
                _plantId = plant.Id;
                entryName.Text = plant.Name;
                entryDescription.Text = plant.Description;
                break;
            case "Delete":
                await _dbService.DeleteItemAsync(plant);
                break;
        }*/
    }

}