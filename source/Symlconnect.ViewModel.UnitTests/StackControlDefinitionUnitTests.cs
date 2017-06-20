using System.Linq;
using NUnit.Framework;
using FakeItEasy;

namespace Symlconnect.ViewModel.UnitTests
{
    [TestFixture]
    public class StackControlDefinitionUnitTests
    {
        [Test]
        public void ControlDefinitionIsSupportedChildItem()
        {
            // Arrange
            var sut = new StackControlDefinition();

            // Act
            var result = sut.IsSupportedChildItem(A.Fake<IControlDefinition>());

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void AddControlDefinition()
        {
            // Arrange
            var sut = new StackControlDefinition();

            // Act
            var fakeControlDefinition = A.Fake<IControlDefinition>();
            sut.AddChildItem(fakeControlDefinition);

            // Assert
            Assert.AreSame(fakeControlDefinition, sut.ChildControlDefinitions.First());
        }

        [Test]
        public void GetChildItems()
        {
            // Arrange
            var sut = new StackControlDefinition();

            // Act
            var fakeControlDefinition = A.Fake<IControlDefinition>();
            sut.AddChildItem(fakeControlDefinition);

            // Assert
            CollectionAssert.AreEqual(new object[] {fakeControlDefinition},
                sut.GetChildItems());
        }
    }
}