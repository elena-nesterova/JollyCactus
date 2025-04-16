using System.Collections.ObjectModel;
using System.Diagnostics;
using JollyCactus.Maui.Model;
using JollyCactus.Maui.ViewModel.PlantProperties;

namespace JollyCactus.Maui.ViewModel
{
    public class PlantVM: BaseViewModel
    {
        public ObservableCollection<PropertyGroupVM> PlantPropertiesByGroups { get; set; } = new();        

        public Model.Plant Model { get => _model; }        

        public string Name { get; private set; } = string.Empty;

        public string BotanicalName { get; private set; } = string.Empty;

        public string Family { get; private set; } = string.Empty;

        public DateTime AdoptionDate { get; private set; }

        public PlantPropertyPictureVM? Picture { get; set; }
        public PlantPropertyOneFromListVM? State { get; set; }

        public uint Amount { get; set; } = 1;

        public string LocationName { get => Location.Name;}

        public LocationVM Location { get => _location;}

        private Model.Plant _model;
        private LocationVM _location;

        public PlantVM(LocationVM location, Model.Plant? plant=null)
        {
            _location = location;
            
            if (plant == null)
            {
                _model = new Model.Plant
                {                    
                    Id = 0
                };
            }
            else
            {
                _model = plant;
            }            
        }

        public async Task LoadPlant()
        {
            //if (_plant.Id != 0)
            //    var props = await App.Database.GetPlantsPropertiesAsync();

            await LoadProperties();


            Debug.WriteLine("JC: PlantVM - LoadPlant");
        }

        public void UpdateProperties()
        {
            _model.Properties.Clear();

            foreach (var group in PlantPropertiesByGroups)
            {
                foreach (var prop in group.Properties)
                {
                    Debug.Assert(prop is PlantPropertyCompositeVM);

                    var propComp = prop as PlantPropertyCompositeVM;

                    if (propComp == null || propComp.SubProperties == null)
                        continue;

                    foreach (var propSub in propComp.SubProperties)
                    {
                        string? dbValue = propSub.AsPersistenceString();
                        if (dbValue != null)
                            _model.Properties.Add(new Model.PlantProperty
                            {
                                PropertyName = propComp.Name,
                                SubName = propSub.Name,
                                PropertyType = propSub.PropertyType,
                                DBValue = dbValue
                            });
                        propSub.IsChanged = false;
                    }
                    propComp.IsChanged = false;
                }
            }            
        }

        public async Task MoveToAsync(LocationVM newLocation)
        {
            if (Location == null || Location.Model == null)
                return;

            if (newLocation == null || newLocation.Id == Location.Id || newLocation.Model == null)
                return;
            
            var oldLocation = Location;

            await App.Database.MovePlantAsync(Model, oldLocation.Model, newLocation.Model);
                                
            _location = newLocation;

            oldLocation.UpdateFromModel();
            _location.UpdateFromModel();
                
            OnPropertyChanged(nameof(Location));
            OnPropertyChanged(nameof(LocationName));
            
        }

        public async Task<bool> UpdatePlant()
        {
            UpdateProperties();

            Debug.Assert(_location != null);

            if (_model.Id == 0)
                return false;
            //await App.Database.AddPlantAsync(_location.Model, _model);
            else
            {
                await App.Database.SavePlantAsync(_model);
                IsChanged = false;
            }
            return true;
        }           

        public PlantPropertyVM? GetPropertyByName(string name)
        {
            PlantPropertyVM? property = null;
            foreach (var group in PlantPropertiesByGroups)
            {
                property = group.Properties.SingleOrDefault(p => p.Name == name);
                if (property != null)
                    break;
            }
            return property;
        }

