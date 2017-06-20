using System.Collections.Generic;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Contracts.Environment;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel;
using Symlconnect.Maternity.Common.ViewModels;

namespace Symlconnect.Maternity.Common.UnitTests.ViewModels
{
    [TestFixture]
    public class PatientHomeViewModelUnitTests
    {
        private IPatientLoader _fakePatientLoader;
        private IFactory<MaternityRecordViewModel> _fakeMaternityRecordViewModelFactory;
        private PatientHomeViewModel _sut;

        [SetUp]
        public void TestSetup()
        {
            _fakePatientLoader = A.Fake<IPatientLoader>();
            _fakeMaternityRecordViewModelFactory = A.Fake<IFactory<MaternityRecordViewModel>>();
            _sut = new PatientHomeViewModel(_fakePatientLoader, A.Fake<ICurrentDateTimeProvider>(),
                _fakeMaternityRecordViewModelFactory);
        }

        [Test]
        public void MaternityRecordsWithNoPatient()
        {
            // Arrange/Act
            var maternityRecords = _sut.MaternityRecords;

            // Assert
            Assert.IsNull(maternityRecords);
        }

        [Test]
        public void MaternityRecordViewModelsWithNoPatient()
        {
            // Arrange/Act
            var maternityRecordViewModels = _sut.MaternityRecordViewModels;

            // Assert
            Assert.IsNull(maternityRecordViewModels);
        }

        [Test]
        public void MaternityRecordViewModelCreation()
        {
            // Arrange
            _sut.Patient = A.Fake<IPatient>();
            var maternityRecords = new List<IEntity>();
            var fakeMaternityRecord = A.Fake<IEntity>();
            maternityRecords.Add(fakeMaternityRecord);
            A.CallTo(
                    () =>
                        _fakePatientLoader.LoadPatientEntities(A<IPatient>.Ignored,
                            A<string>.That.Matches(s => s == "record")))
                .Returns(maternityRecords);

            // Act
            var viewModels = _sut.MaternityRecordViewModels;

            // Assert
            Assert.AreEqual(1, viewModels.Count);
            Assert.AreSame(viewModels[0].Entity, fakeMaternityRecord);
        }
    }
}