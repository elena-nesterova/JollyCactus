using JollyCactus.Maui.ViewModel;
using JollyCactus.Maui.ViewModel.PlantProperties;
using JollyCactus.Maui.Views.Location;
using System.Diagnostics;

namespace JollyCactus.Maui.Views;

public partial class PlantPage : ContentPage
{
    private readonly ViewModel.JollyCactusVM _viewModel;
    private readonly PlantVM _plantVM;
    private readonly bool _isCreating = true;
    private Data.IPropertySharedService _propertySharedService;
    private PlantPropertyVM? _selectedProperty = null;
    
   
    public PlantPage(LocationVM location, PlantVM? plantVM = null)
	{
        var viewModel = Application.Current.MainPage.Handler.MauiContext.Services.GetService<JollyCactusVM>();

        Debug.Assert(viewModel != null);

        _viewModel = (JollyCactusVM)viewModel;

        _isCreating = plantVM == null;
        
        _plantVM = (plantVM == null) ? new PlantVM(location, null) : plantVM;

        InitPage();

        if (_isCreating)
            Title = "New Plant";        
    }

    private void InitPage()
    {
        InitializeComponent();        

        _propertySharedService = new Data.PropertySharedService();
        _propertySharedService.OnPropertyChanged += OnPlantPropertyChanged;

        BindingContext = PlantVModel;//_plantVM;
    }

    
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!_plantVM.PlantPropertiesByGroups.Any())
        {
            await _plantVM.LoadPlant();            
            OnPropertyChanged(nameof(PlantVModel));
        }
       
    } 
   
    public PlantPropertyVM? SelectedProperty
    {
        get => _selectedProperty;
        set
        {
            _selectedProperty = value;
            OpenPropertyPage();            
        }
    }

    public PlantVM PlantVModel { get => _plantVM; }
    
    private async void OnPlantPropertyChanged(string propertyName)
    {
        Debug.WriteLine("JC: PlantPage.OnPlantPropertyChanged - " + propertyName);

        if (propertyName.Equals("Location", StringComparison.CurrentCultureIgnoreCase))
        {
            var sharedContext = _propertySharedService.GetAndRemoveValue<LocationSharedContext>(propertyName);

            if (sharedContext != null && sharedContext.Location != null)
            {
               await _plantVM.MoveToAsync(sharedContext.Location);
                OnPropertyChanged(nameof(PlantVModel));
            }
            return;
        }

        var prop = _propertySharedService.GetAndRemoveValue<PlantPropertyVM>(propertyName);
        
        if (prop != null)
        {
            Debug.WriteLine("JC: PlantPage.OnPlantPropertyChanged - update " + propertyName);
            _plantVM.UpdateProperty(prop);
            OnPropertyChanged(nameof(PlantVModel));
        }               
    }

    private async void OpenPropertyPage()
    {
        if (_selectedProperty == null)        
            return;
        
        if (_propertySharedService.GetValue<PlantPropertyVM>(_selectedProperty.Name) != null)
            return;

        if (_selectedProperty.Clone() is PlantPropertyVM propCopy)
        {

            _propertySharedService.Add<PlantPropertyVM>(_selectedProperty.Name, propCopy);

            await Navigation.PushAsync(
                new ModifyPropertyPage(_selectedProperty.Name, _plantVM.Name, _propertySharedService));
        }
    }

    private void OnPropertyTapped(object sender, EventArgs e)
    {
        if (sender is BindableObject bsender && bsender.BindingContext is PlantPropertyVM)
            SelectedProperty = bsender.BindingContext as PlantPropertyVM;           
    }

    //private void OnPictureTapped(object sender, EventArgs e)
    //{
    //    if (sender is BindableObject bsender && bsender.BindingContext is PlantPropertyVM)
    //        SelectedProperty = bsender.BindingContext as PlantPropertyVM;
    //}

    private async void OnLocationTapped(object sender, EventArgs e)
    {
        string propertyName = "Location";
        
        if (_propertySharedService.GetValue<LocationSharedContext>(propertyName) != null)
            return;
        
        _propertySharedService.Add<LocationSharedContext>(
            propertyName, new LocationSharedContext(_plantVM.Location, _plantVM));

        await Navigation.PushAsync(
            new LocationChoosePage(propertyName, _propertySharedService));
        
    }

    private async void OnOkClicked(object sender, EventArgs e)
    {
        if (_isCreating)
            await _viewModel.AddPlant(_plantVM);
        else
            await _plantVM.UpdatePlant();

        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {        
        await Shell.Current.GoToAsync("..");
    }       
}