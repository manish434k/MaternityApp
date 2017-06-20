using System;
using FakeItEasy;
using NUnit.Framework;
using Symlconnect.Contracts.Environment;
using Symlconnect.Contracts.Factories;

namespace Symlconnect.DataModel.UnitTests.EntityTests
{
    [TestFixture]
    public class EntityPropertyTests
    {
        private Entity _sut;
        private IEntityDefinition _fakeEntityDefinition;
        private IUser _fakeUser;
        private ICurrentDateTimeProvider _fakeCurrentDateTimeProvider;
        private IFactory<EntityPropertyValue> _fakeEntityPropertyValueFactory;
        private const string PropertyName = "PropertyName";
        private DateTime _currentDateTime;

        [SetUp]
        public void TestSetup()
        {
            _currentDateTime = new DateTime(2017, 1, 1);

            _fakeUser = A.Fake<IUser>();
            A.CallTo(() => _fakeUser.UserId).Returns("FakeUserId");

            _fakeCurrentDateTimeProvider = A.Fake<ICurrentDateTimeProvider>();
            A.CallTo(() => _fakeCurrentDateTimeProvider.GetCurrentDateTime()).ReturnsLazily(() => _currentDateTime);

            _fakeEntityDefinition = A.Fake<IEntityDefinition>();
            A.CallTo(() => _fakeEntityDefinition.EntityName).Returns("TestEntityName");

            var fakePropertyDefinition = A.Fake<IPropertyDefinition>();

            _fakeEntityPropertyValueFactory = A.Fake<IFactory<EntityPropertyValue>>();
            A.CallTo(() => _fakeEntityPropertyValueFactory.CreateInstance())
                .ReturnsLazily(() => new EntityPropertyValue());

            // Fake a basic propertydefinition that just passes through calls to SourceToTarget and TargetToSource
            A.CallTo(() => fakePropertyDefinition.PropertyDefinitionKind).Returns(ValueKind.Text);
            A.CallTo(() => fakePropertyDefinition.Name).Returns(PropertyName);
            A.CallTo(
                    () =>
                        fakePropertyDefinition.SourceToTarget(A<IEntity>.Ignored, A<object>.Ignored,
                            A<ISessionContext>.Ignored))
                .ReturnsLazily((IEntity entity, object value, ISessionContext sessionContext) => value);

            var fakePropertyDefinitions = new PropertyDefinitionCollection { fakePropertyDefinition };
            A.CallTo(() => _fakeEntityDefinition.PropertyDefinitions).Returns(fakePropertyDefinitions);

            var fakeEntityValueCollectionFactory = A.Fake<IFactory<EntityPropertyValueCollection>>();
            A.CallTo(() => fakeEntityValueCollectionFactory.CreateInstance())
                .ReturnsLazily(
                    () =>
                        new EntityPropertyValueCollection(_fakeCurrentDateTimeProvider, _fakeEntityPropertyValueFactory));

            _sut = new Entity(fakeEntityValueCollectionFactory, A.Fake<IFactory<ChildEntityCollection>>(),
                A.Fake<IFactory<ChildEntity>>(), _fakeCurrentDateTimeProvider)
            {
                EntityDefinition = _fakeEntityDefinition
            };
        }

        [Test]
        public void BasicPropertyRoundTrip()
        {
            const string propertyValue = "TestValue";

            // Arrange
            var fakeSessionContext = A.Fake<ISessionContext>();

            // Act
            var result = _sut.SetValue(PropertyName, propertyValue, fakeSessionContext);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(propertyValue, _sut.GetValue(PropertyName, fakeSessionContext));
        }

        [Test]
        public void StorePropertyValueForMultipleSessions()
        {
            const string originalValue = "OriginalValue";
            const string newValue = "TestValue";

            // Arrange
            StorePropertyValueForPreviousSession("PreviousSessionId", new DateTime(2017, 1, 1), PropertyName,
                originalValue);

            _currentDateTime = new DateTime(2017, 1, 1, 1, 0, 0);

            var fakeSessionContext = A.Fake<ISessionContext>();
            A.CallTo(() => fakeSessionContext.SessionId).Returns("CurrentSessionId");
            A.CallTo(() => fakeSessionContext.SessionDateTime).Returns(_currentDateTime);
            A.CallTo(() => fakeSessionContext.SessionUser).Returns(_fakeUser);

            // Act
            var valueBefore = _sut.GetValue(PropertyName, fakeSessionContext);
            var result = _sut.SetValue(PropertyName, newValue, fakeSessionContext);

            Assert.AreEqual(originalValue, valueBefore);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(newValue, _sut.GetValue(PropertyName, fakeSessionContext));
        }

