
using System.Diagnostics;

namespace JollyCactus.Maui.Data
{
    internal class PropertySharedService : IPropertySharedService
    {
        
        private Dictionary<string, object?> _properties { get; set; } = new Dictionary<string, object>();

        public event IPropertySharedService.MethodPropertyChanged OnPropertyChanged;

        public void Add<T>(string key, T value) where T : class
        {
            if (_properties.ContainsKey(key))
            {              

                if (_properties[key] != value)
                {
                    Debug.WriteLine("JC: PropertySharedService.Add - modify - " + key + " " + _properties.Count);
                    _properties[key] = value;                    
                }
                else
                {
                    Debug.WriteLine("JC: PropertySharedService.Add - nothing - " + key + " " + _properties.Count);
                }
                if (OnPropertyChanged != null)
                {
                    OnPropertyChanged(key);
                }
            }
            else
            {
                Debug.WriteLine("JC: PropertySharedService.Add - new - " + key + " " + _properties.Count);
                _properties.Add(key, value);
            }
        }
        public T? GetValue<T>(string key) where T : class
        {
            if (_properties.ContainsKey(key))
            {
                if (_properties[key] != null)
                    return _properties[key] as T;
            }
            return null;
        }

        public T? GetAndRemoveValue<T>(string key) where T : class
        {
            if (_properties.ContainsKey(key))
            {
                T value = null;
                if (_properties[key] != null)
                    value = _properties[key] as T;
                _properties.Remove(key);
                return value;
            }
            return null;
        }
    }
}
