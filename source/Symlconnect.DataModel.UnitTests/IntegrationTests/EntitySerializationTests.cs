using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FakeItEasy;
using NUnit.Framework;
using Symlconnect.Common.Serialization;
using Symlconnect.Contracts.Diagnostics;
using Symlconnect.Contracts.Environment;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;
using Symlconnect.DataModel.Deserializers;
using Symlconnect.DataModel.Factories;
using Symlconnect.DataModel.Serializers;
using Symlconnect.DataModel.UnitTests.Properties;

namespace Symlconnect.DataModel.UnitTests.SerializationIntegrationTests
{
    [TestFixture]
    public class EntitySerializationTests
    {
        private ICurrentDateTimeProvider _fakeCurrentDateTimeProvider;
        private ILogger _fakeLogger;
        private CommonValueSerializers _commonValueSerializers;

        [SetUp]
        public void TestSetup()
        {
            _fakeCurrentDateTimeProvider = A.Fake<ICurrentDateTimeProvider>();
            A.CallTo(() => _fakeCurrentDateTimeProvider.GetCurrentDateTime()).Returns(new DateTime(2017, 1, 1));

            _fakeLogger = A.Fake<ILogger>();

            _commonValueSerializers = new CommonValueSerializers();
        }

        /// <summary>
        ///     Test full serialization, using the real serializers, of an Entity with contained values.
        /// </summary>
        [Test]
        public void BasicEntitySerialization()
        {
            // Arrange
            var entity = new Entity(A.Fake<EntityPropertyValueCollectionFactory>(),
                A.Fake<IFactory<ChildEntityCollection>>(), A.Fake<IFactory<ChildEntity>>(), _fakeCurrentDateTimeProvider)
            {
                Id = "EntityId",
                CreatedDateTime = new DateTime(2017, 1, 2),
                CreatedByUserId = "CreatedByUserIdValue",
                CreatedByUserDisplayName = "CreatedByUserDisplayNameValue"
            };

            var entityDefinition = new EntityDefinition
            {
                EntityName = "EntityName",
                DataDictionary = new DataDictionary()
            };
            ((DataDictionary) entityDefinition.DataDictionary).Name = "DataDictionaryName";
            entity.EntityDefinition = entityDefinition;
            entityDefinition.DataDictionary.EntityDefinitions.Add(entityDefinition);

            entity.PropertyValues.Add("stringproperty",
                CreateValues("stringproperty", new object[] {"StringValue1", "StringValue2", "StringValue3"}));
            entity.PropertyValues.Add("longproperty", CreateValues("longproperty", new object[] {1, 2, 3}));
            entity.PropertyValues.Add("doubleproperty", CreateValues("doubleproperty", new object[] {1.1, 2.2, 3.3}));
            entity.PropertyValues.Add("boolproperty", CreateValues("boolproperty", new object[] {true, false, true}));
            entity.PropertyValues.Add("dateproperty",
                CreateValues("dateproperty",
                    new object[] {new DateTime(2017, 1, 1), new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)}));

            // Act
            var document = SerializeEntity(entity);

            // Assert
            var expectedRootElement = XElement.Parse(Resources.entity_basic);
            Assert.IsTrue(XNode.DeepEquals(expectedRootElement, document.Root),
                $"Expected {expectedRootElement} was {document.Root}");
        }

        [Test]
        public void BasicEntityWithChildEntitiesSerialization()
        {
            // Arrange
            var entity = new Entity(A.Fake<EntityPropertyValueCollectionFactory>(),
                A.Fake<IFactory<ChildEntityCollection>>(), A.Fake<IFactory<ChildEntity>>(), _fakeCurrentDateTimeProvider)
            {
                Id = "EntityId",
                CreatedDateTime = new DateTime(2017, 1, 2),
                CreatedByUserId = "CreatedByUserIdValue",
                CreatedByUserDisplayName = "CreatedByUserDisplayNameValue"
            };

            var dataDictionary = new DataDictionary {Name = "DataDictionaryName"};

            var entityDefinition = new EntityDefinition
            {
                EntityName = "EntityName",
                DataDictionary = dataDictionary
            };
            entity.EntityDefinition = entityDefinition;
            entityDefinition.DataDictionary.EntityDefinitions.Add(entityDefinition);

            var childEntity = new Entity(A.Fake<EntityPropertyValueCollectionFactory>(),
                A.Fake<IFactory<ChildEntityCollection>>(), A.Fake<IFactory<ChildEntity>>(), _fakeCurrentDateTimeProvider)
            {
                Id = "ChildEntityId",
                CreatedDateTime = new DateTime(2017, 1, 2),
                CreatedByUserId = "CreatedByUserIdValue",
                CreatedByUserDisplayName = "CreatedByUserDisplayNameValue"
            };
            var childEntityDefinition = new EntityDefinition
            {
                EntityName = "ChildEntityName",
                DataDictionary = dataDictionary
            };
            childEntity.EntityDefinition = childEntityDefinition;
            childEntityDefinition.DataDictionary.EntityDefinitions.Add(childEntityDefinition);

            var childEntityCollectionPropertyDefinition = new ChildEntityCollectionPropertyDefinition
            {
                Name = "ChildEntitiesPropertyName",
                EntityDefinition = childEntityDefinition
            };
            entityDefinition.PropertyDefinitions.Add(childEntityCollectionPropertyDefinition);

            var user = new User {UserId = "UserIdValue"};
            var sessionContext = new SessionContext
            {
                SessionId = "SessionIdValue",
                SessionDateTime = new DateTime(2017, 1, 1),
                SessionUser = user
            };
            entity.AddChildEntity("ChildEntitiesPropertyName", childEntity, sessionContext);

            childEntity.PropertyValues.Add("stringproperty",
                CreateValues("stringproperty", new object[] {"StringValue1"}));

            // Act
            var document = SerializeEntity(entity);

            // Assert
            var expectedRootElement = XElement.Parse(Resources.entity_childentities);
            Assert.IsTrue(XNode.DeepEquals(expectedRootElement, document.Root),
                $"Expected {expectedRootElement} was {document.Root}");
        }

