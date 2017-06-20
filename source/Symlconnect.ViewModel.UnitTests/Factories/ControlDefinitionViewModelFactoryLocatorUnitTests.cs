using System.Collections.Generic;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.ViewModel.Factories;
using Symlconnect.ViewModel.ViewModels;

namespace Symlconnect.ViewModel.UnitTests.Factories
{
    [TestFixture]
    public class ControlDefinitionViewModelFactoryLocatorUnitTests
    {
        private ControlDefinitionViewModelFactoryLocator _sut;
        private List<IControlDefinitionViewModelFactory> _factories;

        [SetUp]
        public void TestSetup()
        {
            _factories = new List<IControlDefinitionViewModelFactory>();
            _sut = new ControlDefinitionViewModelFactoryLocator(_factories);
        }

        [Test]
        public void GoodLocateFactoryForType()
        {
            // Arrange
            var fakeFactory = A.Fake<IControlDefinitionViewModelFactory>();
            A.CallTo(() => fakeFactory.IsControlDefinitionTypeSupported(typeof(TestType)))
                .Returns(true);

            _factories.Add(fakeFactory);

            // Act
            var result = _sut.LocateFactory(typeof(TestType));

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GoodLocateFactoryForTypeMultipleTimes()
        {
            // Arrange
            var fakeFactory = A.Fake<IControlDefinitionViewModelFactory>();
            A.CallTo(() => fakeFactory.IsControlDefinitionTypeSupported(typeof(TestType)))
                .Returns(true);

            _factories.Add(fakeFactory);

            // Act
            var result = _sut.LocateFactory(typeof(TestType));
            var result2 = _sut.LocateFactory(typeof(TestType));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result2);
            Assert.AreSame(result, result2);
        }

        [Test]
        public void BadLocateFactoryForType()
        {
            // Arrange
            var fakeFactory = A.Fake<IControlDefinitionViewModelFactory>();
            A.CallTo(() => fakeFactory.IsControlDefinitionTypeSupported(typeof(TestType)))
                .Returns(true);

            _factories.Add(fakeFactory);

            // Act
            var result = _sut.LocateFactory(typeof(TestType2));

            // Assert
            Assert.IsNull(result);
        }

        private class TestType
        {

        }

        private class TestType2
        {

        }
    }
}