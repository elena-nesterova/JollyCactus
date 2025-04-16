using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Storage;
using JollyCactus.Maui.Settings;
using JollyCactus.Maui.ViewModel;
using System.Diagnostics;

namespace JollyCactus.Maui.Views;

public partial class SettingsPage : ContentPage
{
    private JollyCactusVM _viewModel;
    private JCSettings _jcSettings;

    public SettingsPage()
	{
		InitializeComponent();

        var jcSettings = Application.Current.MainPage.Handler.MauiContext.Services.GetService<JCSettings>();
        _jcSettings = jcSettings;
        

        _viewModel = Application.Current.MainPage.Handler.MauiContext.Services.GetService<JollyCactusVM>();

        BindingContext = _jcSettings;
    }

    private async void OnAccountChanged(object sender, EventArgs e)
    {
        await _viewModel.LoadDatabase(_jcSettings.CurrentDbFullFileName); 
    }

    private void OnAddAccountClicked(object sender, EventArgs e)
	{
        _jcSettings.CreateNewAccount();
    }
    
    private async void OnSaveAccountClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FolderPicker.PickAsync(default);
            if (result.IsSuccessful)
            {
                await _jcSettings.ExportCurrentAccount(result.Folder.Path);
            }            
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error of Saving files", ex.Message, "OK");
        }
    }

    private async void OnImportAccountClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickMultipleAsync(new() {PickerTitle = "Please select account files"});
            
            //var result = await FolderPicker.PickAsync(default);
            if (result.Any())
            {
                await _jcSettings.ImportAccount(result/*.Folder.Path*/);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error of importing files.", ex.Message, "OK");
        }
    }


    private async void OnShareAccountClicked(object sender, EventArgs e)
    {
        try
        {
            await _jcSettings.ShareCurrentAccount();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error of sharing files", ex.Message, "OK");
        }

        //_viewModel.ShareDatabaseCommand.Execute(sender);
        //_jcSettings.RemoveCurrentAccount();
    }

   
    private async void OnDeleteAccountClicked(object sender, EventArgs e)
    {
        var res = await DisplayAlert($"Delete account {_jcSettings.CurrentAccount.Name}?", 
            $"Do you really want to delete {_jcSettings.CurrentAccount.Name} with all locations and plants?", 
            "Yes, delete", "No, please, do not delete");
        
       
        if (res == true)
        {
            await _viewModel.CloseDbConnection();
            //_viewModel.ClearDatabaseCommand.Execute(sender);
            _jcSettings.RemoveCurrentAccount();
        }
    }

    private async void OnClearAccountClicked(object sender, EventArgs e)
    {
        var res = await DisplayAlert($"Clear account {_jcSettings.CurrentAccount.Name}?",
            $"Do you really want to delete all locations and plants from {_jcSettings.CurrentAccount.Name}?",
            "Yes, delete", "No, please, do not delete");

        if (res == true)
        {
            await _viewModel.ClearDatabase();
            _jcSettings.ClearCurrentAccount();
        }
    }

}