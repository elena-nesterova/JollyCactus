using JollyCactus.Maui.ViewModel;

namespace JollyCactus.Maui.Views;

public partial class PlantTablePage : ContentPage
{
    private readonly ViewModel.JollyCactusVM _viewModel;

    public PlantTablePage(ViewModel.JollyCactusVM viewModel)
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
            await _viewModel.LoadPlants();
        }
    }
    private async void OnAddPlantClicked(object sender, EventArgs e)
    {
        if (_viewModel.Locations.Count > 0)
        {
            await Navigation.PushAsync(new PlantPage(_viewModel.Locations[0], null));
        }
    }

    private async void OnPlantTapped(object sender, EventArgs e)
    {
        PlantVM? plant = ((View)sender).BindingContext as PlantVM;
        if (plant != null)
        {
            await Navigation.PushAsync(new PlantPage(plant));
        }
    }

    //private async void OnEditPlantClicked(object sender, EventArgs e)
    //{
    //    PlantVM? plant = ((Button)sender).BindingContext as PlantVM;
    //    if (plant != null)
    //    {
    //        await Navigation.PushAsync(new PlantPage(plant));
    //    } 
   // }

    private async void OnDeletePlantClicked(object sender, EventArgs e)
    {
        PlantVM? plant = ((Button)sender).BindingContext as PlantVM;
        if (plant != null)
        {
            var res = await DisplayAlert("Delete the plant?", "Do you really want to delete plant " + plant.Name + "?", "Yes", "No");

            if (res == true)
            {
                await plant.DeletePlant();
                await _viewModel.LoadPlants();
            }
        }
        
    }

}