using NUnit.Framework;
using FakeItEasy;
using Symlconnect.DataModel.ValueProviders;

namespace Symlconnect.DataModel.UnitTests.IntegrationTests
{
    [TestFixture]
    public class PropertyValueProviderTests : ValueProviderTestsBase
    {
        [Test]
        public void ValueFromAnotherValidPropertyWithAValue()
        {
            // Arrange
            var sut = new VirtualPropertyDefinition();
            var fakePropertyDefinition = A.Fake<IPropertyDefinition>();
            A.CallTo(() => fakePropertyDefinition.Name).Returns("otherproperty");
            PropertyDefinitions.Add(fakePropertyDefinition);
            A.CallTo(
                    () =>
                        FakeEntity.GetValue(A<string>.That.Matches(s => s == "otherproperty"),
                            A<ISessionContext>.Ignored))
                .Returns("otherpropertyvalue");

            var propertyValueProvider = new PropertyValueProvider
            {
                EntityName = null,
                PropertyName = "otherproperty",
                ValueProviderKind = ValueKind.Text
            };
            sut.ValueProvider = propertyValueProvider;

            // Act
            var value = sut.SourceToTarget(FakeEntity, "originalpropertyvalue", FakeSessionContext);

            // Assert
            Assert.AreEqual("otherpropertyvalue", value);
        }

        [Test]
        public void ValueFromAnotherValidPropertyWithNoValue()
        {
            // Arrange
            var sut = new VirtualPropertyDefinition();
            var fakePropertyDefinition = A.Fake<IPropertyDefinition>();
            A.CallTo(() => fakePropertyDefinition.Name).Returns("otherproperty");
            PropertyDefinitions.Add(fakePropertyDefinition);
            A.CallTo(
                    () =>
                        FakeEntity.GetValue(A<string>.That.Matches(s => s == "otherproperty"),
                            A<ISessionContext>.Ignored))
                .Returns(null);

            var propertyValueProvider = new PropertyValueProvider
            {
                EntityName = null,
                PropertyName = "otherproperty",
                ValueProviderKind = ValueKind.Text
            };
            sut.ValueProvider = propertyValueProvider;

            // Act
            var value = sut.SourceToTarget(FakeEntity, "originalpropertyvalue", FakeSessionContext);

            // Assert
            Assert.AreEqual(null, value);
        }

        [Test]
        public void ValueFromAnInvalidPropertyName()
        {
            // Arrange
            var sut = new VirtualPropertyDefinition();

            var propertyValueProvider = new PropertyValueProvider
            {
                EntityName = null,
                PropertyName = "otherproperty",
                ValueProviderKind = ValueKind.Text
            };
            sut.ValueProvider = propertyValueProvider;

            // Act
            var value = sut.SourceToTarget(FakeEntity, "originalpropertyvalue", FakeSessionContext);

            // Assert
            Assert.AreEqual(null, value);
        }

        [Test]
        public void ValueFromAnotherEntity()
        {
            // Arrange
            var sut = new VirtualPropertyDefinition();

            var propertyValueProvider = new PropertyValueProvider
            {
                EntityName = "AnotherEntity",
                PropertyName = "otherproperty",
                ValueProviderKind = ValueKind.Text
            };
            sut.ValueProvider = propertyValueProvider;

            // Act
            var value = sut.SourceToTarget(FakeEntity, "originalpropertyvalue", FakeSessionContext);

            // Assert
            Assert.AreEqual(null, value);
        }
    }
}