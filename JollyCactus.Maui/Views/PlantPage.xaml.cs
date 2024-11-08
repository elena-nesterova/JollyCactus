
using JollyCactus.Maui.ViewModel;
using JollyCactus.Maui.ViewModel.PlantProperties;
using System.Diagnostics;

namespace JollyCactus.Maui.Views;

public partial class PlantPage : ContentPage
{    
    private readonly PlantVM _plantVM;
    private readonly bool _isCreating = true;
    private bool _isLoaded = false;
    private Data.IPropertySharedService _propertySharedService;
    private PlantPropertyVM? _selectedProperty = null;
    //private Dictionary<string, Microsoft.Maui.Controls.View> _properties;

    public PlantPage(PlantVM plantVM)
    {
        _plantVM = plantVM;
        _isCreating = false;

        InitPage();

        Title = plantVM.Name;
    }

    public PlantPage(Model.Location location, Model.Plant? plant = null)
	{		
        _plantVM = new PlantVM(location, plant);
        _isCreating = plant == null;
       
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

        if (!_isLoaded)
        {
            await _plantVM.LoadPlant();
            _isLoaded = true;
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
    
    private void OnPlantPropertyChanged(string propertyName)
    {
        
        var prop = _propertySharedService.GetAndRemoveValue<PlantPropertyVM>(propertyName);
        Debug.Assert(prop != null, "OnPlantPropertyChanged cannot find property: " + propertyName);
        //Debug.Assert(_selectedProperty.Name.Equals(prop.Name), "OnPlantPropertyChanged: Property sharing process corrupt");
        Debug.WriteLine("JC: PlantPage OnPlantPropertyChanged - " + propertyName);

        _plantVM.UpdateProperty(prop);
        OnPropertyChanged(nameof(PlantVModel));
        Debug.WriteLine("JC: OnPlantPropertyChanged");               
    }

    private async void OpenPropertyPage()
    {
        if (_selectedProperty == null)        
            return;
        
        if (_propertySharedService.GetValue<PlantPropertyVM>(_selectedProperty.Name) != null)
            return;

        PlantPropertyVM propCopy = _selectedProperty.Clone() as PlantPropertyVM;        

        _propertySharedService.Add<PlantPropertyVM>(_selectedProperty.Name, propCopy);

        await Navigation.PushAsync(
            new ModifyPropertyPage(_propertySharedService, _selectedProperty.Name));
    }

    private void OnPropertyTapped(object sender, EventArgs e)
    {
        SelectedProperty = ((Grid)sender).BindingContext as PlantPropertyVM;           
    }
   
    private async void OnOkClicked(object sender, EventArgs e)
    {
        await _plantVM.SavePlant();
        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {        
        await Shell.Current.GoToAsync("..");
    }    
    
}