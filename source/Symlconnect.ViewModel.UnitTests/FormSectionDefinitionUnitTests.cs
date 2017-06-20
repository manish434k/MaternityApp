using System.Linq;
using FakeItEasy;
using NUnit.Framework;

namespace Symlconnect.ViewModel.UnitTests
{
    public class FormSectionDefinitionUnitTests
    {
        [Test]
        public void ControlDefinitionIsSupportedChildItem()
        {
            // Arrange
            var sut = new FormSectionDefinition();

            // Act
            var result = sut.IsSupportedChildItem(A.Fake<IControlDefinition>());

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void AddControlDefinition()
        {
            // Arrange
            var sut = new FormSectionDefinition();

            // Act
            var fakeControlDefinition = A.Fake<IControlDefinition>();
            sut.AddChildItem(fakeControlDefinition);

            // Assert
            Assert.AreSame(fakeControlDefinition, sut.ControlDefinitions.First());
        }

        [Test]
        public void AddFormSection()
        {
            // Arrange
            var sut = new FormSectionDefinition();

            // Act
            var fakeFormSectionDefinition = A.Fake<IFormSectionDefinition>();
            sut.AddChildItem(fakeFormSectionDefinition);

            // Assert
            Assert.AreSame(fakeFormSectionDefinition, sut.ChildSectionDefinitions.First());
        }

        [Test]
        public void FormSectionDefinitionIsSupportedChildItem()
        {
            // Arrange
            var sut = new FormSectionDefinition();

            // Act
            var result = sut.IsSupportedChildItem(A.Fake<IFormSectionDefinition>());

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void GetChildItems()
        {
            // Arrange
            var sut = new FormSectionDefinition();

            // Act
            var fakeControlDefinition = A.Fake<IControlDefinition>();
            sut.AddChildItem(fakeControlDefinition);
            var fakeFormSectionDefinition = A.Fake<IFormSectionDefinition>();
            sut.AddChildItem(fakeFormSectionDefinition);

            // Assert
            CollectionAssert.AreEqual(new object[] {fakeControlDefinition, fakeFormSectionDefinition},
                sut.GetChildItems());
        }
    }
}