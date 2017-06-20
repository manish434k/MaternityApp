using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using FakeItEasy;

namespace Symlconnect.DataModel.UnitTests.PropertyDefinitions
{
    [TestFixture]
    public class VirtualPropertyDefinitionTests : PropertyDefinitionTestsBase
    {
        private const string OriginalPropertyValue = "originalpropertyvalue";

        [Test]
        public void VirtualPropertyWithNoValueProviderPassesValueThrough()
        {
            // Arrange
            var sut = new VirtualPropertyDefinition();

            // Act
            var value = sut.SourceToTarget(FakeEntity, OriginalPropertyValue, FakeSessionContext);

            // Assert
            Assert.AreEqual(OriginalPropertyValue, value);
        }

        [Test]
        public void VirtualPropertySupportsValueKind()
        {
            // Arrange
            var sut = new VirtualPropertyDefinition {PropertyDefinitionKind = ValueKind.DateTime};

            // Act
            var result = sut.PropertyDefinitionKind;

            // Assert
            Assert.AreEqual(result, ValueKind.DateTime);
        }

        [Test]
        public void VirtualPropertyWithAValueProviderUsesTheValueProvider()
        {
            // Arrange
            var sut = new VirtualPropertyDefinition();
            var fakeValueProvider = A.Fake<IValueProvider>();
            sut.ValueProvider = fakeValueProvider;

            // Act
            sut.SourceToTarget(FakeEntity, OriginalPropertyValue, FakeSessionContext);

            // Assert
            A.CallTo(
                    () =>
                        fakeValueProvider.ResolveValue(A<IEntity>.Ignored, A<object>.Ignored, A<ISessionContext>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        /// <summary>
        ///     Test that calling target to source clears the changes collection (so that virtual properties don't get stored away)
        /// </summary>
        [Test]
        public void TargetToSource()
        {
            // Arrange
            var sut = new VirtualPropertyDefinition();
            var fakeChangeset = A.Fake<EntityPropertyValueChangeset>();
            var fakeChanges = A.Fake<IList<EntityPropertyValueChange>>();
            A.CallTo(() => fakeChangeset.Changes).Returns(fakeChanges);

            // Act
            sut.TargetToSource(FakeEntity, fakeChangeset);

            // Assert
            A.CallTo(() => fakeChanges.Clear()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void IsSupportedChildItem()
        {
            // Arrange
            var sut = new VirtualPropertyDefinition();

            // Act/Assert
            Assert.IsTrue(sut.IsSupportedChildItem(A.Fake<IValueProvider>()));
            Assert.IsFalse(sut.IsSupportedChildItem(A.Fake<IPropertyDefinition>()));
        }

        [Test]
        public void AddChildItem()
        {
            // Arrange
            var sut = new VirtualPropertyDefinition();
            var fakeChildItem = A.Fake<IValueProvider>();

            // Act
            sut.AddChildItem(fakeChildItem);

            // Assert
            Assert.AreSame(fakeChildItem, sut.ValueProvider);
        }

        [Test]
        public void AddChildItemTwice()
        {
            // Arrange
            var sut = new VirtualPropertyDefinition();
            var fakeChildItem = A.Fake<IValueProvider>();
            var anotherFakeChildItem = A.Fake<IValueProvider>();
            sut.AddChildItem(fakeChildItem);

            // Act
            sut.AddChildItem(anotherFakeChildItem);

            // Assert
            Assert.AreSame(anotherFakeChildItem, sut.ValueProvider);
        }

        [Test]
        public void GetChildItems()
        {
            // Arrange
            var sut = new VirtualPropertyDefinition();
            var fakeChildItem = A.Fake<IValueProvider>();

            // Act
            sut.AddChildItem(fakeChildItem);

            // Assert
            Assert.AreEqual(1, sut.GetChildItems().Cast<object>().Count());
            Assert.AreSame(fakeChildItem, sut.GetChildItems().Cast<object>().First());
        }
    }
}