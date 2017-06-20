using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;

namespace Symlconnect.DataModel.UnitTests.PropertyDefinitions
{
    [TestFixture]
    public class PropertyDefinitionTests : PropertyDefinitionTestsBase
    {
        [Test]
        public void GoodTargetToSource()
        {
            // Arrange
            var sut = new PropertyDefinition();

            // Act            
            var changes = new List<EntityPropertyValueChange>();
            var changeset = A.Fake<EntityPropertyValueChangeset>();
            A.CallTo(() => changeset.Changes).Returns(changes);

            sut.TargetToSource(FakeEntity, changeset);

            // Assert
            // Should not modify changes
            Assert.AreEqual(0, changes.Count);
        }

        [Test]
        public void GoodSourceToTarget()
        {
            // Arrange
            var sut = new PropertyDefinition();

            // Act
            var result = sut.SourceToTarget(FakeEntity, "originalvalue", FakeSessionContext);

            // Assert
            Assert.AreEqual("originalvalue", result);
        }
    }
}