using JollyCactus.Maui.ViewModel;
using System.Collections.ObjectModel;

namespace JollyCactus.Maui.Views.PlantPropertyViews;

public partial class ModifyPropertyStringWithOptionsView : ContentView
{
    private JollyCactusVM _viewModel;
    public ObservableCollection<string> Options = new();

    public ModifyPropertyStringWithOptionsView()
	{
		InitializeComponent();
        _viewModel = Application.Current.MainPage.Handler.MauiContext.Services.GetService<JollyCactusVM>();
    }

    protected override async void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (BindingContext is ViewModel.PlantProperties.PlantPropertyVM property)
        {
            var options = await _viewModel.Recomendation.GetPropertyOptionsList(property);
            if (options != null)
                Options = new ObservableCollection<string>(options);
           
        }
    }

    private void OnTextChanged(object sender, EventArgs e)
    {
        
    }

    private void OnOptionSelected(object sender, EventArgs e)
    {

    }
}