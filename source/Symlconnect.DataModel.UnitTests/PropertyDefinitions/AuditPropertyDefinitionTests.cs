using System;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Contracts.Environment;

namespace Symlconnect.DataModel.UnitTests.PropertyDefinitions
{
    [TestFixture]
    public class AuditPropertyDefinitionTests : PropertyDefinitionTestsBase
    {
        private ICurrentDateTimeProvider _fakeCurrentDateTimeProvider;

        [SetUp]
        public override void TestSetup()
        {
            base.TestSetup();

            _fakeCurrentDateTimeProvider = A.Fake<ICurrentDateTimeProvider>();
            A.CallTo(() => _fakeCurrentDateTimeProvider.GetCurrentDateTime())
                .Returns(new DateTime(2017, 1, 1));
        }

        [Test]
        public void PropertyDefinitionKindShouldBeBoolean()
        {
            // Arrange
            var sut = new AuditPropertyDefinition(_fakeCurrentDateTimeProvider);

            // Act / Assert
            Assert.AreEqual(ValueKind.Boolean, sut.PropertyDefinitionKind);
        }

        [Test]
        public void PropertyDefinitionKindIsNotChangable()
        {
            // Arrange
            var sut = new AuditPropertyDefinition(_fakeCurrentDateTimeProvider);

            // Act / Assert
            Assert.Throws<NotImplementedException>(() => { sut.PropertyDefinitionKind = ValueKind.DateTime; });
        }

        [Test]
        public void SourceToTargetShouldPassThroughStoredValue()
        {
            // Arrange
            var sut = new AuditPropertyDefinition(_fakeCurrentDateTimeProvider);

            // Act
            var result = sut.SourceToTarget(FakeEntity, true, FakeSessionContext);

            // Assert
            Assert.IsInstanceOf<bool>(result);
            Assert.IsTrue((bool) result);
        }

        [Test]
        public void TargetToSourceWithTrueValueShouldStoreAdditionalValues()
        {
            // Arrange
            var sut = new AuditPropertyDefinition(_fakeCurrentDateTimeProvider)
            {
                Name = "AuditPropertyName",
                UserIdPropertyDefinition = new PropertyDefinition {Name = "AuditPropertyNameAuditUserId"},
                ChangeDateTimePropertyDefinition = new PropertyDefinition {Name = "AuditPropertyNameAuditDateTime"}
            };
            var changeset = new EntityPropertyValueChangeset {SessionContext = FakeSessionContext};
            changeset.Changes.Add(new EntityPropertyValueChange {PropertyName = "AuditPropertyName", NewValue = true});

            // Act
            sut.TargetToSource(FakeEntity, changeset);

            // Assert
            Assert.AreEqual(3, changeset.Changes.Count);
            Assert.AreEqual(true, changeset.GetNewValue(FakeEntity, "AuditPropertyName"));
            Assert.AreEqual("FakeUserId", changeset.GetNewValue(FakeEntity, "AuditPropertyNameAuditUserId"));
            Assert.AreEqual(new DateTime(2017, 1, 1),
                changeset.GetNewValue(FakeEntity, "AuditPropertyNameAuditDateTime"));
        }

        [Test]
        public void TargetToSourceWithFalseValueShouldClearAdditionalValues()
        {
            // Arrange
            var sut = new AuditPropertyDefinition(_fakeCurrentDateTimeProvider)
            {
                Name = "AuditPropertyName",
                UserIdPropertyDefinition = new PropertyDefinition {Name = "AuditPropertyNameAuditUserId"},
                ChangeDateTimePropertyDefinition = new PropertyDefinition {Name = "AuditPropertyNameAuditDateTime"}
            };
            var changeset = new EntityPropertyValueChangeset {SessionContext = FakeSessionContext};
            changeset.Changes.Add(new EntityPropertyValueChange {PropertyName = "AuditPropertyName", NewValue = false});

            // Act
            sut.TargetToSource(FakeEntity, changeset);

            // Assert
            Assert.AreEqual(3, changeset.Changes.Count);
            Assert.AreEqual(false, changeset.GetNewValue(FakeEntity, "AuditPropertyName"));
            Assert.IsNull(changeset.GetNewValue(FakeEntity, "AuditPropertyNameAuditUserId"));
            Assert.IsNull(changeset.GetNewValue(FakeEntity, "AuditPropertyNameAuditDateTime"));
        }
    }
}