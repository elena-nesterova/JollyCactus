namespace JollyCactus.Maui.Views.PlantPropertyViews;

using JollyCactus.Maui.ViewModel.PlantProperties;

public partial class PicturesPage : ContentPage
{

    public PicturesPage(PlantPropertyVM property, string selectedPictureName)
	{
        BindingContext = property;

        InitializeComponent();       
          
    }
}