        private void UpdatePublicProperty(PlantPropertyCompositeVM updatedPropertyComposite)
        {
            switch (updatedPropertyComposite.Name)
            {
                case string str when str.Equals(PlantPropertiesValues.PlantPropertyName, StringComparison.InvariantCultureIgnoreCase):
                    {
                        if (updatedPropertyComposite.SubProperties[0] is PlantPropertyStringVM subPropTyped)
                        {
                            Name = subPropTyped.Value;
                            OnPropertyChanged(nameof(Name));
                        }
                        break;
                    }
                case string str when str.Equals(PlantPropertiesValues.PlantPropertyBotanicalName, StringComparison.InvariantCultureIgnoreCase):
                    {
                        if (updatedPropertyComposite.SubProperties[0] is PlantPropertyStringVM subPropTyped)
                        {
                            BotanicalName = subPropTyped.Value;
                            OnPropertyChanged(nameof(BotanicalName));
                        }
                        break;
                    }
                case string str when str.Equals(PlantPropertiesValues.PlantPropertyFamilyName, StringComparison.InvariantCultureIgnoreCase):
                    {
                        if (updatedPropertyComposite.SubProperties[0] is PlantPropertyStringVM subPropTyped)
                        {
                            Family = subPropTyped.Value;
                            OnPropertyChanged(nameof(Family));
                        }
                        break;
                    }
                case string str when str.Equals(PlantPropertiesValues.PlantPropertyAdoptionDateName, StringComparison.InvariantCultureIgnoreCase):
                    {
                        if (updatedPropertyComposite.SubProperties[0] is PlantPropertyDateVM subPropTyped)
                        {
                            AdoptionDate = subPropTyped.Value;
                            OnPropertyChanged(nameof(AdoptionDate));
                        }
                        break;
                    }
                case string str when str.Equals(PlantPropertiesValues.PlantPropertyAmountName, StringComparison.InvariantCultureIgnoreCase):
                    {
                        if (updatedPropertyComposite.SubProperties[0] is PlantPropertyNumberVM subPropTyped)
                        {
                            Amount = subPropTyped.Value;
                            OnPropertyChanged(nameof(Amount));
                        }
                        break;
                    }
                case string str when str.Equals(PlantPropertiesValues.PlantPropertyPictureName, StringComparison.InvariantCultureIgnoreCase):
                    {
                        if (updatedPropertyComposite.SubProperties[0] is PlantPropertyPictureVM subPropTyped)
                        {
                            Picture = subPropTyped;
                            OnPropertyChanged(nameof(Picture));
                        }
                        updatedPropertyComposite.IsDisplay = false;
                        break;
                    }
                case string str when str.Equals(PlantPropertiesValues.PlantPropertyStateName, StringComparison.InvariantCultureIgnoreCase):
                    {
                        if (updatedPropertyComposite.SubProperties[0] is PlantPropertyOneFromListVM subPropTyped)
                        {
                            State = subPropTyped;
                            OnPropertyChanged(nameof(State));
                        }
                        updatedPropertyComposite.IsDisplay = false;
                        break;
                    }
                default:
                    break;

            }
        }

        public void UpdateProperty(PlantPropertyVM changedProperty)
        {
            PlantPropertyVM? dstProperty = GetPropertyByName(changedProperty.Name);
            Debug.Assert(dstProperty != null, "!!! No such property");
            if (dstProperty == null)
            {                
                return;
            }
            
            dstProperty.UpdateFrom(changedProperty);
            
            OnPropertyChanged(nameof(PlantPropertiesByGroups));

            if (dstProperty is PlantPropertyCompositeVM compPropTyped)
                UpdatePublicProperty(compPropTyped);
            
        }        

        private async Task LoadProperties()
        {
            var groupsDef = await App.Database.GetPlantPropertyGroupsListAsync();

            PlantPropertiesByGroups.Clear();

            foreach (var groupDef in groupsDef)
            {
                PropertyGroupVM propertyGroupVM = new PropertyGroupVM
                {
                    GroupName = groupDef.Name,                    
                    Properties = new()
                };
                PlantPropertiesByGroups.Add(propertyGroupVM);

                foreach (var propDef in groupDef.Properties)
                {
                    var plantProps = _model.Properties.Where(p => string.Equals(p.PropertyName, propDef.Name)).ToList();
                    var propVM = CreatePropertyVM(propDef, plantProps);
                    if (propVM != null)
                        propertyGroupVM.Properties.Add(propVM);
                }
            }
        }

