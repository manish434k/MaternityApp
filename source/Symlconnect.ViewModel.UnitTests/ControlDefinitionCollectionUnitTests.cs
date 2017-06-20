using NUnit.Framework;
using FakeItEasy;

namespace Symlconnect.ViewModel.UnitTests
{
    [TestFixture]
    public class ControlDefinitionCollectionUnitTests
    {
        [Test]
        public void CollectionUsesCorrectKey()
        {
            // Arrange
            var sut = new ControlDefinitionCollection();
            var fakeControlDefinition = A.Fake<IControlDefinition>();
            A.CallTo(() => fakeControlDefinition.Id).Returns("idvalue");
            sut.Add(fakeControlDefinition);

            // Act
            var result = sut.Contains("idvalue");

            // Assert
            Assert.IsTrue(result);
            Assert.AreSame(fakeControlDefinition, sut["idvalue"]);
        }
    }
}