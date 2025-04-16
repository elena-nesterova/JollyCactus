using System.Collections.ObjectModel;
using JollyCactus.Maui.ViewModel.PlantProperties;
using JollyCactus.Maui.Model;
using Xunit;

namespace JollyCactus.Maui.Test.ViewModel.PlantProperties
{
    public class PlantPropertyPictureVM_Tests
    {
        [Fact]
        public void PropertyType_ShouldReturnPictureType()
        {
            // Arrange
            var pictureVM = new PlantPropertyPictureVM("Name", "Description", "ParentName");

            // Act
            var propertyType = pictureVM.PropertyType;

            // Assert
            Assert.Equal(PlantPropertyType.PlantPropertyPicture, propertyType);
        }

        [Fact]
        public void Constructor_ShouldInitializeWithPersistenceString()
        {
            // Arrange
            string persistenceString = "avatar.jpg,pic1.jpg,pic2.jpg";

            // Act
            var pictureVM = new PlantPropertyPictureVM("Name", "Description", "ParentName", persistenceString);

            // Assert
            Assert.Equal("avatar.jpg", pictureVM.Value);
            Assert.Equal(3, pictureVM.PicturesCount);
        }

        [Fact]
        public void AddPicture_ShouldAddNewPicture()
        {
            // Arrange
            var pictureVM = new PlantPropertyPictureVM("Name", "Description", "ParentName");

            // Act
            pictureVM.AddPictureCommand.Execute("newPicture.jpg");

            // Assert
            Assert.Single(pictureVM.AllPictures);
            Assert.Equal("newPicture.jpg", pictureVM.AllPictures[0].StringValue);
        }

        [Fact]
        public void AddPicture_ShouldNotAddDuplicatePicture()
        {
            // Arrange
            var pictureVM = new PlantPropertyPictureVM("Name", "Description", "ParentName");
            pictureVM.AddPictureCommand.Execute("duplicate.jpg");

            // Act
            pictureVM.AddPictureCommand.Execute("duplicate.jpg");

            // Assert
            Assert.Single(pictureVM.AllPictures);
        }

        [Fact]
        public void DeletePicture_ShouldRemovePicture()
        {
            // Arrange
            var pictureVM = new PlantPropertyPictureVM("Name", "Description", "ParentName");
            pictureVM.AddPictureCommand.Execute("pictureToDelete.jpg");

            // Act
            pictureVM.DeletePictureCommand.Execute("pictureToDelete.jpg");

            // Assert
            Assert.Empty(pictureVM.AllPictures);
        }

        [Fact]
        public void DeletePicture_ShouldClearAvatarIfDeleted()
        {
            // Arrange
            var pictureVM = new PlantPropertyPictureVM("Name", "Description", "ParentName");
            pictureVM.AddPictureCommand.Execute("avatar.jpg");
            pictureVM.SetPictureAsAvatarCommand.Execute("avatar.jpg");

            // Act
            pictureVM.DeletePictureCommand.Execute("avatar.jpg");

            // Assert
            Assert.Null(pictureVM.Value);
        }

        [Fact]
        public void SetPictureAsAvatar_ShouldSetAvatar()
        {
            // Arrange
            var pictureVM = new PlantPropertyPictureVM("Name", "Description", "ParentName");
            pictureVM.AddPictureCommand.Execute("avatar.jpg");

            // Act
            pictureVM.SetPictureAsAvatarCommand.Execute("avatar.jpg");

            // Assert
            Assert.Equal("avatar.jpg", pictureVM.Value);
        }

        [Fact]
        public void UpdateFrom_ShouldUpdatePicturesAndAvatar()
        {
            // Arrange
            var pictureVM = new PlantPropertyPictureVM("Name", "Description", "ParentName");
            pictureVM.AddPictureCommand.Execute("oldPicture.jpg");

            var updatedVM = new PlantPropertyPictureVM("Name", "Description", "ParentName", "newAvatar.jpg,newPicture.jpg");

            // Act
            pictureVM.UpdateFrom(updatedVM);

            // Assert
            Assert.Equal("newAvatar.jpg", pictureVM.Value);
            Assert.Equal(2, pictureVM.PicturesCount);
        }

        [Fact]
        public void AsPersistenceString_ShouldReturnCorrectString()
        {
            // Arrange
            var pictureVM = new PlantPropertyPictureVM("Name", "Description", "ParentName");
            pictureVM.AddPictureCommand.Execute("avatar.jpg");
            pictureVM.AddPictureCommand.Execute("gallery1.jpg");
            pictureVM.SetPictureAsAvatarCommand.Execute("avatar.jpg");

            // Act
            var persistenceString = pictureVM.AsPersistenceString();

            // Assert
            Assert.Equal("avatar.jpg,avatar.jpg,gallery1.jpg", persistenceString);
        }

        [Fact]
        public void Clone_ShouldCreateDeepCopy()
        {
            // Arrange
            var pictureVM = new PlantPropertyPictureVM("Name", "Description", "ParentName");
            pictureVM.AddPictureCommand.Execute("avatar.jpg");
            pictureVM.AddPictureCommand.Execute("gallery1.jpg");

            // Act
            var clone = (PlantPropertyPictureVM)pictureVM.Clone();

            // Assert
            Assert.Equal(pictureVM.Name, clone.Name);
            Assert.Equal(pictureVM.Description, clone.Description);
            Assert.Equal(pictureVM.PicturesCount, clone.PicturesCount);
            Assert.NotSame(pictureVM.AllPictures, clone.AllPictures); // Ensure deep copy
        }
    }
}
