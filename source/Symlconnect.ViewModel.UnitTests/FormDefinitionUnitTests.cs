using System.Linq;
using NUnit.Framework;
using FakeItEasy;

namespace Symlconnect.ViewModel.UnitTests
{
    [TestFixture]
    public class FormDefinitionUnitTests
    {
        [Test]
        public void ControlDefinitionIsSupportedChildItem()
        {
            // Arrange
            var sut = new FormDefinition();

            // Act
            var result = sut.IsSupportedChildItem(A.Fake<IControlDefinition>());

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void AddControlDefinition()
        {
            // Arrange
            var sut = new FormDefinition();

            // Act
            var fakeControlDefinition = A.Fake<IControlDefinition>();
            sut.AddChildItem(fakeControlDefinition);

            // Assert
            Assert.AreSame(fakeControlDefinition, sut.SharedControlDefinitions.First());
        }

        [Test]
        public void AddFormSection()
        {
            // Arrange
            var sut = new FormDefinition();

            // Act
            var fakeFormSectionDefinition = A.Fake<IFormSectionDefinition>();
            sut.AddChildItem(fakeFormSectionDefinition);

            // Assert
            Assert.AreSame(fakeFormSectionDefinition, sut.SectionDefinitions.First());
        }

        [Test]
        public void FormSectionDefinitionIsSupportedChildItem()
        {
            // Arrange
            var sut = new FormDefinition();

            // Act
            var result = sut.IsSupportedChildItem(A.Fake<IFormSectionDefinition>());

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void GetChildItems()
        {
            // Arrange
            var sut = new FormDefinition();

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