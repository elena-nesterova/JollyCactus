using JollyCactus.Maui.ViewModel.PlantProperties;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace JollyCactus.Maui.Views;

public partial class ModifyPropertyPage : ContentPage 
{
    private readonly Data.IPropertySharedService _sharedService;
    private readonly string _propertyName;
    private bool _needSave = false;
   // private readonly bool _canBeEmpty;

    PlantPropertyVM? _property;
    
    public ModifyPropertyPage(Data.IPropertySharedService sharedService, string propertyName)
        //Data.IPropertySharedService sharedService, string title, string propertyName, bool canBeEmpty = false)
	{
		InitializeComponent();

        _sharedService = sharedService;
        _propertyName = propertyName;
        _property = _sharedService.GetValue<PlantPropertyVM>(propertyName);
        Debug.Assert(_property != null, _propertyName);
        Debug.WriteLine("JC: PropertyPage - " + _property.Name);

        //Properties.Add(property);
        //collectionViewProperty.ItemsSource= (System.Collections.IEnumerable)_property;

        if (_property != null)
        {
            this.Title = "Change " + _property.Name;
            BindingContext = _property;
        }
    }
     

   // public string PropertyValue {  get; set; }    
   
    private async void OnOkClicked(object sender, EventArgs e)
    {
        //if (string.IsNullOrEmpty(entryPropertiesValue.Text) && !_canBeEmpty)
        //{
        //    await DisplayAlert("Invalid value", "Field " + _propertyName + "can not be empty.", "OK");
        //}
        //else
        //{
            //SetProperty(PropertyValue);
            

        //OnPropertyChanged(_propertyName);
        if (_property != null)
        {
            _needSave = true;

            Debug.WriteLine("JC: PropertyPage Save - " + _property.Name);
            _sharedService.Add<PlantPropertyVM>(_propertyName, _property);
        }
        await Shell.Current.GoToAsync("..");
        //}
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        //var s = Shell.Current.Navigation.NavigationStack;
        //await Shell.Current.Navigation.PopAsync();


        //SetProperty(null);
        //OnPropertyChanged(_propertyName);
        
        await Shell.Current.GoToAsync("..");
    }

    protected override void OnDisappearing()
    {
        if (!_needSave)
        {
            Debug.WriteLine("JC: PropertyPage Not Save - " + _propertyName);
            _sharedService.Add<PlantPropertyVM>(_propertyName, null);
            OnPropertyChanged(_propertyName);
        }
        //Shell.Current.Navigating -= OnNavigating;
        base.OnDisappearing();
    }
/*
    private string PlantPropertyVM()
    {
        string? value = _sharedService.GetValue<PlantPropertyVM>(_propertyName);
        if (value == null)
            return "";
        return value;
    }

    private void SetProperty(PlantPropertyVM? value)
    {
        _sharedService.Add<string>(_propertyName, value);        
    }*/
}