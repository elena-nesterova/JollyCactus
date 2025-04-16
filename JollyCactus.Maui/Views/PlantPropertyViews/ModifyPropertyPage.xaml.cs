using JollyCactus.Maui.ViewModel.PlantProperties;
using System.Diagnostics;

namespace JollyCactus.Maui.Views;

public partial class ModifyPropertyPage : ContentPage 
{
    private readonly Data.IPropertySharedService _sharedService;
    private readonly string _propertyName;
    private bool _needSave = false;
   
    PlantPropertyVM? _property;
    
    public ModifyPropertyPage(string propertyName, string plantName, Data.IPropertySharedService service)
    {
		InitializeComponent();
                
        _sharedService = service;

        _propertyName = propertyName;
        _property = _sharedService.GetValue<PlantPropertyVM>(propertyName);
        Debug.Assert(_property != null, _propertyName);
        
        if (_property != null)
        {
            this.Title = plantName + " - " + _property.Name;
            BindingContext = _property;
        }
    }
    public string PropertyValue {  get; set; }    
   
    private async void OnOkClicked(object sender, EventArgs e)
    {        
        if (_property != null)
        {
            _needSave = true;

            Debug.WriteLine("JC: PropertyPage Save - " + _property.Name);
            _sharedService.Add<PlantPropertyVM>(_propertyName, _property);
        }
        await Shell.Current.GoToAsync("..");
        
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {        
        await Shell.Current.GoToAsync("..");
    }

    protected override void OnDisappearing()
    {
        Debug.WriteLine("JC: PropertyPage Disappearing NeedSave - " + _needSave + " " + _propertyName);
        if (!_needSave)
        {
            _sharedService.Add<PlantPropertyVM>(_propertyName, null);
            OnPropertyChanged(_propertyName);
        }
        base.OnDisappearing();
    }
}