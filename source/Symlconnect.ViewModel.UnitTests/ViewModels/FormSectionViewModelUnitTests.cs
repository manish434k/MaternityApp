using System;
using System.Linq;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Contracts.Factories;
using Symlconnect.ViewModel.ViewModels;

namespace Symlconnect.ViewModel.UnitTests.ViewModels
{
    [TestFixture]
    public class FormSectionViewModelUnitTests
    {
        private FormSectionViewModel _sut;
        private IControlDefinitionViewModelFactoryLocator _fakeControlDefinitionViewModelFactoryLocator;
        private IControlDefinitionViewModelFactory _fakeControlDefinitionViewModelFactory;
        private IFormSectionDefinition _formSection;
        private FormSectionDefinitionCollection _childFormSectionDefinitions;
        private ControlDefinitionCollection _controlDefinitions;
        private IFormContext _fakeFormContext;
        private IFactory<FormSectionViewModel> _formSectionViewModelFactory;

        [SetUp]
        public void TestSetup()
        {
            _fakeControlDefinitionViewModelFactoryLocator = A.Fake<IControlDefinitionViewModelFactoryLocator>();
            _fakeControlDefinitionViewModelFactory = A.Fake<IControlDefinitionViewModelFactory>();
            A.CallTo(() => _fakeControlDefinitionViewModelFactoryLocator.LocateFactory(A<Type>.Ignored))
                .Returns(_fakeControlDefinitionViewModelFactory);
            _formSectionViewModelFactory = A.Fake<IFactory<FormSectionViewModel>>();
            _sut = new FormSectionViewModel(_formSectionViewModelFactory,
                _fakeControlDefinitionViewModelFactoryLocator);

            _childFormSectionDefinitions = new FormSectionDefinitionCollection();
            _controlDefinitions = new ControlDefinitionCollection();

            // Create a fake form section and map the form sections and control sections to fields in this type so our tests can easily 
            // add test sections and controls
            _formSection = A.Fake<IFormSectionDefinition>();
            A.CallTo(() => _formSection.ChildSectionDefinitions).Returns(_childFormSectionDefinitions);
            A.CallTo(() => _formSection.ControlDefinitions).Returns(_controlDefinitions);

            _sut.FormSectionDefinition = _formSection;
            _fakeFormContext = A.Fake<IFormContext>();
            _sut.FormContext = _fakeFormContext;
        }

        [Test]
        public void FormContextIsAppliedToNewChildViewModels()
        {
            // Arrange
            var fakeChildSectionDefinition = A.Fake<IFormSectionDefinition>();
            _childFormSectionDefinitions.Add(fakeChildSectionDefinition);
            var fakeControlDefinition = A.Fake<IControlDefinition>();
            _controlDefinitions.Add(fakeControlDefinition);

            var fakeFormContext = A.Fake<IFormContext>();

            // Act
            _sut.FormContext = fakeFormContext;

            // Assert
            Assert.AreSame(_sut.ChildFormSectionViewModels[0].FormContext, fakeFormContext);
            Assert.AreSame(_sut.ControlDefinitionViewModels[0].FormContext, fakeFormContext);
        }

        [Test]
        public void ChangingFormContextPropogatesToChildViewModels()
        {
            // Arrange
            var fakeChildSectionDefinition = A.Fake<IFormSectionDefinition>();
            _childFormSectionDefinitions.Add(fakeChildSectionDefinition);
            var fakeControlDefinition = A.Fake<IControlDefinition>();
            _controlDefinitions.Add(fakeControlDefinition);

            var formSectionViewModels = _sut.ChildFormSectionViewModels;
            var controlViewModels = _sut.ControlDefinitionViewModels;
            var fakeFormContext = A.Fake<IFormContext>();

            // Act
            _sut.FormContext = fakeFormContext;

            // Assert
            CollectionAssert.IsNotEmpty(formSectionViewModels);
            CollectionAssert.IsNotEmpty(controlViewModels);
            Assert.AreSame(_sut.ChildFormSectionViewModels[0].FormContext, fakeFormContext);
            Assert.AreSame(_sut.ControlDefinitionViewModels[0].FormContext, fakeFormContext);
        }