        [Test]
        public void DontStorePropertyValueIfSameAsPreviousSession()
        {
            const string originalValue = "OriginalValue";

            // Arrange
            StorePropertyValueForPreviousSession("PreviousSessionId", _currentDateTime, PropertyName, originalValue);

            _currentDateTime = new DateTime(2017, 1, 1, 1, 0, 0);

            var fakeSessionContext = A.Fake<ISessionContext>();
            A.CallTo(() => fakeSessionContext.SessionId).Returns("CurrentSessionId");
            A.CallTo(() => fakeSessionContext.SessionDateTime).Returns(_currentDateTime);
            A.CallTo(() => fakeSessionContext.SessionUser).Returns(_fakeUser);

            // Act
            var result = _sut.SetValue(PropertyName, originalValue, fakeSessionContext);

            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.ChangeSet.WasStoreUpdatedForAllChanges);
        }

        [Test]
        public void StoreMultipleValuesForTheSameSession()
        {
            const string originalValue = "OriginalValue";

            // Arrange
            StorePropertyValueForPreviousSession("PreviousSessionId", _currentDateTime, PropertyName, originalValue);

            _currentDateTime = new DateTime(2017, 1, 1, 1, 0, 0);

            var fakeSessionContext = A.Fake<ISessionContext>();
            A.CallTo(() => fakeSessionContext.SessionId).Returns("CurrentSessionId");
            A.CallTo(() => fakeSessionContext.SessionUser).Returns(_fakeUser);

            // Act
            var originalResult = _sut.SetValue(PropertyName, "firstvalue", fakeSessionContext);
            var secondResult = _sut.SetValue(PropertyName, "newvalue", fakeSessionContext);

            Assert.IsTrue(originalResult.IsSuccess);
            Assert.IsTrue(secondResult.IsSuccess);

            Assert.AreEqual("newvalue", _sut.GetValue(PropertyName, fakeSessionContext));
        }

        [Test]
        public void StoreMultipleValuesForTheSameSessionWithSameValue()
        {
            const string originalValue = "OriginalValue";

            // Arrange
            StorePropertyValueForPreviousSession("PreviousSessionId", _currentDateTime, PropertyName, originalValue);

            _currentDateTime = new DateTime(2017, 1, 1, 1, 0, 0);

            var fakeSessionContext = A.Fake<ISessionContext>();
            A.CallTo(() => fakeSessionContext.SessionId).Returns("CurrentSessionId");
            A.CallTo(() => fakeSessionContext.SessionUser).Returns(_fakeUser);

            // Act
            var originalResult = _sut.SetValue(PropertyName, "firstvalue", fakeSessionContext);
            var secondResult = _sut.SetValue(PropertyName, "firstvalue", fakeSessionContext);

            Assert.IsTrue(originalResult.IsSuccess);
            Assert.IsTrue(originalResult.ChangeSet.WasStoreUpdatedForAllChanges);
            Assert.IsTrue(secondResult.IsSuccess);
            Assert.IsFalse(secondResult.ChangeSet.WasStoreUpdatedForAllChanges);

            Assert.AreEqual("firstvalue", _sut.GetValue(PropertyName, fakeSessionContext));
        }

        [Test]
        public void RevertStoredValueForASession()
        {
            const string originalValue = "OriginalValue";

            // Arrange
            StorePropertyValueForPreviousSession("PreviousSessionId", _currentDateTime, PropertyName, originalValue);

            _currentDateTime = new DateTime(2017, 1, 1, 1, 0, 0);

            var fakeSessionContext = A.Fake<ISessionContext>();
            A.CallTo(() => fakeSessionContext.SessionId).Returns("CurrentSessionId");
            A.CallTo(() => fakeSessionContext.SessionUser).Returns(_fakeUser);
            A.CallTo(() => fakeSessionContext.SessionDateTime).Returns(_currentDateTime);

            // Saving this value at 1am in a new session
            var originalResult = _sut.SetValue(PropertyName, "firstsessionvalue", fakeSessionContext);

            // Now bumping the session id and datetime
            A.CallTo(() => fakeSessionContext.SessionId).Returns("NewSessionId");
            A.CallTo(() => fakeSessionContext.SessionDateTime).Returns(new DateTime(2017, 1, 2));
            _currentDateTime = new DateTime(2017, 1, 2, 0, 0, 0);

            // Act
            // Storing a new value on 2/1
            var secondResult = _sut.SetValue(PropertyName, "newvalue", fakeSessionContext);
            // Now reverting to the original value - should remove the previous value
            var thirdResult = _sut.SetValue(PropertyName, "firstsessionvalue", fakeSessionContext);

            Assert.IsTrue(originalResult.IsSuccess);
            Assert.IsTrue(secondResult.IsSuccess);
            Assert.IsTrue(thirdResult.IsSuccess);

            Assert.AreEqual("firstsessionvalue", _sut.GetValue(PropertyName, fakeSessionContext));

            // TODO: Would like to validate that the user/session stored against the value, but that detail is opaque
        }

