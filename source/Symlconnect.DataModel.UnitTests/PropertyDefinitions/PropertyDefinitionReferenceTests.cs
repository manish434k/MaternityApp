using System;
using FakeItEasy;
using NUnit.Framework;

namespace Symlconnect.DataModel.UnitTests.PropertyDefinitions
{
    [TestFixture]
    public class PropertyDefinitionReferenceTests : PropertyDefinitionTestsBase
    {
        [Test]
        public void TargetToSourceCallsThroughToReferencedPropertyDefinition()
        {
            // Arrange
            var referencedPropertyDefinition = A.Fake<IPropertyDefinition>();
            var sut = new PropertyDefinitionReference {ReferencedPropertyDefinition = referencedPropertyDefinition};

            // Act
            sut.TargetToSource(FakeEntity, A.Fake<EntityPropertyValueChangeset>());

            // Assert
            A.CallTo(
                    () =>
                        referencedPropertyDefinition.TargetToSource(A<IEntity>.Ignored,
                            A<EntityPropertyValueChangeset>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void SourceToTargetCallsThroughToReferencedPropertyDefinition()
        {
            // Arrange
            var referencedPropertyDefinition = A.Fake<IPropertyDefinition>();
            var sut = new PropertyDefinitionReference {ReferencedPropertyDefinition = referencedPropertyDefinition};

            // Act
            sut.SourceToTarget(FakeEntity, null, FakeSessionContext);

            // Assert
            A.CallTo(
                    () =>
                        referencedPropertyDefinition.SourceToTarget(A<IEntity>.Ignored,
                            A<object>.Ignored, A<ISessionContext>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void PropertyDefinitionKindIsNotSettable()
        {
            // Arrange
            var sut = new PropertyDefinitionReference();

            // Act/Assert
            Assert.Throws<NotImplementedException>(() => { sut.PropertyDefinitionKind = ValueKind.DateTime; });
        }
    }
}