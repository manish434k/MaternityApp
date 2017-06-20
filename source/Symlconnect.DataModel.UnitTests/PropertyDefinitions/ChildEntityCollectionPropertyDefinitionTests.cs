using System;
using NUnit.Framework;

namespace Symlconnect.DataModel.UnitTests.PropertyDefinitions
{
    [TestFixture]
    public class ChildEntityCollectionPropertyDefinitionTests : PropertyDefinitionTestsBase
    {
        [Test]
        public void GoodTargetToSource()
        {
            // Arrange
            var sut = new ChildEntityCollectionPropertyDefinition();

            // Act/Assert
            Assert.Throws<NotImplementedException>(() => { sut.TargetToSource(null, null); });
        }

        [Test]
        public void GoodSourceToTarget()
        {
            // Arrange
            var sut = new ChildEntityCollectionPropertyDefinition();

            // Act/Assert
            Assert.Throws<NotImplementedException>(() => { sut.SourceToTarget(null, null, null); });
        }

        [Test]
        public void PropertyDefinitionKindGet()
        {
            // Arrange
            var sut = new ChildEntityCollectionPropertyDefinition();

            // Act
            var valueKind = sut.PropertyDefinitionKind;

            // Assert
            Assert.AreEqual(ValueKind.ChildEntityCollection, valueKind);
        }

        [Test]
        public void PropertyDefinitionKindSet()
        {
            // Arrange
            var sut = new ChildEntityCollectionPropertyDefinition();

            // Act/Assert
            Assert.Throws<NotImplementedException>(() => { sut.PropertyDefinitionKind = ValueKind.DateTime; });
        }
    }
}