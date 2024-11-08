namespace JollyCactus.Maui.Views.PlantPropertyViews;

public partial class ModifyPropertyPictureView : ContentView
{
	public ModifyPropertyPictureView()
	{
		InitializeComponent();
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
            FileResult photo;

            if (isCapture) 
                photo = await MediaPicker.Default.CapturePhotoAsync();
            else
                photo = await MediaPicker.Default.PickPhotoAsync();

            if (photo != null)
            {
                // save the file into local storage
                string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);

                await sourceStream.CopyToAsync(localFileStream);

                if (BindingContext is ViewModel.PlantProperties.PlantPropertyPictureVM property)
                {
                    property.Value = localFilePath;
                }
            }
        }
    }
}