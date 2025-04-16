using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace JollyCactus.Maui.Settings
{
    
    class JCSettings : IDisposable, INotifyPropertyChanged
    {        
        private class Settings
        {
            public ObservableCollection<Account> Accounts { get; set; } = new();
            
            public string CurrentAccountName { get; set; } = "";

        }

        private Settings _settings;

        private Account _currentAccount = null;

        
        public ObservableCollection<Account> Accounts { get => _settings.Accounts; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        

        public string? CurrentDbFullFileName
        {
            get 
            {
                if (CurrentAccount == null)
                {
                    return null;
                }
                return JcAccountPaths.GetFullFileNameByName(CurrentAccount.DatabaseFileName, CurrentAccount);
            }
        }

        public Account CurrentAccount 
        { 
            get => _currentAccount; 
            set
            {
                if (value != _currentAccount)
                {
                    if (value == null)
                    {
                        _currentAccount = value;
                        Save();
                        OnPropertyChanged(nameof(CurrentAccount));
                    }
                    else if (!Accounts.Contains(value))
                    {
                        throw new ArgumentException("Account not found");
                    }
                    else
                    {
                        _currentAccount = value;
                        _settings.CurrentAccountName = _currentAccount.Name;
                        Save();
                        OnPropertyChanged(nameof(CurrentAccount));
                    }
                }
            }
        }

        public string GetFullFileNameByName(string fileName)
        {
            return JcAccountPaths.GetFullFileNameByName(fileName, CurrentAccount);
        }

        public JCSettings()
        {
            if (!File.Exists(JcAccountPaths.GetSettingsFullFileName()))
            {                
                CreateSettingsIfNotExists();
                return;
            }
            try
            {
                //File.Delete(JollyCactusPaths.GetSettingsFullFileName());
                string json = File.ReadAllText(JcAccountPaths.GetSettingsFullFileName());
                _settings = JsonSerializer.Deserialize<Settings>(json);

                if (_settings.Accounts == null)
                {
                    CreateNewAccount();
                }
                else
                {
                    foreach (var account in _settings.Accounts)
                    {
                        account.PropertyChanged += Account_OnPropertyChanged;
                    }

                    if (string.IsNullOrEmpty(_settings.CurrentAccountName))
                    {
                        _settings.CurrentAccountName = _settings.Accounts.First().Name;
                    }
                    else
                    {
                        _currentAccount = _settings.Accounts.FirstOrDefault(
                            a => a.Name == _settings.CurrentAccountName, _settings.Accounts.First());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("JC: Failed to load settings");
                CreateSettingsIfNotExists();
            }
        }

        private void AddAccount(Account account)
        {
            if (_settings.Accounts.Any(a => a.Name == account.Name))
            {
                throw new ArgumentException("Account with this name already exists");
            }

            try
            {
                Directory.CreateDirectory(JcAccountPaths.GetAccountDirectory(account));
            }
            catch (Exception)
            {
                Debug.WriteLine("JC: Failed to save settings");
            }

            _settings.Accounts.Add(account);
            account.PropertyChanged += Account_OnPropertyChanged;

            if (CurrentAccount == null)
            {
                CurrentAccount = account;
            }
            Save();

            OnPropertyChanged(nameof(Accounts));
            OnPropertyChanged(nameof(CurrentAccount));
        }

        private string GenerateNewAccountName()
        {
            string accountName = "My home";
            int i = 1;
            while (_settings.Accounts.Any(a => a.Name == accountName))
            {
                accountName = "My home" + i;
                i++;
            }
            return accountName;
        }

        public void CreateNewAccount()
        {         
            var dir = JcAccountPaths.GetNewAccountDirectoryName();
            var account = new Account(
                GenerateNewAccountName(), JcAccountPaths.GetNewAccountDirectoryName(), JcAccountPaths.GetNewDbFileName());
            AddAccount(account);
            CurrentAccount = account;          
            
        }

        public async Task ImportAccount(string importedAccountDirectory)
        {
            var accountName = Path.GetFileName(importedAccountDirectory);
            var dbFile = Directory.EnumerateFiles(importedAccountDirectory, $"*.{JcPaths.DbExtension}")
                .FirstOrDefault(f => !string.IsNullOrWhiteSpace(f), JcAccountPaths.GetNewDbFileName());
            
            var account = new Account(
                accountName, JcAccountPaths.GetNewAccountDirectoryName(), Path.GetFileName(dbFile));

            await CopyDirectoryAsync(
                importedAccountDirectory,
                JcAccountPaths.GetAccountDirectory(account));

            AddAccount(account);
            CurrentAccount = account;
        }

        public async Task ImportAccount(IEnumerable<FileResult> importedFiles)
        {
            if (importedFiles == null || 
                !importedFiles.Any(f => f.FileName.EndsWith(JcPaths.DbExtension)))
            {
                throw new ArgumentException($"There is no .{JcPaths.DbExtension} file with account information");
            }
            
            //var accountName = Path.GetFileName(importedAccountDirectory);
            var dbFile = importedFiles.FirstOrDefault(f => f.FileName.EndsWith(JcPaths.DbExtension));
            //Directory.EnumerateFiles(importedAccountDirectory, $"*.{JcPaths.DbExtension}")
            //.FirstOrDefault(f => !string.IsNullOrWhiteSpace(f), JollyCactusPaths.GetNewDbFileName());

            var account = new Account(
                GenerateNewAccountName(), JcAccountPaths.GetNewAccountDirectoryName(), dbFile.FileName);
            var accountDirectory = JcAccountPaths.GetAccountDirectory(account);

            try
            {
                Directory.CreateDirectory(accountDirectory);

                foreach (var file in importedFiles)
                {
                    var destFile = Path.Combine(accountDirectory, file.FileName);
                    File.Copy(file.FullPath, destFile, true);
                }
            }
            catch (Exception ex)
            {
                if (Directory.Exists(accountDirectory))
                {
                    Directory.Delete(accountDirectory, true);
                }
                throw new ArgumentException($"Failed to copy files: {ex.Message}");
            }

            AddAccount(account);
            CurrentAccount = account;
        }


        public void RemoveCurrentAccount()
        {
            if (CurrentAccount != null)
                RemoveAccount(CurrentAccount);            
        }

        public void ClearCurrentAccount()
        {
            if (CurrentAccount != null)
                ClearAccount(CurrentAccount);
        }

        public async Task ShareCurrentAccount()
        {
            if (CurrentAccount == null)
            {
                return;
            }
            //await ShareFile(CurrentDbFullFileName, $"JollyCactus-{CurrentAccount.Name}");
            //await ShareFile(JollyCactusPaths.GetAccountDirectory(CurrentAccount.Name), $"JollyCactus-{CurrentAccount.Name}");
            await ShareDirectory(JcAccountPaths.GetAccountDirectory(CurrentAccount), $"JollyCactus-{CurrentAccount.Name}");
        }

        public async Task ExportCurrentAccount(string dirToSave)
        {
            if (CurrentAccount == null)
            {
                return;
            }
            await CopyDirectoryAsync(
                JcAccountPaths.GetAccountDirectory(CurrentAccount),
                dirToSave);
                //Path.Combine(dirToSave, CurrentAccount.Name));            
        }

        //public async Task ImportDatabase(object obj, string fileName)
        //{
        //await ClearAll();
        //_connection.CloseAsync().Wait();

        //var bytes = File.ReadAllBytes(fileName);
        //File.WriteAllBytes(JollyCactusPaths..GetDatabasePath(), bytes);
        //Connect();
        //}

        private async Task CopyDirectoryAsync(string srcDir, string dstDir)
        {
            try
            {
                if (!Directory.Exists(srcDir))
                {
                    throw new DirectoryNotFoundException($"Source directory does not exist: {srcDir}");
                }
                if (!Directory.Exists(dstDir))
                {                   
                    Directory.CreateDirectory(dstDir);                    
                }
                foreach (var file in Directory.GetFiles(srcDir))
                {
                    var destFile = Path.Combine(dstDir, Path.GetFileName(file));
                    File.Copy(file, destFile, true);
                }
                foreach (var dir in Directory.GetDirectories(srcDir))
                {
                    var destDir = Path.Combine(dstDir, Path.GetFileName(dir));
                    await CopyDirectoryAsync(dir, destDir);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"JC: Failed to copy directory: {ex.Message}");
            }
        }

        private async Task ShareDirectory(string fullDirName, string title)
        {

            var files = Directory.GetFiles(fullDirName).Select(f => new ShareFile(f)).ToList();
            //files.Add(new ShareFile(fullDirName));

            await Share.Default.RequestAsync(new ShareMultipleFilesRequest
            {
                Title = title,
                Files = files
            });
        }

        private async Task ShareFile(string fullFileName, string title)
        {
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = title,
                File = new ShareFile(fullFileName)
            });
        }

        private void ClearAccount(Account account)
        {            
            var notDbFiles = Directory.EnumerateFiles(JcAccountPaths.GetAccountDirectory(account)).Where(f => !f.EndsWith(account.DatabaseFileName));
            foreach (var file in notDbFiles)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception)
                {
                    Debug.WriteLine($"JC: Failed to delete file {file}");
                }
            }
        }

        private void RemoveAccount(Account account)
        {
            if (!_settings.Accounts.Contains(account))
            {
                throw new ArgumentException("Account not found");
            }
            _settings.Accounts.Remove(account);
            account.PropertyChanged -= Account_OnPropertyChanged;

            if (CurrentAccount == account)
            {
                if (Accounts.Count > 0)
                {
                    CurrentAccount = Accounts[0];
                }
                else
                {
                    CurrentAccount = null;
                }
            }

            Directory.Delete(JcAccountPaths.GetAccountDirectory(account), true);

            Save();
            OnPropertyChanged(nameof(Accounts));
            OnPropertyChanged(nameof(CurrentAccount));
        }

        private void Save()
        {
            try
            {
                string json = JsonSerializer.Serialize(_settings);
                File.WriteAllText(JcAccountPaths.GetSettingsFullFileName(), json);                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("JC: Failed to save settings");
            }            
        }
               

        private void CreateSettingsIfNotExists()
        {
            _settings = new Settings();            
            CreateNewAccount();
        }

        private void Account_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (CurrentAccount != null)
            {
                _settings.CurrentAccountName = CurrentAccount.Name;
            }
            Save();
            OnPropertyChanged(nameof(Accounts));
            OnPropertyChanged(nameof(CurrentAccount));
        }

        public void Dispose()
        {
            Save();
        }
    }
}
