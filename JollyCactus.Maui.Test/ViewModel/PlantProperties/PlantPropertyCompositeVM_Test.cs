using System.Collections.ObjectModel;

using JollyCactus.Maui.ViewModel.PlantProperties;
using JollyCactus.Maui.Model;
using Xunit;
using System.Xml.Linq;

namespace JollyCactus.Maui.Test.ViewModel.PlantProperties
{
    public class PlantPropertyCompositeVM_Tests
    {
        [Fact]
        public void PropertyType_ShouldReturnCompositeType()
        {
            // Arrange
            var composite = new PlantPropertyCompositeVM("Name", "Description", "ParentName");

            // Act
            var propertyType = composite.PropertyType;

            // Assert
            Assert.Equal(PlantPropertyType.PlantPropertyComposite, propertyType);
        }

        [Fact]
        public void AddSubProperty_ShouldAddSubProperty()
        {
            // Arrange
            var composite = new PlantPropertyCompositeVM("Name", "Description", "ParentName");
            var subProperty = new MockPlantPropertyVM("SubName", "SubDescription", "ParentName");

            // Act
            composite.AddSubProperty(subProperty);

            // Assert
            Assert.Single(composite.SubProperties);
            Assert.Equal(subProperty, composite.SubProperties[0]);
        }

        [Fact]
        public void UpdateFrom_ShouldUpdateMatchingSubProperties()
        {
            // Arrange
            var composite = new PlantPropertyCompositeVM("Name", "Description", "ParentName");
            var subProperty = new MockPlantPropertyVM("SubName", "SubDescription", "ParentName");
            composite.AddSubProperty(subProperty);

            var updatedComposite = new PlantPropertyCompositeVM("Name", "Description", "ParentName");
            var updatedSubProperty = new MockPlantPropertyVM("SubName", "UpdatedDescription", "ParentName");
            updatedComposite.AddSubProperty(updatedSubProperty);

            // Act
            composite.UpdateFrom(updatedComposite);

            // Assert
            Assert.Equal("UpdatedDescription", composite.SubProperties[0].Description);
        }

        [Fact]
        public void UpdateFrom_ShouldNotUpdateNonMatchingSubProperties()
        {
            // Arrange
            var composite = new PlantPropertyCompositeVM("Name", "Description", "ParentName");
            var subProperty = new MockPlantPropertyVM("SubName", "SubDescription", "ParentName");
            composite.AddSubProperty(subProperty);

            var updatedComposite = new PlantPropertyCompositeVM("Name", "Description", "ParentName");
            var nonMatchingSubProperty = new MockPlantPropertyVM("NonMatchingName", "UpdatedDescription", "ParentName");
            updatedComposite.AddSubProperty(nonMatchingSubProperty);

            // Act
            composite.UpdateFrom(updatedComposite);

            // Assert
            Assert.Equal("SubDescription", composite.SubProperties[0].Description);
        }

        [Fact]
        public void Clone_ShouldCreateDeepCopy()
        {
            // Arrange
            var composite = new PlantPropertyCompositeVM("Name", "Description", "ParentName");
            var subProperty = new MockPlantPropertyVM("SubName", "SubDescription", "ParentName");
            composite.AddSubProperty(subProperty);

            // Act
            var clone = (PlantPropertyCompositeVM)composite.Clone();

            // Assert
            Assert.Equal(composite.Name, clone.Name);
            Assert.Equal(composite.Description, clone.Description);
            Assert.Single(clone.SubProperties);
            Assert.Equal(subProperty.Name, clone.SubProperties[0].Name);
            Assert.NotSame(subProperty, clone.SubProperties[0]); // Ensure deep copy
        }

        [Fact]
        public void OnSubPropertyChanged_ShouldUpdateIsChanged()
        {
            // Arrange
            var composite = new PlantPropertyCompositeVM("Name", "Description", "ParentName");
            var subProperty = new MockPlantPropertyVM("SubName", "SubDescription", "ParentName");
            composite.AddSubProperty(subProperty);

            // Act
            subProperty.IsChanged = true;

            // Assert
            Assert.True(composite.IsChanged);

            // Act
            subProperty.IsChanged = false;

            // Assert
            Assert.False(composite.IsChanged);
        }
    }

    // Mock implementation of PlantPropertyVM for testing
    internal class MockPlantPropertyVM : PlantPropertyVM
    {
        public MockPlantPropertyVM(string name, string description, string parentName)
            : base(name, description, parentName) { }

        public override PlantPropertyType PropertyType => PlantPropertyType.PlantPropertyString;

        public override string AsPersistenceString() => string.Empty;

        public override void UpdateFrom(PlantPropertyVM property)
        {
            if (property is MockPlantPropertyVM mockProperty)
            {
                Description = mockProperty.Description;
            }
        }

        public override object Clone()
        {
            return new MockPlantPropertyVM(Name, Description, ParentName);
        }
    }
}
