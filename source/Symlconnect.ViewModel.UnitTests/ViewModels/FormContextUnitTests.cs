using NUnit.Framework;
using FakeItEasy;
using Symlconnect.DataModel;
using Symlconnect.ViewModel.ViewModels;

namespace Symlconnect.ViewModel.UnitTests.ViewModels
{
    [TestFixture]
    public class FormContextUnitTests
    {
        [Test]
        public void Test()
        {
            // Arrange
            var fakeSessionContext = A.Fake<ISessionContext>();
            var fakeEntity = A.Fake<IEntity>();

            // Act
            var sut = new FormContext()
            {
                Entity = fakeEntity,
                SessionContext = fakeSessionContext
            };

            // Assert
            Assert.AreSame(sut.SessionContext, fakeSessionContext);
            Assert.AreSame(sut.Entity, fakeEntity);
        }
    }
}