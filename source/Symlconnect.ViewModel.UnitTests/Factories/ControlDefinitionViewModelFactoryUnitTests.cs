using NUnit.Framework;
using Symlconnect.DataModel;
using Symlconnect.ViewModel.Factories;
using Symlconnect.ViewModel.ViewModels;

namespace Symlconnect.ViewModel.UnitTests.Factories
{
    [TestFixture]
    public class ControlDefinitionViewModelFactoryUnitTests
    {
        [Test]
        public void GoodIsControlDefinitionTypeSupported()
        {
            // Arrange
            var sut = new ControlDefinitionViewModelFactory<TestControlDefinitionViewModel, IControlDefinition>();

            // Act
            var result = sut.IsControlDefinitionTypeSupported(typeof(IControlDefinition));

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void BadIsControlDefinitionTypeSupported()
        {
            // Arrange
            var sut = new ControlDefinitionViewModelFactory<TestControlDefinitionViewModel, IControlDefinition>();

            // Act
            var result = sut.IsControlDefinitionTypeSupported(typeof(object));

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void CreateViewModel()
        {
            // Arrange
            var sut = new ControlDefinitionViewModelFactory<TestControlDefinitionViewModel, IControlDefinition>();

            // Act
            var result = sut.CreateViewModel();

            // Assert
            Assert.IsInstanceOf<TestControlDefinitionViewModel>(result);
        }

        private class TestControlDefinitionViewModel : IControlDefinitionViewModel<IControlDefinition>,
            IControlDefinitionViewModel
        {
            public IControlDefinition ControlDefinition { get; set; }
            public object Value { get; set; }
            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public bool IsVisible { get; }

            public bool IsPropertyNameReferenced(string entityName, string propertyName)
            {
                throw new System.NotImplementedException();
            }

            public void NotifyPropertyChanged(string entityName, string propertyName)
            {
                throw new System.NotImplementedException();
            }

            public IFormContext FormContext { get; set; }
            public RuleDefinitionCollection InvalidRuleDefinitions => null;
        }
    }
}