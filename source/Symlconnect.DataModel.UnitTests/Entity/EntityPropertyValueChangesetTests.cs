using NUnit.Framework;
using FakeItEasy;

namespace Symlconnect.DataModel.UnitTests.IntegrationTests
{
    [TestFixture]
    public class EntityPropertyValueChangesetTests
    {
        [Test]
        public void GoodGetNewValueWithNoValue()
        {
            // Arrange
            var sut = new EntityPropertyValueChangeset();

            var fakeEntity = A.Fake<IEntity>();
            A.CallTo(() => fakeEntity.EntityDefinition.EntityName).Returns("EntityName");

            // Act
            var result = sut.GetNewValue(fakeEntity, "PropertyName");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GoodGetNewValueWithMatchingValue()
        {
            // Arrange
            var sut = new EntityPropertyValueChangeset();
            sut.Changes.Add(new EntityPropertyValueChange
            {
                EntityName = "EntityName",
                NewValue = "NewValue",
                PropertyName = "PropertyName"
            });

            var fakeEntity = A.Fake<IEntity>();
            A.CallTo(() => fakeEntity.EntityDefinition.EntityName).Returns("EntityName");

            // Act
            var result = sut.GetNewValue(fakeEntity, "PropertyName");

            // Assert
            Assert.AreEqual("NewValue", result);
        }

        [Test]
        public void GoodGetNewValueWithMatchingValueNullEntityName()
        {
            // Arrange
            var sut = new EntityPropertyValueChangeset();
            sut.Changes.Add(new EntityPropertyValueChange {NewValue = "NewValue", PropertyName = "PropertyName"});

            var fakeEntity = A.Fake<IEntity>();
            A.CallTo(() => fakeEntity.EntityDefinition.EntityName).Returns("EntityName");

            // Act
            var result = sut.GetNewValue(fakeEntity, "PropertyName");

            // Assert
            Assert.AreEqual("NewValue", result);
        }

        [Test]
        public void GoodGetNewValueWithOtherValuesButNoMatchingValue()
        {
            // Arrange
            var sut = new EntityPropertyValueChangeset();
            sut.Changes.Add(new EntityPropertyValueChange
            {
                EntityName = "EntityName",
                NewValue = "NewValue",
                PropertyName = "OtherPropertyName"
            });

            var fakeEntity = A.Fake<IEntity>();
            A.CallTo(() => fakeEntity.EntityDefinition.EntityName).Returns("EntityName");

            // Act
            var result = sut.GetNewValue(fakeEntity, "PropertyName");

            // Assert
            Assert.AreEqual(null, result);
        }

        [Test]
        public void GoodGetNewValueWithMatchingPropertyNameOnAnotherEntity()
        {
            // Arrange
            var sut = new EntityPropertyValueChangeset();
            sut.Changes.Add(new EntityPropertyValueChange
            {
                EntityName = "OtherEntityName",
                NewValue = "NewValue",
                PropertyName = "PropertyName"
            });

            var fakeEntity = A.Fake<IEntity>();
            A.CallTo(() => fakeEntity.EntityDefinition.EntityName).Returns("EntityName");

            // Act
            var result = sut.GetNewValue(fakeEntity, "PropertyName");

            // Assert
            Assert.AreEqual(null, result);
        }
    }
}