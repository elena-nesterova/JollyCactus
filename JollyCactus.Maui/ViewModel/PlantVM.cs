using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using JollyCactus.Maui.ViewModel.PlantProperties;

namespace JollyCactus.Maui.ViewModel
{
    public class PlantVM: BaseViewModel
    {
        public ObservableCollection<PropertyGroupVM> PlantPropertiesByGroups { get; set; } = new();
        private Model.Plant _plant;
        private Model.Location _location;

        public string Name { get; set; }
        public string Picture { get; set; }
        public int State { get; set; }

        public Model.Location Location { get; set; }

        public PlantVM(Model.Location location, Model.Plant? plant=null)
        {
            _location = location;

            if (plant == null)
            {
                _plant = new Model.Plant
                {                    
                    Id = 0
                };
            }
            else
            {
                _plant = plant;
            }            
        }

        public async Task LoadPlant()
        {
            //if (_plant.Id != 0)
            //    var props = await App.Database.GetPlantsPropertiesAsync();

            await LoadProperties();


            Debug.WriteLine("LoadPlant");
        }

        public async Task DeletePlant()
        {
            if (_plant.Id != 0)
                await App.Database.DeletePlantAsync(_plant);
        }

        public async Task SavePlant()
        {
            _plant.Properties.Clear();

            foreach (var group in PlantPropertiesByGroups)
            {
                foreach (var prop in group.Properties)
                {
                    Debug.Assert(prop is PlantPropertyCompositeVM);

                    var propComp = prop as PlantPropertyCompositeVM;

                    if (propComp.SubProperties==null)
                        continue;
                    foreach (var propSub in propComp.SubProperties)
                    {
                        string? dbValue = propSub.AsPersistenceString();
                        if (dbValue != null)
                            _plant.Properties.Add(new Model.PlantProperty
                            {
                                PropertyName = propComp.Name,
                                SubName = propSub.Name,
                                PropertyType = propSub.PropertyType,
                                DBValue = dbValue
                            });
                    }
                }
            }

            if (_plant.Id == 0)
                await App.Database.AddPlantAsync(_location, _plant);
            else
                await App.Database.SavePlantAsync(_plant);
        }        

        private PlantPropertyVM? GetPropertyByName(string name)
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

        public void UpdateProperty(PlantPropertyVM changedProperty)
        {
            PlantPropertyVM? dstProperty = GetPropertyByName(changedProperty.Name);
            Debug.Assert(dstProperty != null, "!!! No such property");
            if (dstProperty == null)
            {                
                return;
            }
            // TODO
            dstProperty.UpdateFrom(changedProperty);
            /*if (dstProperty.SubProperties != null && changedProperty.SubProperties != null)
            {
                foreach (var subDstProp in dstProperty.SubProperties)
                {
                    var subSrcProp = changedProperty.SubProperties.FirstOrDefault(x => x.Name.Equals(subDstProp.Name), null);
                    if ( subSrcProp != null)
                    {
                        subDstProp.UpdateFrom(subSrcProp);
                    }                    
                }
            }*/
            OnPropertyChanged(nameof(PlantPropertiesByGroups));
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
                    var plantProps = _plant.Properties.Where(p => string.Equals(p.PropertyName, propDef.Name)).ToList();
                    var propVM = CreatePropertyVM(propDef, plantProps);
                    if (propVM != null)
                        propertyGroupVM.Properties.Add(propVM);
                }
            }

        }

        private PlantPropertyVM? CreatePropertyVM(
            Model.PlantPropertyDefinition propertyDefinition, List<Model.PlantProperty>? plantProperties)
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
                    plantPropVM.SubProperties.Add(plantSubPropertyVM);
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
                        plantPropVM.SubProperties.Add(plantSubPropertyVM);
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
                        plantPropVM.SubProperties.Add(plantSubPropertyVM);
                    }
                }
            }
                    
            
                if (string.Equals(plantPropVM.Name, "name", StringComparison.OrdinalIgnoreCase))
                {
                    //TODO
                    if (plantPropVM.SubProperties[0] is PlantPropertyStringVM subPropTyped)
                        Name = subPropTyped.Value;
                }
                //if (string.Equals(plantPropertyVM.Name, "picture", StringComparison.OrdinalIgnoreCase))
                //{
                    //TODO
                //    Picture = plantPropertyVM.SubProperties[0].Value;
                //    plantPropertyVM.IsDisplay = false;
                //}
           
               
            return plantPropVM;         
        }
       
    }
}
