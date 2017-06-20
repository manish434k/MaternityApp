using System.Xml.Linq;
using FakeItEasy;
using NUnit.Framework;
using Symlconnect.Contracts.Environment;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel.Serializers;

namespace Symlconnect.DataModel.UnitTests.ElementSerializers
{
    [TestFixture]
    public class EntityPropertyValueCollectionSerializerTests
    {
        private ICurrentDateTimeProvider _fakeCurrentDateTimeProvider;
        private IFactory<EntityPropertyValue> _fakeEntityPropertyValueFactory;

        [SetUp]
        public void TestSetup()
        {
            _fakeCurrentDateTimeProvider = A.Fake<ICurrentDateTimeProvider>();
            _fakeEntityPropertyValueFactory = A.Fake<IFactory<EntityPropertyValue>>();
        }

        [Test]
        public void GoodSerialization()
        {
            // Arrange
            var collection = new EntityPropertyValueCollection(_fakeCurrentDateTimeProvider,
                _fakeEntityPropertyValueFactory) {PropertyName = "PropertyNameValue"};
            var sut = new EntityPropertyValueCollectionElementSerializer();

            // Act
            var element = sut.SerializeToXElement(collection, null, null);

            // Assert
            var expectedElement = XElement.Parse("<values propertyname=\"PropertyNameValue\"/>");
            Assert.IsTrue(XNode.DeepEquals(expectedElement, element), $"Expected {expectedElement} was {element}");
        }

        [Test]
        public void BadSerialization()
        {
            // Arrange
            var sut = new EntityPropertyValueCollectionElementSerializer();

            // Act
            var element = sut.SerializeToXElement(A.Fake<object>(), null, null);

            // Assert
            Assert.IsNull(element);
        }
    }
}