        [Test]
        public void BadFindControlDefinitionViewModelsWitNoMatchingControlDefinitions()
        {
            // Arrange
            A.CallTo(() => _fakeControlDefinitionViewModelFactory.CreateViewModel()).ReturnsLazily(
                () =>
                {
                    var fakeViewModel = A.Fake<IControlDefinitionViewModel>();
                    A.CallTo(
                        () =>
                            fakeViewModel.IsPropertyNameReferenced(A<string>.Ignored,
                                A<string>.That.Matches(pn => pn == "PropertyNameValue"))).Returns(false);
                    return fakeViewModel;
                });
            _controlDefinitions.Add(CreateFakeControlDefinitionWithMatchingValuePropertyName("PropertyNameValue"));

            // Act
            var result = _sut.FindControlDefinitionViewModels(null, "PropertyNameValue").ToList();

            // Assert
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void GoodFindControlDefinitionViewModelsWithMatchingControlDefinition()
        {
            // Arrange
            A.CallTo(() => _fakeControlDefinitionViewModelFactory.CreateViewModel()).ReturnsLazily(
                () =>
                {
                    var fakeViewModel = A.Fake<IControlDefinitionViewModel>();
                    A.CallTo(
                        () =>
                            fakeViewModel.IsPropertyNameReferenced(A<string>.Ignored,
                                A<string>.That.Matches(pn => pn == "PropertyNameValue"))).Returns(true);
                    return fakeViewModel;
                });
            _controlDefinitions.Add(CreateFakeControlDefinitionWithMatchingValuePropertyName("PropertyNameValue"));

            // Act
            var result = _sut.FindControlDefinitionViewModels(null, "PropertyNameValue").ToList();

            // Assert
            CollectionAssert.IsNotEmpty(result);
        }

        [Test]
        public void GoodFindControlDefinitionViewModelsWithMatchingControlDefinitionOnChildFormSection()
        {
            // Arrange
            // Fake the ViewModels so that the child FormSection viewmodel returns a ControlDefinitionViewModel
            //  when FindControlDefinitionViewModels is called
            A.CallTo(() => _formSectionViewModelFactory.CreateInstance()).ReturnsLazily(
                () =>
                {
                    var fakeViewModel = A.Fake<FormSectionViewModel>();
                    A.CallTo(
                        () =>
                            fakeViewModel.FindControlDefinitionViewModels(A<string>.Ignored,
                                A<string>.That.Matches(pn => pn == "PropertyNameValue"))).ReturnsLazily(
                        (string entityName, string propertyName) => new[]
                        {
                            A.Fake<IControlDefinitionViewModel>()
                        });
                    return fakeViewModel;
                });

            _childFormSectionDefinitions.Add(A.Fake<IFormSectionDefinition>());

            // Act
            var result = _sut.FindControlDefinitionViewModels(null, "PropertyNameValue").ToList();

            // Assert
            CollectionAssert.IsNotEmpty(result);
        }

        [Test]
        public void GoodFindControlDefinitionViewModelsWithMatchingControlDefinitionContainer()
        {
            // Arrange
            A.CallTo(() => _fakeControlDefinitionViewModelFactory.CreateViewModel()).ReturnsLazily(
                () =>
                {
                    var fakeViewModel = A.Fake<IControlDefinitionViewModelAndContainer>();
                    A.CallTo(
                        () =>
                            fakeViewModel.FindControlDefinitionViewModels(A<string>.Ignored,
                                A<string>.That.Matches(pn => pn == "PropertyNameValue"))).ReturnsLazily(
                        (string entityName, string propertyName) =>
                            new[]
                            {
                                A.Fake<IControlDefinitionViewModel>()
                            });
                    return fakeViewModel;
                });
            _controlDefinitions.Add(CreateFakeControlDefinitionWithMatchingValuePropertyName("PropertyNameValue"));

            // Act
            var result = _sut.FindControlDefinitionViewModels(null, "PropertyNameValue").ToList();

            // Assert
            CollectionAssert.IsNotEmpty(result);
        }

        private IControlDefinition CreateFakeControlDefinitionWithMatchingValuePropertyName(string propertyName)
        {
            var fakeControlDefinition = A.Fake<IControlDefinition>();
            A.CallTo(() => fakeControlDefinition.ValuePropertyName).Returns(propertyName);
            return fakeControlDefinition;
        }

        public interface IControlDefinitionViewModelAndContainer : IControlDefinitionViewModel,
            IControlDefinitionViewModelContainer
        {
        }
    }
}