using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JollyCactus.Maui.Settings
{
    class Account : INotifyPropertyChanged
    {
        [JsonIgnore]
        private string _name;

        [JsonInclude]
        public string DiectoryName { get; set; }

        [JsonInclude]
        public string Name 
        { 
            get => _name; 
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && _name != value)
                {
                    _name = value;

                    OnPropertyChanged(nameof(Name));                    
                }
            }
        }

        [JsonInclude]
        public string DatabaseFileName { get; set; }

        public Account()
        {
            _name = string.Empty;
            DiectoryName = string.Empty;
            DatabaseFileName = string.Empty;
        }

        public Account(string name, string directoryName, string databaseFileName)
        {
            Name = name;
            DiectoryName = (string.IsNullOrWhiteSpace(directoryName)) ? name : directoryName;
            DatabaseFileName = databaseFileName;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        //public delegate void MethodPropertyChanged();
                
        //public event MethodPropertyChanged OnPropertyChanged;
    }
}
