using System.Linq;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Contracts.ObjectModel;

namespace Symlconnect.DataModel.UnitTests.IntegrationTests
{
    [TestFixture]
    public class ChildEntityTests
    {
        [Test]
        public void GoodIsSupportedChildItem()
        {
            // Arrange
            var sut = (IChildItemContainer) new ChildEntity();

            // Act
            var result = sut.IsSupportedChildItem(A.Fake<IEntity>());

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void BadIsSupportedChildItem()
        {
            // Arrange
            var sut = (IChildItemContainer) new ChildEntity();

            // Act
            var result = sut.IsSupportedChildItem(A.Fake<object>());

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void GoodAddChildItem()
        {
            // Arrange
            var sut = (IChildItemContainer) new ChildEntity();
            var fakeChildItem = A.Fake<IEntity>();

            // Act
            sut.AddChildItem(fakeChildItem);

            // Assert
            Assert.AreSame(fakeChildItem, ((ChildEntity) sut).Entity);
        }

        [Test]
        public void BadAddChildItem()
        {
            // Arrange
            var sut = (IChildItemContainer) new ChildEntity();
            var fakeChildItem = A.Fake<object>();

            // Act
            sut.AddChildItem(fakeChildItem);

            // Assert
            Assert.IsNull(((ChildEntity) sut).Entity);
        }

        [Test]
        public void GetChildItems()
        {
            // Arrange
            var sut = (IChildItemContainer) new ChildEntity();
            var fakeChildItem = A.Fake<IEntity>();
            sut.AddChildItem(fakeChildItem);

            // Act
            var result = sut.GetChildItems();

            // Assert
            Assert.AreSame(fakeChildItem, result.Cast<object>().First());
        }

        [Test]
        public void GetEmptyChildItems()
        {
            // Arrange
            var sut = (IChildItemContainer) new ChildEntity();

            // Act
            var result = sut.GetChildItems();

            // Assert
            CollectionAssert.IsEmpty(result);
        }
    }
}