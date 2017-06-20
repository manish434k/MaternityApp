using System;
using System.Xml.Linq;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Common.Serialization;
using Symlconnect.Contracts.Serialization;
using Symlconnect.DataModel.Deserializers;
using Symlconnect.UnitTests.Framework.ElementDeserializers;

namespace Symlconnect.DataModel.UnitTests.ElementDeserializers
{
    [TestFixture]
    public class EntityElementDeserializerTests : ElementDeserializationTestBase<Entity, Entity>
    {
        private IDataDictionaryLocator _fakeDictionaryLocator;
        private IDataDictionary _fakeDictionary;
        private EntityDefinitionCollection _entityDefinitions;
        private IEntityDefinition _fakeEntityDefinition;

        protected override void TestSetup()
        {
            _entityDefinitions = new EntityDefinitionCollection();
            _fakeEntityDefinition = A.Fake<IEntityDefinition>();
            A.CallTo(() => _fakeEntityDefinition.EntityName).Returns("entityname");
            _entityDefinitions.Add(_fakeEntityDefinition);

            _fakeDictionary = A.Fake<IDataDictionary>();
            A.CallTo(() => _fakeDictionary.EntityDefinitions).Returns(_entityDefinitions);

            _fakeDictionaryLocator = A.Fake<IDataDictionaryLocator>();
            A.CallTo(() => _fakeDictionaryLocator.GetDataDictionary(A<string>.Ignored)).Returns(_fakeDictionary);

            base.TestSetup();
        }

        protected override IElementDeserializer<Entity> CreateElementDeserializerInstance()
        {
            return new EntityElementDeserializer(FakeFactory, _fakeDictionaryLocator, new CommonValueDeserializers());
        }

        [Test]
        public void GoodDeserialization()
        {
            // Arrange
            var element =
                XElement.Parse(
                    "<entity id=\"entityidvalue\" entityname=\"entityname\" datadictionary=\"datadictionaryname\" createddatetime=\"2017-01-02T00:00:00.0000000Z\" createdbyuserid=\"CreatedByUserIdValue\" createdbyuserdisplayname=\"CreatedByUserDisplayNameValue\" />");

            // Act
            var entity = (Entity) ElementDeserializer.DeserializeFromXElement(element, null, null);

            // Assert
            Assert.AreSame(_fakeEntityDefinition, entity.EntityDefinition);
        }

        [Test]
        public void MissingDataDictionaryAtribute()
        {
            // Arrange
            var element = XElement.Parse("<entity id=\"entityidvalue\" entityname=\"entityname\" />");

            // Act / Assert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, null); });
        }

        [Test]
        public void MissingDataDictionary()
        {
            // Arrange
            var element =
                XElement.Parse(
                    "<entity id=\"entityidvalue\" entityname=\"entityname\" datadictionary=\"datadictionaryname\" createddatetime=\"2017-01-02T00:00:00.0000000Z\" createdbyuserid=\"CreatedByUserIdValue\" createdbyuserdisplayname=\"CreatedByUserDisplayNameValue\" />");
            A.CallTo(() => _fakeDictionaryLocator.GetDataDictionary(A<string>.Ignored)).Returns(null);

            // Act / Assert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, null); });
        }

        [Test]
        public void MissingEntityNameAttribute()
        {
            // Arrange
            var element =
                XElement.Parse(
                    "<entity id=\"entityidvalue\" datadictionary=\"datadictionaryname\" createddatetime=\"2017-01-02T00:00:00.0000000Z\" createdbyuserid=\"CreatedByUserIdValue\" createdbyuserdisplayname=\"CreatedByUserDisplayNameValue\"/>");

            // Act / Assert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, null); });
        }

        [Test]
        public void MissingEntityDefinition()
        {
            // Arrange
            var element =
                XElement.Parse(
                    "<entity id=\"entityidvalue\" entityname=\"entityname\" datadictionary=\"datadictionaryname\" createddatetime=\"2017-01-02T00:00:00.0000000Z\" createdbyuserid=\"CreatedByUserIdValue\" createdbyuserdisplayname=\"CreatedByUserDisplayNameValue\"/>");
            _entityDefinitions.Clear();

            // Act / Assert
            Assert.Throws<InvalidOperationException>(
                () => { ElementDeserializer.DeserializeFromXElement(element, null, null); });
        }
    }
}