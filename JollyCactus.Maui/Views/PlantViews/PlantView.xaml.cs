using JollyCactus.Maui.ViewModel;
using JollyCactus.Maui.ViewModel.PlantProperties;
using JollyCactus.Maui.Model;

namespace JollyCactus.Maui.Views.PlantViews;

public partial class PlantView : ContentView
{
	public string BotanicalName { get; private set; }
    public string Family { get; private set; }
    public PlantPropertyVM Sunlight { get; private set; }
    public PlantPropertyVM Watering { get; private set; }
       

    public PlantView()
	{
        InitializeComponent();     
  	}

    protected override void OnBindingContextChanged()
    {       

        if (BindingContext is PlantVM plant)
        {            
            var prop = plant.GetPropertyByName(PlantPropertiesValues.PlantPropertySunlightName);
            if (prop != null)
            {
                if (prop is PlantPropertyCompositeVM comp)
                {
                    // TODO: show current season
                    if (comp.SubProperties.Count > 0)
                    {
                        Sunlight = comp.SubProperties[0];
                        OnPropertyChanged(nameof(Sunlight));
                    }
                }
            }

            prop = plant.GetPropertyByName(PlantPropertiesValues.PlantPropertyWateringName);
            if (prop != null)
            {
                if (prop is PlantPropertyCompositeVM comp)
                {
                    // TODO: show current season
                    if (comp.SubProperties.Count > 0)
                    {
                        Watering = comp.SubProperties[0];
                        OnPropertyChanged(nameof(Watering));
                    }
                }
            }

            prop = plant.GetPropertyByName(PlantPropertiesValues.PlantPropertyBotanicalName);
            if (prop != null)
            {
                if (prop is PlantPropertyCompositeVM comp)
                {
                    if (comp.SubProperties.Count > 0)
                        if (comp.SubProperties[0] is PlantPropertyStringVM str)
                        {
                            BotanicalName = str.Value;
                            OnPropertyChanged(nameof(BotanicalName));
                        }
                }
            }

            prop = plant.GetPropertyByName(PlantPropertiesValues.PlantPropertyFamilyName);
            if (prop != null)
            {
                if (prop is PlantPropertyCompositeVM comp)
                {
                    if (comp.SubProperties.Count > 0)
                        if (comp.SubProperties[0] is PlantPropertyStringVM str)
                        {
                            Family = str.Value;
                            OnPropertyChanged(nameof(Family));
                        }
                }
            }
        }

        base.OnBindingContextChanged();

    } 

}