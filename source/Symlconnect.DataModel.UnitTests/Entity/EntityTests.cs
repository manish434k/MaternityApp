using System;
using System.Linq;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Contracts.Environment;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.ObjectModel;

namespace Symlconnect.DataModel.UnitTests.IntegrationTests
{
    [TestFixture]
    public class DataDictionarySerialization
    {
        private IFactory<EntityPropertyValue> _fakeEntityPropertyValueFactory;
        private ICurrentDateTimeProvider _fakeCurrentDateTimeProvider;
        private DateTime _currentDateTime;
        private IFactory<EntityPropertyValueCollection> _fakeEntityValueCollectionFactory;

        private const string ChildEntityCollectionPropertyName = "ChildEntityCollectionPropertyName";

        [SetUp]
        public void TestSetup()
        {
            _currentDateTime = new DateTime(2017, 1, 1);

            _fakeCurrentDateTimeProvider = A.Fake<ICurrentDateTimeProvider>();
            A.CallTo(() => _fakeCurrentDateTimeProvider.GetCurrentDateTime()).ReturnsLazily(() => _currentDateTime);

            _fakeEntityPropertyValueFactory = A.Fake<IFactory<EntityPropertyValue>>();
            A.CallTo(() => _fakeEntityPropertyValueFactory.CreateInstance())
                .ReturnsLazily(() => new EntityPropertyValue());

            _fakeEntityValueCollectionFactory = A.Fake<IFactory<EntityPropertyValueCollection>>();
            A.CallTo(() => _fakeEntityValueCollectionFactory.CreateInstance())
                .ReturnsLazily(
                    () =>
                        new EntityPropertyValueCollection(_fakeCurrentDateTimeProvider, _fakeEntityPropertyValueFactory));
        }

        [Test]
        public void AddChildEntityAndGetChildEntities()
        {
            // Arrange
            var sut = new Entity(_fakeEntityValueCollectionFactory, A.Fake<IFactory<ChildEntityCollection>>(),
                A.Fake<IFactory<ChildEntity>>(), _fakeCurrentDateTimeProvider);

            var fakeChildEntity = ConfigureEntityForChildEntity(sut, ChildEntityCollectionPropertyName,
                "ChildEntityName");

            // Act
            sut.AddChildEntity(ChildEntityCollectionPropertyName, fakeChildEntity, A.Fake<ISessionContext>());
            var childEntities = sut.GetChildEntities(ChildEntityCollectionPropertyName, A.Fake<ISessionContext>());

            // Assert
            Assert.AreEqual(1, childEntities.Count());
            Assert.AreSame(childEntities.First(), fakeChildEntity);
        }

