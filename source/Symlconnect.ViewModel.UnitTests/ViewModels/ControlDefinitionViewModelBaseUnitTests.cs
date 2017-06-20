using System;
using System.ComponentModel;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.DataModel;
using Symlconnect.ViewModel.ViewModels;

namespace Symlconnect.ViewModel.UnitTests.ViewModels
{
    [TestFixture]
    public class ControlDefinitionViewModelBaseUnitTests
    {
        private ControlDefinitionViewModelBase<IControlDefinition> _sut;

        [SetUp]
        public void TestSetup()
        {
            _sut = new ControlDefinitionViewModelBase<IControlDefinition>();
        }

        [Test]
        public void GetValueWithNoFormContext()
        {
            // Arrange/Act
            var result = _sut.Value;

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void SetValueWithNoFormContext()
        {
            // Arrange/Act/Assert
            Assert.Throws<InvalidOperationException>(() => { _sut.Value = "newvalue"; });
        }

        [Test]
        public void GetVisibleWithNoFormContext()
        {
            // Arrange/Act
            var result = _sut.IsVisible;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void GetValueWithEmptyFormContext()
        {
            // Arrange
            _sut.FormContext = CreateEmptyFakeFormContext();

            // Act
            var result = _sut.Value;

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void SetValueWithEmptyFormContext()
        {
            // Arrange
            _sut.FormContext = CreateEmptyFakeFormContext();

            // Act/Assert
            Assert.Throws<InvalidOperationException>(() => { _sut.Value = "newvalue"; });
        }

        [Test]
        public void GetValueWithPartialFormContext()
        {
            // Arrange
            _sut.FormContext = CreatePartialFormContext();

            // Act
            var result = _sut.Value;

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void SetValueWithPartialFormContext()
        {
            // Arrange
            _sut.FormContext = CreatePartialFormContext();

            // Act/Assert
            Assert.Throws<InvalidOperationException>(() => { _sut.Value = "newvalue"; });
        }

        [Test]
        public void GetVisibleWithEmptyFormContext()
        {
            // Arrange
            _sut.FormContext = CreateEmptyFakeFormContext();

            // Act
            var result = _sut.IsVisible;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void GetVisibleWithPartialFormContext()
        {
            // Arrange
            _sut.FormContext = CreatePartialFormContext();

            // Act
            var result = _sut.IsVisible;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void GetValueWithNullValuePropertyName()
        {
            // Arrange
            _sut.FormContext = CreateGoodFakeFormContext();
            _sut.ControlDefinition = A.Fake<IControlDefinition>();
            A.CallTo(() => _sut.ControlDefinition.ValuePropertyName).Returns(null);

            // Act
            var result = _sut.Value;

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void SetValueWithNullValuePropertyName()
        {
            // Arrange
            _sut.FormContext = CreateGoodFakeFormContext();
            _sut.ControlDefinition = A.Fake<IControlDefinition>();
            A.CallTo(() => _sut.ControlDefinition.ValuePropertyName).Returns(null);

            // Act/Assert
            Assert.Throws<InvalidOperationException>(() => { _sut.Value = "newvalue"; });
        }

        [Test]
        public void GetVisibleWithNullIsVisiblePropertyName()
        {
            // Arrange
            _sut.FormContext = CreateGoodFakeFormContext();
            _sut.ControlDefinition = A.Fake<IControlDefinition>();
            A.CallTo(() => _sut.ControlDefinition.IsVisiblePropertyName).Returns(null);

            // Act
            var result = _sut.IsVisible;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void GoodGetValue()
        {
            // Arrange
            _sut.FormContext = CreateGoodFakeFormContext();
            _sut.ControlDefinition = A.Fake<IControlDefinition>();
            A.CallTo(() => _sut.ControlDefinition.ValuePropertyName).Returns("ValuePropertyNameValue");
            A.CallTo(() => _sut.FormContext.Entity.GetValue(A<string>.Ignored, A<ISessionContext>.Ignored))
                .ReturnsLazily((string propertyName, ISessionContext sessionContext) => propertyName);

            // Act
            var result = _sut.Value;

            // Assert
            Assert.AreEqual("ValuePropertyNameValue", result);
        }

        [Test]
        public void GoodGetIsVisible()
        {
            // Arrange
            _sut.FormContext = CreateGoodFakeFormContext();
            _sut.ControlDefinition = A.Fake<IControlDefinition>();
            A.CallTo(() => _sut.ControlDefinition.ValuePropertyName).Returns("IsVisiblePropertyNameValue");
            A.CallTo(() => _sut.FormContext.Entity.GetValue(A<string>.Ignored, A<ISessionContext>.Ignored))
                .ReturnsLazily((string propertyName, ISessionContext sessionContext) => true);

            // Act
            var result = _sut.IsVisible;

            // Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void GoodSetValue()
        {
            // Arrange
            _sut.FormContext = CreateGoodFakeFormContext();
            _sut.ControlDefinition = A.Fake<IControlDefinition>();
            A.CallTo(() => _sut.ControlDefinition.ValuePropertyName).Returns("OriginalValuePropertyNameValue");
            object valueThatWasSet = null;
            A.CallTo(
                    () =>
                        _sut.FormContext.Entity.SetValue(A<string>.Ignored, A<object>.Ignored,
                            A<ISessionContext>.Ignored))
                .ReturnsLazily((string propertyName, object value, ISessionContext sessionContext) =>
                {
                    valueThatWasSet = value;
                    return new EntitySetValueResult() { IsSuccess = true };
                });

            // Act
            _sut.Value = "NewValuePropertyNameValue";

            // Assert
            Assert.AreEqual("NewValuePropertyNameValue", valueThatWasSet);
        }

        [Test]
        public void ControlDefinitionViewModelGetExplicitProperty()
        {
            // Arrange
            _sut.ControlDefinition = A.Fake<IControlDefinition>();

            // Act
            var explicitControlDefinition = ((IControlDefinitionViewModel)_sut).ControlDefinition;

            // Assert
            Assert.AreSame(_sut.ControlDefinition, explicitControlDefinition);
        }

        [Test]
        public void ControlDefinitionViewModelSetExplicitProperty()
        {
            // Act
            var fakeControlDefinition = A.Fake<IControlDefinition>();
            ((IControlDefinitionViewModel)_sut).ControlDefinition = fakeControlDefinition;

            // Assert
            Assert.AreSame(_sut.ControlDefinition, fakeControlDefinition);
        }

        [Test]
        public void GoodIsValuePropertyNameReferenced()
        {
            // Arrange
            _sut.FormContext = CreateGoodFakeFormContext();
            _sut.ControlDefinition = A.Fake<IControlDefinition>();
            A.CallTo(() => _sut.ControlDefinition.ValuePropertyName).Returns("ValuePropertyNameValue");

            // Act
            var result = _sut.IsPropertyNameReferenced(null, "ValuePropertyNameValue");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void BadIsValuePropertyNameReferenced()
        {
            // Arrange
            _sut.FormContext = CreateGoodFakeFormContext();
            _sut.ControlDefinition = A.Fake<IControlDefinition>();
            A.CallTo(() => _sut.ControlDefinition.ValuePropertyName).Returns("OtherValuePropertyNameValue");

            // Act
            var result = _sut.IsPropertyNameReferenced(null, "ValuePropertyNameValue");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void GoodIsVisiblePropertyNameReferenced()
        {
            // Arrange
            _sut.FormContext = CreateGoodFakeFormContext();
            _sut.ControlDefinition = A.Fake<IControlDefinition>();
            A.CallTo(() => _sut.ControlDefinition.IsVisiblePropertyName).Returns("IsVisiblePropertyNameValue");

            // Act
            var result = _sut.IsPropertyNameReferenced(null, "IsVisiblePropertyNameValue");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void BadIsVisiblePropertyNameReferenced()
        {
            // Arrange
            _sut.FormContext = CreateGoodFakeFormContext();
            _sut.ControlDefinition = A.Fake<IControlDefinition>();
            A.CallTo(() => _sut.ControlDefinition.ValuePropertyName).Returns("OtherIsVisiblePropertyNameValue");

            // Act
            var result = _sut.IsPropertyNameReferenced(null, "IsVisiblePropertyNameValue");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void NotifyPropertyChangedFiresPropertyChangedEventForValuePropertyIfValuePropertyNameChanges()
        {
            _sut.ControlDefinition = A.Fake<IControlDefinition>();
            A.CallTo(() => _sut.ControlDefinition.ValuePropertyName).Returns("ValuePropertyNameValue");

            PropertyChangedEventArgs args = null;
            PropertyChangedEventHandler eventHandler = (sender, ea) =>
            {
                if (ea.PropertyName == "Value")
                {
                    Assert.IsNull(args);
                    args = ea;
                }
            };
            _sut.PropertyChanged += eventHandler;

            _sut.NotifyPropertyChanged(null, "ValuePropertyNameValue");

            _sut.PropertyChanged -= eventHandler;

            Assert.IsNotNull(args);
            Assert.AreEqual("Value", args.PropertyName);
        }

        [Test]
        public void NotifyPropertyChangedDoesNotFirePropertyChangedEventForValuePropertyIfUnrelatedPropertyNameChanges()
        {
            _sut.ControlDefinition = A.Fake<IControlDefinition>();
            A.CallTo(() => _sut.ControlDefinition.ValuePropertyName).Returns("ValuePropertyNameValue");

            PropertyChangedEventArgs args = null;
            PropertyChangedEventHandler eventHandler = (sender, ea) =>
            {
                if (ea.PropertyName == "Value")
                {
                    Assert.IsNull(args);
                    args = ea;
                }
            };
            _sut.PropertyChanged += eventHandler;

            _sut.NotifyPropertyChanged(null, "OtherValuePropertyNameValue");

            _sut.PropertyChanged -= eventHandler;

            Assert.IsNull(args);
        }

        [Test]
        public void NotifyPropertyChangedFiresPropertyChangedEventForIsVisiblePropertyIfValuePropertyNameChanges()
        {
            _sut.ControlDefinition = A.Fake<IControlDefinition>();
            A.CallTo(() => _sut.ControlDefinition.IsVisiblePropertyName).Returns("IsVisiblePropertyNameValue");

            PropertyChangedEventArgs args = null;
            PropertyChangedEventHandler eventHandler = (sender, ea) =>
            {
                if (ea.PropertyName == "IsVisible")
                {
                    Assert.IsNull(args);
                    args = ea;
                }
            };
            _sut.PropertyChanged += eventHandler;

            _sut.NotifyPropertyChanged(null, "IsVisiblePropertyNameValue");

            _sut.PropertyChanged -= eventHandler;

            Assert.IsNotNull(args);
            Assert.AreEqual("IsVisible", args.PropertyName);
        }

        [Test]
        public void
            NotifyPropertyChangedDoesNotFirePropertyChangedEventForIsVisiblePropertyIfUnrelatedPropertyNameChanges()
        {
            _sut.ControlDefinition = A.Fake<IControlDefinition>();
            A.CallTo(() => _sut.ControlDefinition.IsVisiblePropertyName).Returns("IsVisiblePropertyNameValue");

            PropertyChangedEventArgs args = null;
            PropertyChangedEventHandler eventHandler = (sender, ea) =>
            {
                if (ea.PropertyName == "IsVisible")
                {
                    Assert.IsNull(args);
                    args = ea;
                }
            };
            _sut.PropertyChanged += eventHandler;

            _sut.NotifyPropertyChanged(null, "OtherIsVisiblePropertyNameValue");

            _sut.PropertyChanged -= eventHandler;

            Assert.IsNull(args);
        }

        /// <summary>
        ///     Creates a fake form context that returns null values for all properties
        /// </summary>
        private IFormContext CreateEmptyFakeFormContext()
        {
            var fakeFormContext = A.Fake<IFormContext>();
            A.CallTo(() => fakeFormContext.Entity).Returns(null);
            A.CallTo(() => fakeFormContext.SessionContext).Returns(null);

            return fakeFormContext;
        }

        /// <summary>
        ///     Creates a fake form context that returns null values for all properties
        /// </summary>
        private IFormContext CreatePartialFormContext()
        {
            var fakeFormContext = A.Fake<IFormContext>();
            A.CallTo(() => fakeFormContext.Entity).Returns(null);

            return fakeFormContext;
        }

        private IFormContext CreateGoodFakeFormContext()
        {
            var fakeFormContext = A.Fake<IFormContext>();
            return fakeFormContext;
        }
    }
}