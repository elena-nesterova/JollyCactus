
namespace JollyCactus.Maui.ViewModel
{
    public class LocationVM : BaseViewModel
    {        
        public int Id { get; private set; } = 0;

        public string Name { get; set; } = string.Empty;

        public string Note { get; set; } = String.Empty;

        public bool IsRoot { get; set; } = false;

        public int PlantsCount { get; private set; } = 0;

        public Model.Location? Model
        {
            get => _location;
            
        }
        
        private Model.Location? _location;
       
        public LocationVM(Model.Location? location = null)
        {
            if (location != null)
            {
                _location = location;
                Name = location.Name;
                Note = location.Note;
                Id = location.Id;
                PlantsCount = location.Plants.Count;
            }
        }
        
        public void CommitChanges()
        {
            if (!IsRoot)
            {
                if (_location == null)
                    _location = new Model.Location();

                _location.Name = Name;
                _location.Note = Note;               
            }
        }
               
        public void UpdateFromModel(int rootAmount = 0)
        {
            if (IsRoot)
            {
                //if (PlantsCount != rootAmount)
                {
                    PlantsCount = rootAmount;
                    OnPropertyChanged(nameof(PlantsCount));
                }
            }
            else
            {
                if (_location == null)
                {
                    //if (!string.IsNullOrEmpty(Note))
                    {
                        Note = string.Empty;
                        OnPropertyChanged(nameof(Note));
                    }
                }
                else
                {
                    //if (!Name.Equals(_location.Name))
                    {
                        Name = _location.Name;
                        OnPropertyChanged(nameof(Name));
                    }
                    //if (!Note.Equals(_location.Note))
                    {
                        Note = _location.Note;
                        OnPropertyChanged(nameof(Note));
                    }
                    //if (Id != _location.Id)
                    {
                        Id = _location.Id;
                        OnPropertyChanged(nameof(Id));
                    }
                    //if (PlantsCount != _location.Plants.Count)
                    {
                        PlantsCount = _location.Plants.Count;
                        OnPropertyChanged(nameof(PlantsCount));
                    }                    
                }
            }  
        }
    }
}
