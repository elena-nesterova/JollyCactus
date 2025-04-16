using System.Collections.ObjectModel;
using JollyCactus.Maui.ViewModel.PlantProperties;
using JollyCactus.Maui.Model;
using Xunit;

namespace JollyCactus.Maui.Test.ViewModel.PlantProperties
{
    public class PlantPropertyStringsFromListVM_Tests
    {        
        [Fact]
        public void PropertyType_ShouldReturnStringsFromListType()
        {
            // Arrange
            var vm = new PlantPropertyStringsFromListVM("Name", "Description", "ParentName");

            // Act
            var propertyType = vm.PropertyType;

            // Assert
            Assert.Equal(PlantPropertyType.PlantPropertyStringsFromList, propertyType);
        }


        [Theory]
        [InlineData("ParentName", "Value1,Value2", 0)]
        [InlineData("sunlight", "Value1,Value2", 0)] // Use nameof to reference the constant
        [InlineData("sunlight", "fullshade,partialsun", 2)]
        [InlineData("sunlight", "fullshade,Value2", 1)]
        public void Constructor_ShouldInitializeWithPersistenceString(string parentName, string persistenceString, int expectedCount)
        {
            // Arrange
            // Act
            var vm = new PlantPropertyStringsFromListVM("Name", "Description", parentName, persistenceString);
            // Assert
            Assert.Equal(expectedCount, vm.Value.Count);
        }
       

        [Fact]
        public void UpdateFrom_ShouldUpdateValues()
        {
            // Arrange
            var vm = new PlantPropertyStringsFromListVM("Name", "Description", "sunlight");
            var updatedVM = new PlantPropertyStringsFromListVM("Name", "Description", "sunlight", "fullshade,partialsun");

            // Act
            vm.UpdateFrom(updatedVM);

            // Assert
            Assert.Equal(2, vm.Value.Count);
            Assert.Contains(vm.Value, v => v.StringValue == "fullshade");
            Assert.Contains(vm.Value, v => v.StringValue == "partialsun");
        }

        [Fact]
        public void AsPersistenceString_ShouldReturnCorrectString()
        {
            // Arrange
            var vm = new PlantPropertyStringsFromListVM("Name", "Description", "sunlight", "fullshade,partialsun");

            // Act
            var persistenceString = vm.AsPersistenceString();

            // Assert
            Assert.Equal("fullshade,partialsun", persistenceString);
        }

        [Fact]
        public void Clone_ShouldCreateDeepCopy()
        {
            // Arrange
            var vm = new PlantPropertyStringsFromListVM("Name", "Description", "sunlight", "fullshade,partialsun");

            // Act
            var clone = (PlantPropertyStringsFromListVM)vm.Clone();

            // Assert
            Assert.Equal(vm.Name, clone.Name);
            Assert.Equal(vm.Description, clone.Description);
            Assert.Equal(vm.Value.Count, clone.Value.Count);
            Assert.NotSame(vm.Value, clone.Value); // Ensure deep copy
        }

        [Fact]
        public void SelectedValuesChanged_ShouldUpdateValue()
        {
            // Arrange
            var vm = new PlantPropertyStringsFromListVM("Name", "Description", "sunlight", "fullshade");
            vm.SelectedObjects.Add(vm.AllPossibleValues.First(v => v.StringValue == "partialsun"));

            // Act
            vm.SelectedValuesChanged(null);

            // Assert            
            Assert.Equal(2, vm.Value.Count);
            Assert.Contains(vm.Value, v => v.StringValue == "fullshade");
            Assert.Contains(vm.Value, v => v.StringValue == "partialsun");
        }
    }
}

