namespace JollyCactus.Maui.Views.PropertyViews;

public partial class PlantPropertyView : ContentView
{
	public PlantPropertyView()
	{
		InitializeComponent();
	}
/*
    public static readonly BindableProperty DataTemplateSelectorProperty = BindableProperty.Create(nameof(DataTemplateSelector), typeof(DataTemplateSelector), typeof(PlantPropertyView), null, propertyChanged: DataTemplateSelectorChanged);
    public static readonly BindableProperty DataTemplateProperty = BindableProperty.Create(nameof(DataTemplate), typeof(DataTemplate), typeof(PlantPropertyView), null, propertyChanged: DataTemplateChanged);
    public static readonly BindableProperty DataProperty = BindableProperty.Create(nameof(Data), typeof(object), typeof(PlantPropertyView), null, propertyChanged: DataChanged);

    public DataTemplateSelector DataTemplateSelector
    {
        get => (DataTemplateSelector)GetValue(DataTemplateSelectorProperty);
        set => SetValue(DataTemplateSelectorProperty, value);
    }

    public DataTemplate DataTemplate
    {
        get => (DataTemplate)GetValue(DataTemplateProperty);
        set => SetValue(DataTemplateProperty, value);
    }

    public object Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }*/
/*
    private static void DataTemplateSelectorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PlantPropertyView contentPresenter && newValue is DataTemplateSelector dataTemplateSelector)
        {
            BindableLayout.SetItemTemplateSelector(contentPresenter.HostGrid, dataTemplateSelector);
        }
    }
    private static void DataTemplateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PlantPropertyView contentPresenter && newValue is DataTemplate dataTemplate)
        {
            BindableLayout.SetItemTemplate(contentPresenter.HostGrid, dataTemplate);
        }
    }

    private static void DataChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PlantPropertyView contentPresenter)
        {
            BindableLayout.SetItemsSource(contentPresenter.HostGrid, new object[] { newValue });
        }
    }*/
}