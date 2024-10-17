using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JollyCactus.Maui.Data
{
    public interface IPropertySharedService
    {
        void Add<T>(string key, T? value) where T : class;
        T? GetValue<T>(string key) where T : class;

        T? GetAndRemoveValue<T>(string key) where T : class;

        public delegate void MethodPropertyChanged(string propertyName);

        public event MethodPropertyChanged OnPropertyChanged;
    }
}