        private XDocument SerializeEntity(Entity entity)
        {
            var elementSerializers = new List<IElementSerializer<IEntity>>
            {
                new EntityElementSerializer(new CommonValueSerializers()),
                new EntityPropertyValueCollectionElementSerializer(),
                new EntityPropertyValueElementSerializer(_commonValueSerializers,
                    _commonValueSerializers, _commonValueSerializers, _commonValueSerializers),
                new ChildEntityCollectionElementSerializer(),
                new ChildEntityElementSerializer(_commonValueSerializers)
            };

            var entitySerializer = new EntityDocumentSerializer(elementSerializers, _fakeLogger);

            // Act
            var document = entitySerializer.SerializeToXDocument(entity);
            return document;
        }

        /// <summary>
        ///     Test full deserialization, using the real serializers, of an element representing an Entity with contained values.
        /// </summary>
        [Test]
        public void BasicEntityDeserializationTest()
        {
            // Arrange

            var entityPropertyValueFactory = new EntityPropertyValueFactory();
            var entityPropertyValueCollectionFactory =
                new EntityPropertyValueCollectionFactory(_fakeCurrentDateTimeProvider,
                    entityPropertyValueFactory);
            var entityFactory = new EntityFactory(entityPropertyValueCollectionFactory,
                A.Fake<IFactory<ChildEntityCollection>>(), A.Fake<IFactory<ChildEntity>>(), _fakeCurrentDateTimeProvider);

            var dataDictionaryLocator = A.Fake<IDataDictionaryLocator>();
            var dataDictionary = new DataDictionary {Name = "DataDictionaryName"};
            var entityDefinition = new EntityDefinition
            {
                EntityName = "EntityName",
                DataDictionary = dataDictionary
            };
            dataDictionary.EntityDefinitions.Add(entityDefinition);
            A.CallTo(
                    () =>
                        dataDictionaryLocator.GetDataDictionary(A<string>.That.Matches(s => s == "DataDictionaryName")))
                .Returns(dataDictionary);

            var commonValueDeserializers = new CommonValueDeserializers();
            var elementDeserializers = new List<IElementDeserializer<IEntity>>
            {
                new EntityElementDeserializer(entityFactory, dataDictionaryLocator, new CommonValueDeserializers()),
                new EntityPropertyValueCollectionElementDeserializer(entityPropertyValueCollectionFactory),
                new EntityPropertyValueElementDeserializer(entityPropertyValueFactory,
                    commonValueDeserializers, commonValueDeserializers, commonValueDeserializers,
                    commonValueDeserializers),
                new ChildEntityCollectionElementDeserializer(new ChildEntityCollectionFactory()),
                new ChildEntityElementDeserializer(commonValueDeserializers, new ChildEntityFactory())
            };

            var entityDeserializer = new EntityDocumentDeSerializer(elementDeserializers,
                Enumerable.Empty<IElementGroupDeserializer>(), _fakeLogger);

            // Act
            var referenceDocument = XDocument.Parse(Resources.entity_basic);
            var entity = entityDeserializer.DeserializeFromXDocument(referenceDocument);

            // Assert
            // If all the information in the original entity was preserved, then re-serializing it should result in the same Xml
            var roundTripSerialization = SerializeEntity(entity).Root;
            Assert.IsTrue(XNode.DeepEquals(referenceDocument.Root, roundTripSerialization),
                $"Expected {referenceDocument.Root} was {roundTripSerialization}");
        }

        /// <summary>
        ///     Helper to create a collection containing a set of test values. Each value will have a different SessionId and
        ///     UserId.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="valueVariants">Array of property values.</param>
        /// <returns></returns>
        private EntityPropertyValueCollection CreateValues(string propertyName, object[] valueVariants)
        {
            var values = new EntityPropertyValueCollection(_fakeCurrentDateTimeProvider,
                    A.Fake<IFactory<EntityPropertyValue>>())
                {PropertyName = propertyName};

            for (var x = 0; x < valueVariants.Length; x++)
            {
                values.Add(new EntityPropertyValue
                {
                    SessionId = $"SessionIdValue{x}",
                    UserId = $"UserIdValue{x}",
                    ChangeDateTime = new DateTime(2017, 1, 1 + x),
                    Value = valueVariants[x]
                });
            }
            return values;
        }
    }
}