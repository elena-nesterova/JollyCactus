using JollyCactus.Maui.ViewModel;
using System.Diagnostics;

namespace JollyCactus.Maui.Views;

public partial class LocationTablePage : ContentPage
{    
    private readonly ViewModel.JollyCactusVM _viewModel;

    public LocationTablePage()
	{
        
        InitializeComponent();

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
        }
    }


    private async void OnAddLocationClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LocationPage(null));        
    }

    private async void OnAddPlantClicked(object sender, EventArgs e)
    {
        if (sender is BindableObject bsender && bsender.BindingContext is LocationVM location)
            await Navigation.PushAsync(new PlantPage(location, null));        
    }

    private async void OnEditLocationClicked(object sender, EventArgs e)
    {
        if (sender is BindableObject bsender && bsender.BindingContext is LocationVM location)        
            await Navigation.PushAsync(new LocationPage(location));        
    }

    private async void OnDeleteLocationClicked(object sender, EventArgs e)
    {
        if (sender is BindableObject bsender && bsender.BindingContext is LocationVM location)
        {
            var res = await DisplayAlert("Delete location?", "Do you really want to delete location " + location.Name + "?", "Yes", "No");

            if (res == true)                
            {
                await _viewModel.DeleteLocation(location);
            }
        }
        
    }

    private async void lvLocationsItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is BindableObject bsender && bsender.BindingContext is LocationVM location)
        {           
            var page = this.Handler.MauiContext.Services.GetService<PlantTablePage>();
            if (page != null)
            {
                _viewModel.SelectedLocation = location;
                await Navigation.PushAsync(page);
            }
        }
    }

}