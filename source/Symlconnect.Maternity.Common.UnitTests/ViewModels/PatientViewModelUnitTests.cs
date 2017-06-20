using System;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Contracts.Environment;
using Symlconnect.Maternity.Common.ViewModels;

namespace Symlconnect.Maternity.Common.UnitTests.ViewModels
{
    [TestFixture]
    public class PatientViewModelUnitTests
    {
        private PatientViewModel _sut;
        private DateTime _currentDateTime;

        [SetUp]
        public void TestSetup()
        {
            var fakeCurrentDateTimeProvider = A.Fake<ICurrentDateTimeProvider>();
            A.CallTo(() => fakeCurrentDateTimeProvider.GetCurrentDateTime())
                .ReturnsLazily(() => _currentDateTime);
            _sut = new PatientViewModel(fakeCurrentDateTimeProvider);
        }

        [Test]
        public void AgeWithValidDateOfBirthBeforeCurrentDate()
        {
            // Arrange
            _currentDateTime = new DateTime(2017, 1, 2);
            var fakePatient = A.Fake<IPatient>();
            A.CallTo(() => fakePatient.DateOfBirth)
                .Returns(new DateTime(2000, 1, 1));
            _sut.Patient = fakePatient;

            // Act
            var result = _sut.Age;

            // Assert
            Assert.AreEqual("17", result);
        }

        [Test]
        public void AgeWithValidDateOfBirthAfterCurrentDate()
        {
            // Arrange
            _currentDateTime = new DateTime(2017, 1, 2);
            var fakePatient = A.Fake<IPatient>();
            A.CallTo(() => fakePatient.DateOfBirth)
                .Returns(new DateTime(2000, 6, 1));
            _sut.Patient = fakePatient;

            // Act
            var result = _sut.Age;

            // Assert
            Assert.AreEqual("16", result);
        }

        [Test]
        public void AgeWithMissingDateOfBirth()
        {
            // Arrange
            _currentDateTime = new DateTime(2017, 1, 2);
            var fakePatient = A.Fake<IPatient>();
            A.CallTo(() => fakePatient.DateOfBirth)
                .Returns(null);
            _sut.Patient = fakePatient;

            // Act
            var result = _sut.Age;

            // Assert
            // (uknown) is the text that should be shown for Age if the Date of Birth isn't defined.
            Assert.AreEqual("(uknown)", result);
        }

        [Test]
        public void CaptionWithNullPatient()
        {
            // Arrange/Act
            var result = _sut.Caption;

            // Assert
            Assert.AreEqual(null, result);
        }

        [Test]
        public void CaptionWithPatientWithPopulatedProperties()
        {
            // Arrange
            _currentDateTime = new DateTime(2017, 1, 2);
            var fakePatient = A.Fake<IPatient>();
            A.CallTo(() => fakePatient.DateOfBirth)
                .Returns(new DateTime(2000, 6, 1));
            A.CallTo(() => fakePatient.Name)
                .Returns("PatientName");
            A.CallTo(() => fakePatient.PatientNumber)
                .Returns("PatientNumber");
            _sut.Patient = fakePatient;

            // Act
            var result = _sut.Caption;

            // Assert
            Assert.AreEqual("PatientName, No. PatientNumber, 16 years", result);
        }
    }
}