        private PlantPropertyVM? CreatePropertyVM(
            PlantPropertyDefinition propertyDefinition, List<PlantProperty>? plantProperties)
        {
            PlantPropertyCompositeVM plantPropVM = PlantPropertyVMFactory.GetPropertyCompositeVM(propertyDefinition);
            
            PlantPropertyVM? plantSubPropertyVM = null;
                

            if (propertyDefinition.SubProperties == null && (plantProperties == null || plantProperties.Count == 0))
            {
                plantSubPropertyVM = PlantPropertyVMFactory.GetPropertyVM(
                    propertyDefinition,
                    (plantProperties != null && plantProperties.Count > 0) ? plantProperties[1] : null);
                if (plantSubPropertyVM != null)
                {   
                    plantPropVM.AddSubProperty(plantSubPropertyVM);
                }
            }
            else if (propertyDefinition.SubProperties != null)
            {
                foreach(var subPropDef in propertyDefinition.SubProperties)
                {
                    var plantProp = (plantProperties != null) ? 
                        plantProperties.FirstOrDefault(p => string.Equals(p.SubName, subPropDef.SubName), null) : null;
                    plantSubPropertyVM = PlantPropertyVMFactory.GetPropertyVM(subPropDef, plantProp);                    
                    if (plantSubPropertyVM != null)
                    {
                        plantPropVM.AddSubProperty(plantSubPropertyVM);
                    }
                }
            }
            else if (plantProperties != null)
            {
                foreach (var plantProp in plantProperties)
                {
                    plantSubPropertyVM = PlantPropertyVMFactory.GetPropertyVM(propertyDefinition, plantProp);
                    if (plantSubPropertyVM != null)
                    {
                        plantPropVM.AddSubProperty(plantSubPropertyVM);
                    }
                }
            }

            //TODO
            switch (plantPropVM.Name)
            {
                case string str when str.Equals(PlantPropertiesValues.PlantPropertyName, StringComparison.InvariantCultureIgnoreCase):
                    {
                        if (plantPropVM.SubProperties[0] is PlantPropertyStringVM subPropTyped)
                        {
                            Name = subPropTyped.Value;
                            OnPropertyChanged(nameof(Name));
                        }
                        break;
                    }
                case string str when str.Equals(PlantPropertiesValues.PlantPropertyAmountName, StringComparison.InvariantCultureIgnoreCase):
                    {
                        if (plantPropVM.SubProperties[0] is PlantPropertyNumberVM subPropTyped)
                        {
                            Amount = subPropTyped.Value;
                            OnPropertyChanged(nameof(Amount));
                        }
                        break;
                    }
                case string str when str.Equals(PlantPropertiesValues.PlantPropertyPictureName, StringComparison.InvariantCultureIgnoreCase):
                    {
                        if (plantPropVM.SubProperties[0] is PlantPropertyPictureVM subPropTyped)
                        {
                            Picture = subPropTyped;
                            OnPropertyChanged(nameof(Picture));
                        }
                        plantPropVM.IsDisplay = false;
                        break;
                    }
                case string str when str.Equals(PlantPropertiesValues.PlantPropertyStateName, StringComparison.InvariantCultureIgnoreCase):
                    {
                        if (plantPropVM.SubProperties[0] is PlantPropertyOneFromListVM subPropTyped)
                        {
                            State = subPropTyped;
                            OnPropertyChanged(nameof(State));
                        }
                        plantPropVM.IsDisplay = false;
                        break;
                    }
                default:                    
                    break;

            }
                           
            return plantPropVM;         
        }
       
    }
}
