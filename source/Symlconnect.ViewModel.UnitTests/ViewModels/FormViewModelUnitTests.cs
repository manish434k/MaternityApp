using System;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Contracts.Factories;
using Symlconnect.ViewModel;
using Symlconnect.ViewModel.ViewModels;

namespace Symlconnect.DataModel.UnitTests.IntegrationTests
{
    [TestFixture]
    public class FormViewModelUnitTests
    {
        private FormViewModel _sut;
        private IControlDefinitionViewModelFactoryLocator _fakeControlDefinitionViewModelFactoryLocator;
        private IControlDefinitionViewModelFactory _fakeControlDefinitionViewModelFactory;
        private IFormSectionDefinition _formSection;
        private FormSectionDefinitionCollection _childFormSectionDefinitions;
        private ControlDefinitionCollection _controlDefinitions;
        private IFormContext _fakeFormContext;
        private IFactory<FormSectionViewModel> _formSectionViewModelFactory;
        private IFormDefinition _formDefinition;
        private FormSectionDefinitionCollection _formSections;

        [SetUp]
        public void TestSetup()
        {
            _fakeControlDefinitionViewModelFactoryLocator = A.Fake<IControlDefinitionViewModelFactoryLocator>();
            _fakeControlDefinitionViewModelFactory = A.Fake<IControlDefinitionViewModelFactory>();
            A.CallTo(() => _fakeControlDefinitionViewModelFactoryLocator.LocateFactory(A<Type>.Ignored))
                .Returns(_fakeControlDefinitionViewModelFactory);
            _formSectionViewModelFactory = A.Fake<IFactory<FormSectionViewModel>>();
            _sut = new FormViewModel(_formSectionViewModelFactory);

            _childFormSectionDefinitions = new FormSectionDefinitionCollection();
            _controlDefinitions = new ControlDefinitionCollection();

            _formDefinition = A.Fake<IFormDefinition>();
            _formSections = new FormSectionDefinitionCollection();
            A.CallTo(() => _formDefinition.SectionDefinitions).Returns(_formSections);

            // Create a fake form section and map the form sections and control sections to fields in this type so our tests can easily 
            // add test sections and controls
            _formSection = A.Fake<IFormSectionDefinition>();
            A.CallTo(() => _formSection.ChildSectionDefinitions).Returns(_childFormSectionDefinitions);
            A.CallTo(() => _formSection.ControlDefinitions).Returns(_controlDefinitions);
            _formSections.Add(_formSection);

            _sut.FormDefinition = _formDefinition;
            _fakeFormContext = A.Fake<IFormContext>();
            _sut.FormContext = _fakeFormContext;
        }

        [Test]
        public void TestFormSectionViewModelConstruction()
        {
            // Arrange
            var sut = new FormViewModel(A.Fake<IFactory<FormSectionViewModel>>());

            var fakeFormDefinition = A.Fake<IFormDefinition>();
            var sectionDefinitions = new FormSectionDefinitionCollection();
            var fakeFormSectionDefinition = A.Fake<FormSectionDefinition>();
            sectionDefinitions.Add(fakeFormSectionDefinition);
            A.CallTo(() => fakeFormDefinition.SectionDefinitions).Returns(sectionDefinitions);

            sut.FormDefinition = fakeFormDefinition;

            // Act
            var viewModels = sut.FormSectionViewModels;

            // Assert
            Assert.AreEqual(1, viewModels.Count);
            Assert.AreSame(fakeFormSectionDefinition, viewModels[0].FormSectionDefinition);
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
            Assert.AreSame(_sut.FormSectionViewModels[0].FormContext, fakeFormContext);
        }

        [Test]
        public void ChangingFormContextPropogatesToChildViewModels()
        {
            // Arrange
            var fakeChildSectionDefinition = A.Fake<IFormSectionDefinition>();
            _childFormSectionDefinitions.Add(fakeChildSectionDefinition);
            var fakeControlDefinition = A.Fake<IControlDefinition>();
            _controlDefinitions.Add(fakeControlDefinition);

            var formSectionViewModels = _sut.FormSectionViewModels;
            var fakeFormContext = A.Fake<IFormContext>();

            // Act
            _sut.FormContext = fakeFormContext;

            // Assert
            CollectionAssert.IsNotEmpty(formSectionViewModels);
            Assert.AreSame(_sut.FormSectionViewModels[0].FormContext, fakeFormContext);
        }

        [Test]
        public void OnEntityPropertyChangedNotifiesAMatchingControlViewModel()
        {
            // Arrange
            var controlDefinitionViewModel = A.Fake<IControlDefinitionViewModel>();

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
                            controlDefinitionViewModel
                        });
                    return fakeViewModel;
                });

            // Add a FormSectionDefinition which will cause our FakeViewModel to be created
            _childFormSectionDefinitions.Add(A.Fake<IFormSectionDefinition>());

            // Now simulate a PropertyChanged event from the entity
            var fakeEntity = A.Fake<IEntity>();
            var args = new EntityPropertyChangedEventArgs(fakeEntity, "PropertyNameValue");

            // Act
            _sut.FormContext.Entity.EntityPropertyChanged += Raise.With(null, args);

            // Assert

            // Make sure the ControlDefinitionViewModel was told about the event
            A.CallTo(
                () =>
                    controlDefinitionViewModel.NotifyPropertyChanged(A<string>.Ignored,
                        A<string>.That.Matches(s => s == "PropertyNameValue"))).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void OnEntityPropertyChangedDoesntFireForaNonMatchingControlViewModel()
        {
            // Arrange
            var controlDefinitionViewModel = A.Fake<IControlDefinitionViewModel>();

            // Fake the ViewModels so that the child FormSection viewmodel returns a ControlDefinitionViewModel
            //  when FindControlDefinitionViewModels is called
            A.CallTo(() => _formSectionViewModelFactory.CreateInstance()).ReturnsLazily(
                () =>
                {
                    var fakeViewModel = A.Fake<FormSectionViewModel>();
                    A.CallTo(
                        () =>
                            fakeViewModel.FindControlDefinitionViewModels(A<string>.Ignored,
                                A<string>.That.Matches(pn => pn == "OtherPropertyNameValue"))).ReturnsLazily(
                        (string entityName, string propertyName) => new[]
                        {
                            controlDefinitionViewModel
                        });
                    return fakeViewModel;
                });

            // Add a FormSectionDefinition which will cause our FakeViewModel to be created
            _childFormSectionDefinitions.Add(A.Fake<IFormSectionDefinition>());

            // Now simulate a PropertyChanged event from the entity
            var fakeEntity = A.Fake<IEntity>();
            var args = new EntityPropertyChangedEventArgs(fakeEntity, "PropertyNameValue");

            // Act
            _sut.FormContext.Entity.EntityPropertyChanged += Raise.With(null, args);

            // Assert

            // Make sure the ControlDefinitionViewModel was not told about the event
            A.CallTo(
                () =>
                    controlDefinitionViewModel.NotifyPropertyChanged(A<string>.Ignored,
                        A<string>.That.Matches(s => s == "PropertyNameValue"))).MustNotHaveHappened();
        }
    }
}