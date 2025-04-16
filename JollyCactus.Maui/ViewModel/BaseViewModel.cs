using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JollyCactus.Maui.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        bool _isBusy = false;
        bool _isChanged = false;
        string _title = "";

        public bool IsBusy { 
            get => _isBusy;
            set
            {
                if (_isBusy ==  value) return;
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged();
            }
        }
        public bool IsChanged
        {
            get => _isChanged;
            set
            {
                if (_isChanged == value) return;
                _isChanged = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
