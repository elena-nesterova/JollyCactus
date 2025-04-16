using JollyCactus.Maui.ViewModel;
using System.Diagnostics;

namespace JollyCactus.Maui.Views.Location;

public class LocationSharedContext
{
    public LocationSharedContext(LocationVM location, PlantVM plant) 
    {
        this.Location = location;
        this.Plant = plant;
    }
    public LocationVM Location { get; set; }
    public PlantVM Plant { get; set; }
}

public partial class LocationChoosePage : ContentPage
{
    
    public bool IsChanged { get; set; } = false;

    private readonly Data.IPropertySharedService _sharedService;
    private readonly string _propertyName;
    private readonly LocationVM? _location = null;
    private LocationVM? _selectedLocation = null;
    private readonly PlantVM _plant;

    public LocationChoosePage(string propertyName, Data.IPropertySharedService service)
    {
        var viewModel = Application.Current.MainPage.Handler.MauiContext.Services.GetService<JollyCactusVM>();

        Debug.Assert(viewModel != null);

        BindingContext = viewModel;
        _propertyName = propertyName;
        _sharedService = service;

        var sharedContext = _sharedService.GetValue<LocationSharedContext>(propertyName);
        Debug.Assert(sharedContext != null);

        _location = sharedContext.Location;
        _plant = sharedContext.Plant;

        Debug.Assert(_location != null);
        Debug.Assert(_plant != null);
        
        _selectedLocation = _location;

        this.Title = _plant.Name + " - location";

        InitializeComponent();
        
    }

    public LocationVM? SelectedLocation
    {
        get => _selectedLocation;
        set
        {            
            if (_selectedLocation != value)
            {
                _selectedLocation = value;
                //OnPropertyChanged(nameof(SelectedLocation));
            }
            IsChanged = _selectedLocation != _location;
            OnPropertyChanged(nameof(IsChanged));
        }
    }

    private async void OnOkClicked(object sender, EventArgs e)
    {
        if (IsChanged && SelectedLocation != null)
        {
            _sharedService.Add<LocationSharedContext>(_propertyName, new LocationSharedContext(_selectedLocation, _plant));
        }
        else
        {
            IsChanged = false;
        }
        await Shell.Current.GoToAsync("..");

    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    protected override void OnDisappearing()
    {
        if (!IsChanged)
        {
            _sharedService.Add<LocationSharedContext>(_propertyName, null);
            OnPropertyChanged(_propertyName);
        }
        base.OnDisappearing();
    }

}