using JollyCactus.Maui.Settings;

namespace JollyCactus.Maui.Views.PlantPropertyViews;

public partial class ModifyPropertyPictureView : ContentView
{
    public Command LongPressCommand { get; set; }
    public Command TapCommand { get; set; }
    
	public ModifyPropertyPictureView()
	{
		InitializeComponent();

        LongPressCommand = new Command<object>(OnPictureSelected);
        TapCommand = new Command<object>(OnTap);
    }

    private async void DeleteFromGallery(string picture)
    {
        var res = await App.Current.MainPage.DisplayAlert("Delete the picture?", "Do you really want to delete the picture from the gallery?", "Yes", "No");

        if (res == true)
        {
            if (BindingContext is ViewModel.PlantProperties.PlantPropertyPictureVM property)
            {
                property.DeletePictureCommand.Execute(picture);                
            }

            var jcSettings = Application.Current.MainPage.Handler.MauiContext.Services.GetService<JCSettings>();
            if (jcSettings != null)
            {
                string localFilePath = jcSettings.GetFullFileNameByName(picture); // or FileSystem.CacheDirectory
                if (File.Exists(localFilePath))
                {
                    File.Delete(localFilePath);
                }
            }
        }
    }

    private void SetAsAvatar(string picture)
    {
        if (BindingContext is ViewModel.PlantProperties.PlantPropertyPictureVM property)
        {
            property.SetPictureAsAvatarCommand.Execute(picture);            
        }
    }

    private async void OnTap(object sender/*, EventArgs e*/)
    {
        if (BindingContext is ViewModel.PlantProperties.PlantPropertyPictureVM property 
            &&
            sender is ViewModel.PlantProperties.PlantPropertyOneString picture)
        {
            await Navigation.PushAsync(
                new PicturesPage(property, picture.StringValue));
        }
    }
       
    private async void OnPictureSelected(object sender)
    {        
        if (sender is ViewModel.PlantProperties.PlantPropertyOneString picture)
        {
            string action = await App.Current.MainPage.DisplayActionSheet("", "Cancel", null, "Delete picture", "Use as avatar");

            if (action == "Delete picture")
                DeleteFromGallery(picture.StringValue);
            else if (action == "Use as avatar")
                SetAsAvatar(picture.StringValue);
        }
    }
   

    private async void OnChoosePictureClicked(object sender, EventArgs e)
    {
        await TakePhoto(false);        
    }

    private async void OnCameraClicked(object sender, EventArgs e)
    {
        await TakePhoto(true);
    }

    private async Task TakePhoto(bool isCapture)
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult? photo = null; 

            if (isCapture)
                photo = await MediaPicker.Default.CapturePhotoAsync();
            else
                photo = await MediaPicker.Default.PickPhotoAsync();

            if (photo != null) 
            {
                // save the file into local storage
                var jcSettings = Application.Current.MainPage.Handler.MauiContext.Services.GetService<JCSettings>();
                if (jcSettings != null)
                {
                    string localFilePath = jcSettings.GetFullFileNameByName(photo.FileName); // or FileSystem.CacheDirectory

                    using Stream sourceStream = await photo.OpenReadAsync();
                    using FileStream localFileStream = File.OpenWrite(localFilePath);

                    await sourceStream.CopyToAsync(localFileStream);

                    if (BindingContext is ViewModel.PlantProperties.PlantPropertyPictureVM property)
                    {
                        property.AddPictureCommand.Execute(Path.GetFileName(localFilePath));
                        //property.Value = localFilePath;
                    }
                }
            }
        }
    }
}