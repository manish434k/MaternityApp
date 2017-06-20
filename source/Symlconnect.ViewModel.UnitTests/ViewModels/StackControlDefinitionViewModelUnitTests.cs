using System;
using System.Linq;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.ViewModel.ViewModels;

namespace Symlconnect.ViewModel.UnitTests.ViewModels
{
    [TestFixture]
    public class StackControlDefinitionViewModelUnitTests
    {
        private IControlDefinitionViewModelFactoryLocator _fakeControlDefinitionViewModelFactoryLocator;
        private IControlDefinitionViewModelFactory _fakeControlDefinitionViewModelFactory;
        private StackControlDefinitionViewModel _sut;
        private ControlDefinitionCollection _controlDefinitions;

        [SetUp]
        public void TestSetup()
        {
            _fakeControlDefinitionViewModelFactoryLocator = A.Fake<IControlDefinitionViewModelFactoryLocator>();
            _fakeControlDefinitionViewModelFactory = A.Fake<IControlDefinitionViewModelFactory>();
            A.CallTo(() => _fakeControlDefinitionViewModelFactoryLocator.LocateFactory(A<Type>.Ignored))
                .Returns(_fakeControlDefinitionViewModelFactory);

            _sut = new StackControlDefinitionViewModel
            {
                ChildFactoryLocator = _fakeControlDefinitionViewModelFactoryLocator
            };
            var fakeStackControlDefinition = A.Fake<StackControlDefinition>();
            _sut.ControlDefinition = fakeStackControlDefinition;

            _controlDefinitions = fakeStackControlDefinition.ChildControlDefinitions;
        }

        [Test]
        public void FormContextIsAppliedToNewChildViewModels()
        {
            // Arrange
            var fakeControlDefinition = A.Fake<IControlDefinition>();
            _controlDefinitions.Add(fakeControlDefinition);

            var fakeFormContext = A.Fake<IFormContext>();

            // Act
            _sut.FormContext = fakeFormContext;

            // Assert
            Assert.AreSame(_sut.ControlDefinitionViewModels[0].FormContext, fakeFormContext);
        }

        [Test]
        public void ChangingFormContextPropogatesToChildViewModels()
        {
            // Arrange
            var fakeControlDefinition = A.Fake<IControlDefinition>();
            _controlDefinitions.Add(fakeControlDefinition);

            var controlViewModels = _sut.ControlDefinitionViewModels;
            var fakeFormContext = A.Fake<IFormContext>();

            // Act
            _sut.FormContext = fakeFormContext;

            // Assert
            CollectionAssert.IsNotEmpty(controlViewModels);
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

            var fakeControlDefinition = A.Fake<IControlDefinition>();
            _controlDefinitions.Add(fakeControlDefinition);

            // Act
            var result =
                ((IControlDefinitionViewModelContainer) _sut).FindControlDefinitionViewModels(null, "PropertyNameValue")
                .ToList();

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

            var fakeControlDefinition = A.Fake<IControlDefinition>();
            _controlDefinitions.Add(fakeControlDefinition);

            // Act
            var result =
                ((IControlDefinitionViewModelContainer) _sut).FindControlDefinitionViewModels(null, "PropertyNameValue")
                .ToList();

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
                    var fakeViewModel = A.Fake<FormSectionViewModelUnitTests.IControlDefinitionViewModelAndContainer>();
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

            var fakeControlDefinition = A.Fake<IControlDefinition>();
            _controlDefinitions.Add(fakeControlDefinition);

            // Act
            var result =
                ((IControlDefinitionViewModelContainer) _sut).FindControlDefinitionViewModels(null, "PropertyNameValue")
                .ToList();

            // Assert
            CollectionAssert.IsNotEmpty(result);
        }
    }
}