        [Test]
        public void AddChildEntityCollectionThatAlreadyExists()
        {
            // Arrange
            var sut = new Entity(_fakeEntityValueCollectionFactory, A.Fake<IFactory<ChildEntityCollection>>(),
                A.Fake<IFactory<ChildEntity>>(), _fakeCurrentDateTimeProvider);
            ((IChildItemContainer)sut).AddChildItem(new ChildEntityCollection() { PropertyName = "ChildEntityPropertyName" });

            // Act/Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                ((IChildItemContainer) sut).AddChildItem(new ChildEntityCollection()
                {
                    PropertyName = "ChildEntityPropertyName"
                });
            });
        }

        [Test]
        public void GetEmptyChildEntities()
        {
            // Arrange
            var sut = new Entity(_fakeEntityValueCollectionFactory, A.Fake<IFactory<ChildEntityCollection>>(),
                A.Fake<IFactory<ChildEntity>>(), _fakeCurrentDateTimeProvider);

            // Act
            var childEntities = sut.GetChildEntities(ChildEntityCollectionPropertyName, A.Fake<ISessionContext>());

            // Assert
            Assert.AreEqual(0, childEntities.Count());
        }

        [Test]
        public void AddMultipleChildEntities()
        {
            // Arrange
            var sut = new Entity(_fakeEntityValueCollectionFactory, A.Fake<IFactory<ChildEntityCollection>>(),
                A.Fake<IFactory<ChildEntity>>(), _fakeCurrentDateTimeProvider);

            var firstFakeChildEntity = ConfigureEntityForChildEntity(sut, ChildEntityCollectionPropertyName,
                "ChildEntityName");
            var secondFakeChildEntity = A.Fake<IEntity>();
            A.CallTo(() => secondFakeChildEntity.EntityDefinition).Returns(firstFakeChildEntity.EntityDefinition);

            // Act
            sut.AddChildEntity(ChildEntityCollectionPropertyName, firstFakeChildEntity, A.Fake<ISessionContext>());
            sut.AddChildEntity(ChildEntityCollectionPropertyName, secondFakeChildEntity, A.Fake<ISessionContext>());

            // Assert
            var childEntities = sut.GetChildEntities(ChildEntityCollectionPropertyName, A.Fake<ISessionContext>());
            Assert.AreEqual(2, childEntities.Count());
            Assert.AreSame(childEntities.First(), firstFakeChildEntity);
            Assert.AreSame(childEntities.Skip(1).First(), secondFakeChildEntity);
        }

        [Test]
        public void AddChildEntityWithoutEntityProperty()
        {
            // Arrange
            var sut = new Entity(_fakeEntityValueCollectionFactory, A.Fake<IFactory<ChildEntityCollection>>(),
                A.Fake<IFactory<ChildEntity>>(), _fakeCurrentDateTimeProvider);

            var fakeChildEntity = ConfigureEntityForChildEntity(sut, ChildEntityCollectionPropertyName,
                "ChildEntityName");

            sut.EntityDefinition.PropertyDefinitions.Clear();

            // Act/Assert
            Assert.Throws<InvalidOperationException>(
                () =>
                {
                    sut.AddChildEntity(ChildEntityCollectionPropertyName, fakeChildEntity, A.Fake<ISessionContext>());
                });
        }

        [Test]
        public void AddChildEntityWithMismatchedEntityName()
        {
            // Arrange
            var sut = new Entity(_fakeEntityValueCollectionFactory, A.Fake<IFactory<ChildEntityCollection>>(),
                A.Fake<IFactory<ChildEntity>>(), _fakeCurrentDateTimeProvider);

            var fakeChildEntity = ConfigureEntityForChildEntity(sut, ChildEntityCollectionPropertyName,
                "ChildEntityName");

            var childEntityPropertyDefinition =
                (IChildEntityCollectionPropertyDefinition) sut.EntityDefinition.PropertyDefinitions[0];
            A.CallTo(() => childEntityPropertyDefinition.EntityDefinition).Returns(A.Fake<EntityDefinition>());

            // Act/Assert
            Assert.Throws<InvalidOperationException>(
                () =>
                {
                    sut.AddChildEntity(ChildEntityCollectionPropertyName, fakeChildEntity, A.Fake<ISessionContext>());
                });
        }

        /// <summary>
        ///     Placeholder for when this method gets an implementation.
        /// </summary>
        [Test]
        public void ResolveRelatedEntities()
        {
            // Arrange
            var sut = new Entity(_fakeEntityValueCollectionFactory, A.Fake<IFactory<ChildEntityCollection>>(),
                A.Fake<IFactory<ChildEntity>>(), _fakeCurrentDateTimeProvider);

            // Act/Assert
            Assert.Throws<NotImplementedException>(() => { sut.ResolveRelatedEntity(null); });
        }

        [Test]
        public void AddChildItemThatHasAlreadyBeenAdded()
        {
            // Arrange
            var entityPropertyValues = new EntityPropertyValueCollection(_fakeCurrentDateTimeProvider,
                    _fakeEntityPropertyValueFactory)
                {PropertyName = "PropertyName"};

            var sut = new Entity(_fakeEntityValueCollectionFactory, A.Fake<IFactory<ChildEntityCollection>>(),
                A.Fake<IFactory<ChildEntity>>(), _fakeCurrentDateTimeProvider)
            {
                EntityDefinition = A.Fake<IEntityDefinition>()
            };
            A.CallTo(() => sut.EntityDefinition.EntityName).Returns("EntityName");
            ((IChildItemContainer)sut).AddChildItem(entityPropertyValues);

            // Act/Assert
            Assert.Throws<InvalidOperationException>(() => { ((IChildItemContainer)sut).AddChildItem(entityPropertyValues); });
        }

        [Test]
        public void AddChildItemThatHasAlreadyBeenAddedNoEntityDefinition()
        {
            // Arrange
            var entityPropertyValues = new EntityPropertyValueCollection(_fakeCurrentDateTimeProvider,
                    _fakeEntityPropertyValueFactory)
            { PropertyName = "PropertyName" };

            var sut = new Entity(_fakeEntityValueCollectionFactory, A.Fake<IFactory<ChildEntityCollection>>(),
                A.Fake<IFactory<ChildEntity>>(), _fakeCurrentDateTimeProvider);
            ((IChildItemContainer)sut).AddChildItem(entityPropertyValues);

            // Act/Assert
            Assert.Throws<InvalidOperationException>(() => { ((IChildItemContainer)sut).AddChildItem(entityPropertyValues); });
        }

        [Test]
        public void IdPropertyDefaultsToGuid()
        {
            // Arrange
            var sut = new Entity(_fakeEntityValueCollectionFactory, A.Fake<IFactory<ChildEntityCollection>>(),
                A.Fake<IFactory<ChildEntity>>(), _fakeCurrentDateTimeProvider);

            // Act
            var result = sut.Id;
            
            // Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        private IEntity ConfigureEntityForChildEntity(IEntity entity, string childEntityCollectionPropertyName,
            string childEntityName)
        {
            var fakeEntityDefinition = A.Fake<IEntityDefinition>();
            var propertyDefinitions = new PropertyDefinitionCollection();
            A.CallTo(() => fakeEntityDefinition.PropertyDefinitions).Returns(propertyDefinitions);
            var fakePropertyDefinition = A.Fake<IChildEntityCollectionPropertyDefinition>();
            A.CallTo(() => fakePropertyDefinition.Name).Returns(childEntityCollectionPropertyName);
            propertyDefinitions.Add(fakePropertyDefinition);

            entity.EntityDefinition = fakeEntityDefinition;

            var fakeChildEntityDefinition = A.Fake<IEntityDefinition>();
            A.CallTo(() => fakeChildEntityDefinition.EntityName).Returns(childEntityName);
            A.CallTo(() => fakePropertyDefinition.EntityDefinition).Returns(fakeChildEntityDefinition);

            var fakeChildEntity = A.Fake<IEntity>();
            A.CallTo(() => fakeChildEntity.EntityDefinition).Returns(fakeChildEntityDefinition);

            return fakeChildEntity;
        }
    }
}