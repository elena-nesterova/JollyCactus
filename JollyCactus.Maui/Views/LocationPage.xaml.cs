namespace JollyCactus.Maui.Views;

public partial class LocationPage : ContentPage
{
    //private readonly Data.LocalDbService _dbService;
    private readonly ViewModel.JollyCactusVM _viewModel;
    private readonly Model.Location _location;
    
    public LocationPage(ViewModel.JollyCactusVM viewModel, Model.Location? location = null)
	{
		InitializeComponent();

        _viewModel = viewModel;
            

        if (location == null)
        {
            _location = new Model.Location
            {
                Name = "Room 1",
                Note = "",
                //Plants = new()
            };
            
            Title = "New location";
        }
        else
        {
            _location = location;
            Title = "Change location: " + _location.Name;
        }
        BindingContext = _location;
    }

    private async void OnBtnAddLocationOkClicked(object sender, EventArgs e)
    {
        await _viewModel.AddLocation(_location);
        /*if (_location == null)
        {
            await _dbService.SaveLocationAsync(new Model.Location
            {
                Name = entryName.Text,
                Note = editorNote.Text
            });
        }
        else
        {
            await _dbService.SaveLocationAsync(new Model.Location
            {
                Id = _location.Id,
                Name = entryName.Text,
                Note = editorNote.Text
            });
            //_location.Name = entryName.Text;
            //_location.Note = editorNote.Text;

            //await _dbService.SaveLocationAsync(_location);
        }*/

        await Shell.Current.GoToAsync("..");
    }

    private void OnBtnAddLocationExitClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..");
    }
}