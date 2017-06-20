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
    public class ChildEntityCollectionPropertyDefinitionElementDeserializerTests :
        ElementDeserializationTestBase<ChildEntityCollectionPropertyDefinition, IDataDictionary>
    {
        protected override IElementDeserializer<IDataDictionary> CreateElementDeserializerInstance()
        {
            return
                new ChildEntityCollectionPropertyDefinitionElementDeserializer(
                    A.Fake<IFactory<ChildEntityCollectionPropertyDefinition>>());
        }

        [Test]
        public void GoodDeserialization()
        {
            // Arrange
            var element = XElement.Parse("<childentity name=\"namevalue\" entityname=\"entitynamevalue\" />");
            var fakeDataDictionary = A.Fake<IDataDictionary>();
            var entityDefinitions = new EntityDefinitionCollection();
            A.CallTo(() => fakeDataDictionary.EntityDefinitions).Returns(entityDefinitions);
            var fakeEntityDefinition = A.Fake<IEntityDefinition>();
            A.CallTo(() => fakeEntityDefinition.EntityName).Returns("entitynamevalue");
            entityDefinitions.Add(fakeEntityDefinition);

            // Act
            var instance =
                (ChildEntityCollectionPropertyDefinition)
                ElementDeserializer.DeserializeFromXElement(element, null, fakeDataDictionary);

            // Assert
            Assert.IsNotNull(instance);
        }

        [Test]
        public void NullDataDictionary()
        {
            // Arrange
            var element = XElement.Parse("<childentity name=\"namevalue\" entityname=\"entitynamevalue\" />");

            // Act
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, null); });
        }

        [Test]
        public void MissingEntityDefinition()
        {
            // Arrange
            var element = XElement.Parse("<childentity name=\"namevalue\" entityname=\"entitynamevalue\" />");

            // Act
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, A.Fake<IDataDictionary>()); });
        }

        [Test]
        public void MissingEntityNameAttribute()
        {
            // Arrange
            var element = XElement.Parse("<childentity name=\"namevalue\" />");

            // Act / Assert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, A.Fake<IDataDictionary>()); });
        }

        [Test]
        public void MissingNameAttribute()
        {
            // Arrange
            var element = XElement.Parse("<childentity entityname=\"entitynamevalue\" />");

            // Act / Assert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, A.Fake<IDataDictionary>()); });
        }

        [Test]
        public void ElementName()
        {
            // Arrange / Act / Assert
            Assert.AreEqual("childentitycollection", ElementDeserializer.ElementName);
        }
    }
}