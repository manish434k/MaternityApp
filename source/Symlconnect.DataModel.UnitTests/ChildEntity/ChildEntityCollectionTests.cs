using System.Linq;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Contracts.ObjectModel;

namespace Symlconnect.DataModel.UnitTests.IntegrationTests
{
    [TestFixture]
    public class ChildEntityCollectionTests
    {
        [Test]
        public void GoodIsSupportedChildItem()
        {
            // Arrange
            var sut = (IChildItemContainer) new ChildEntityCollection();

            // Act
            bool result = sut.IsSupportedChildItem(A.Fake<ChildEntity>());

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void BadIsSupportedChildItem()
        {
            // Arrange
            var sut = (IChildItemContainer) new ChildEntityCollection();

            // Act
            bool result = sut.IsSupportedChildItem(A.Fake<object>());

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void GoodAddChildItem()
        {
            // Arrange
            var sut = (IChildItemContainer) new ChildEntityCollection();
            var childEntity = A.Fake<ChildEntity>();

            // Act
            sut.AddChildItem(childEntity);

            // Assert
            Assert.AreEqual(1, ((ChildEntityCollection) sut).Count);
            Assert.AreSame(((ChildEntityCollection) sut)[0], childEntity);
        }

        [Test]
        public void BadAddChildItem()
        {
            // Arrange
            var sut = (IChildItemContainer) new ChildEntityCollection();
            var childEntity = A.Fake<object>();

            // Act
            sut.AddChildItem(childEntity);

            // Assert
            CollectionAssert.IsEmpty((ChildEntityCollection) sut);
        }

        [Test]
        public void GoodGetChildItems()
        {
            // Arrange
            var sut = (IChildItemContainer) new ChildEntityCollection();
            var childEntity = A.Fake<ChildEntity>();
            sut.AddChildItem(childEntity);

            // Act
            var result = sut.GetChildItems();

            // Assert
            Assert.AreEqual(1, result.Cast<object>().Count());
            Assert.AreSame(result.Cast<object>().First(), childEntity);
        }

        [Test]
        public void EmptyGetChildItems()
        {
            // Arrange
            var sut = (IChildItemContainer) new ChildEntityCollection();

            // Act
            var result = sut.GetChildItems();

            // Assert
            Assert.AreEqual(0, result.Cast<object>().Count());
        }
    }
}