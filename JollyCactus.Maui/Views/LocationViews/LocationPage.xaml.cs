using JollyCactus.Maui.ViewModel;
using System.Diagnostics;

namespace JollyCactus.Maui.Views;

public partial class LocationPage : ContentPage
{
    //private readonly Data.LocalDbService _dbService;
    private readonly ViewModel.JollyCactusVM _viewModel;
    private readonly LocationVM _location;
    
    public LocationPage(LocationVM? location = null)
	{
		InitializeComponent();

        var viewModel = Application.Current.MainPage.Handler.MauiContext.Services.GetService<JollyCactusVM>();

        Debug.Assert(viewModel != null);

        _viewModel = (JollyCactusVM)viewModel;


        if (location == null)
        {
            _location = new LocationVM
            {
                Name = "Room 1",
                Note = "",
                
            };
            
            Title = "New location";
        }
        else
        {
            _location = location;
            Title = "Change location: " + _location.Name;
        }
        BindingContext = _location;
    }

    private async void OnBtnAddLocationOkClicked(object sender, EventArgs e)
    {
        await _viewModel.SaveLocation(_location);       

        await Shell.Current.GoToAsync("..");
    }

    private void OnBtnAddLocationExitClicked(object sender, EventArgs e)
    {
        _location.UpdateFromModel();
        Shell.Current.GoToAsync("..");
    }
}