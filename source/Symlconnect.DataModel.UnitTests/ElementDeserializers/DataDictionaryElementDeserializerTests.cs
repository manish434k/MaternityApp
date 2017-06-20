using System;
using System.Xml.Linq;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;
using Symlconnect.DataModel.Deserializers;
using Symlconnect.UnitTests.Framework.ElementDeserializers;

namespace Symlconnect.DataModel.UnitTests.ElementDeserializers
{
    [TestFixture]
    public class DataDictionaryElementDeserializerTests :
        ElementDeserializationTestBase<DataDictionary, IDataDictionary>
    {
        protected override IElementDeserializer<IDataDictionary> CreateElementDeserializerInstance()
        {
            return new DataDictionaryElementDeserializer(A.Fake<IFactory<DataDictionary>>());
        }

        [Test]
        public void GoodDeserialization()
        {
            // Arrange
            var element = XElement.Parse("<element name=\"namevalue\" />");

            // Act
            var instance =
                (DataDictionary) ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.IsNotNull(instance);
            Assert.AreEqual("namevalue", instance.Name);
        }

        [Test]
        public void MissingNameAttribute()
        {
            // Arrange
            var element = XElement.Parse("<element />");

            // Act / Assert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, null); });
        }
    }
}