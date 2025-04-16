
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace JollyCactus.Maui.ViewModel.PlantProperties
{
    public class PlantPropertyOneString
    {
        public string StringValue { get; set; } = "";
    }

    public class PlantPropertyPictureVM : PlantPropertyVM<string?>
    {
        public ObservableCollection<PlantPropertyOneString> AllPictures { get; private set; }

        public ICommand AddPictureCommand { get; set; }
        public ICommand DeletePictureCommand { get; set; }
        public ICommand SetPictureAsAvatarCommand { get; set; }

        public int PicturesCount { get => AllPictures.Count(); }

        public PlantPropertyPictureVM(string name, string description, string parentName, string persistenceStringValue = "") :
            base(name, description, parentName)
        {
            Value = null;
            AllPictures = new();

            if (!string.IsNullOrEmpty(persistenceStringValue))
            {
                var pictures = persistenceStringValue.Split(',');

                if (pictures.Any())
                {
                    // first element is Value (avatar), next values is gallery
                    Value = pictures.First();
                    bool isContainsValue = false;
                    foreach (var picture in pictures.Skip(1))
                    {
                        AllPictures.Add(new PlantPropertyOneString() { StringValue = picture });
                        if (string.Equals(picture, Value))
                            isContainsValue = true;
                    }  
                    if (!isContainsValue && !string.IsNullOrEmpty(Value))
                        AllPictures.Add(new PlantPropertyOneString() { StringValue = Value });
                }
            }

            AddPictureCommand = new Command<object>(AddPicture);
            DeletePictureCommand = new Command<object>(DeletePicture);
            SetPictureAsAvatarCommand = new Command<object>(SetPictureAsAvatar);
            IsChanged = false;
        }

        public override Model.PlantPropertyType PropertyType => Model.PlantPropertyType.PlantPropertyPicture;


        public override void UpdateFrom(PlantPropertyVM property)
        {
            if (property is PlantPropertyPictureVM propTyped)
            {
                Value = propTyped.Value;

                AllPictures.Clear();
                foreach (var picture in propTyped.AllPictures)
                {
                    AllPictures.Add(new PlantPropertyOneString() { StringValue = picture.StringValue });
                }               
            }
        }

        public override string AsPersistenceString()
        {
            string res = "";

            if (!string.IsNullOrEmpty(Value))
            { 
                res = Value;
            }
            if (AllPictures.Count > 0)
            {
                res += ",";
                res += string.Join(',', AllPictures.Select(x => x.StringValue));
            }
                            
            return res;
        }

        public override object Clone()
        {
            //return MemberwiseClone();
            var clone = new PlantPropertyPictureVM(Name, Description, ParentName, AsPersistenceString());

            return clone;
        }

        private void AddPicture(object obj)
        {
            if (obj is string picture)
            {
                if (!AllPictures.Any(x=>x.StringValue.Equals(picture)))
                {
                    AllPictures.Add(new PlantPropertyOneString() { StringValue = picture});
                    if (string.IsNullOrEmpty(Value))
                    {
                        Value = picture;
                        //OnPropertyChanged(nameof(Value));
                    }

                    IsChanged = true;
                    OnPropertyChanged(nameof(AllPictures));
                }
            }
            
            
        }

        private void DeletePicture(object obj)
        {
            if (obj is string picture)
            {
                var delPicture = AllPictures.First(x => x.StringValue.Equals(picture));
                if (delPicture != null)
                {
                    AllPictures.Remove(delPicture);
                    
                    if (!string.IsNullOrEmpty(Value) && Value.Equals(picture))
                    {
                        Value = null;
                        //OnPropertyChanged(nameof(Value));
                    }
                    IsChanged = true;
                    OnPropertyChanged(nameof(AllPictures));
                }                
            }            
        }

        private void SetPictureAsAvatar(object obj)
        {
            if (obj is string picture)
            {
                var addPicture = AllPictures.Where(x => x.StringValue.Equals(picture));
                if (addPicture == null || !addPicture.Any())
                {
                    AddPicture(picture);
                }
                Value = picture;
               // OnPropertyChanged(nameof(Value));
            }
        }

    }
   
}