        [Test]
        public void RetrieveValueInAPreviousSession()
        {
            const string originalValue = "OriginalValue";

            // Arrange
            StorePropertyValueForPreviousSession("PreviousSessionId", _currentDateTime, PropertyName, originalValue);

            _currentDateTime = new DateTime(2017, 1, 1, 1, 0, 0);

            // New session context, different to the previous session
            var fakeSessionContext = A.Fake<ISessionContext>();
            A.CallTo(() => fakeSessionContext.SessionId).Returns("CurrentSessionId");
            A.CallTo(() => fakeSessionContext.SessionDateTime).Returns(_currentDateTime);
            A.CallTo(() => fakeSessionContext.SessionUser).Returns(_fakeUser);

            // Act
            var result = _sut.GetValue(PropertyName, fakeSessionContext);

            // Assert
            Assert.AreEqual(originalValue, result);
        }

        [Test]
        public void RetrieveValueNoValuesExist()
        {            
            _currentDateTime = new DateTime(2017, 1, 1, 1, 0, 0);

            // New session context, different to the previous session
            var fakeSessionContext = A.Fake<ISessionContext>();
            A.CallTo(() => fakeSessionContext.SessionId).Returns("CurrentSessionId");
            A.CallTo(() => fakeSessionContext.SessionDateTime).Returns(_currentDateTime);
            A.CallTo(() => fakeSessionContext.SessionUser).Returns(_fakeUser);

            // Act
            var result = _sut.GetValue(PropertyName, fakeSessionContext);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void StorePropertyValueForMultipleSessionsInDifferentDateTimeOrder()
        {
            const string firstValue = "FirstValue";
            const string secondValue = "SecondValue";

            // Arrange

            // First save is at midnight on 1/1/17
            StorePropertyValueForPreviousSession("FirstSessionId", _currentDateTime, PropertyName, firstValue);

            // Act

            // Second save is at midnight on 31/12/16
            _currentDateTime = new DateTime(2016, 12, 31, 0, 0, 0);
            StorePropertyValueForPreviousSession("SecondSessionId", _currentDateTime, PropertyName, secondValue);

            // Assert
            // Set up a new session context just so we can query the property value without
            //  having the results influenced by the current session being equal to a value
            //  stored.
            var fakeSessionContext = A.Fake<ISessionContext>();
            A.CallTo(() => fakeSessionContext.SessionId).Returns("CurrentSessionId");
            A.CallTo(() => fakeSessionContext.SessionDateTime).Returns(new DateTime(2017, 1, 1));
            A.CallTo(() => fakeSessionContext.SessionUser).Returns(_fakeUser);

            Assert.AreEqual(firstValue, _sut.GetValue(PropertyName, fakeSessionContext));
        }

        [Test]
        public void InvalidPropertyNameGetValue()
        {
            // Arrange
            var fakeSessionContext = A.Fake<ISessionContext>();

            // Act / Assert
            Assert.Throws<InvalidOperationException>(() => { _sut.GetValue("InvalidPropertyName", fakeSessionContext); });
        }

        [Test]
        public void InvalidPropertyNameSetValue()
        {
            // Arrange
            var fakeSessionContext = A.Fake<ISessionContext>();

            // Act / Assert
            Assert.Throws<InvalidOperationException>(
                () => { _sut.SetValue("InvalidPropertyName", "Value", fakeSessionContext); });
        }

        [Test]
        public void OnPropertyChangedEvent()
        {
            const string propertyValue = "TestValue";

            // Arrange
            var fakeSessionContext = A.Fake<ISessionContext>();
            var eventCount = 0;
            _sut.EntityPropertyChanged += (sender, args) => eventCount++;

            // Act
            _sut.SetValue(PropertyName, propertyValue, fakeSessionContext);

            // Assert
            Assert.AreEqual(1, eventCount);
        }

        private void StorePropertyValueForPreviousSession(string sessionId, DateTime sessionDateTime,
            string propertyName, object value)
        {
            var sessionContext = A.Fake<ISessionContext>();

            A.CallTo(() => sessionContext.SessionId).Returns(sessionId);
            A.CallTo(() => sessionContext.SessionDateTime).Returns(sessionDateTime);
            A.CallTo(() => sessionContext.SessionUser).Returns(_fakeUser);

            _sut.SetValue(propertyName, value, sessionContext);

            Assert.AreEqual(value, _sut.GetValue(propertyName, sessionContext));
        }
    }
}