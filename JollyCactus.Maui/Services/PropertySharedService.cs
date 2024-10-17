
namespace JollyCactus.Maui.Data
{
    internal class PropertySharedService : IPropertySharedService
    {
        //public delegate void MethodContainer();

        //Событие OnCount c типом делегата MethodContainer.
        //public event MethodContainer onCount;

        private Dictionary<string, object?> _properties { get; set; } = new Dictionary<string, object>();

        public event IPropertySharedService.MethodPropertyChanged OnPropertyChanged;

        public void Add<T>(string key, T value) where T : class
        {
            if (_properties.ContainsKey(key))
            {
                //if (_properties[key] != value)
                //{
                //    _properties[key] = value;
                    if (OnPropertyChanged != null)
                    {
                        OnPropertyChanged(key);
                    }
                //}
            }
            else
            {
                _properties.Add(key, value);
            }
        }
        public T? GetValue<T>(string key) where T : class
        {
            if (_properties.ContainsKey(key))
            {
                return _properties[key] as T;
            }
            return null;
        }

        public T? GetAndRemoveValue<T>(string key) where T : class
        {
            if (_properties.ContainsKey(key))
            {                
                T value = _properties[key] as T;
                _properties.Remove(key);
                return value;
            }
            return null;
        }
    }
}
