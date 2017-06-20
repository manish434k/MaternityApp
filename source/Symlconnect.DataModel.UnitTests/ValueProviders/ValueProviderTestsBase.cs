using System;
using NUnit.Framework;
using FakeItEasy;

namespace Symlconnect.DataModel.UnitTests.IntegrationTests
{
    public abstract class ValueProviderTestsBase
    {
        public ISessionContext FakeSessionContext { get; private set; }
        public IUser FakeUser { get; private set; }
        public IEntity FakeEntity { get; private set; }
        public IEntityDefinition FakeEntityDefinition { get; private set; }
        protected PropertyDefinitionCollection PropertyDefinitions { get; private set; }

        [SetUp]
        public void TestSetup()
        {
            // Fake user
            FakeUser = A.Fake<IUser>();
            A.CallTo(() => FakeUser.UserId).Returns("FakeUserId");

            // Fake session context
            FakeSessionContext = A.Fake<ISessionContext>();
            A.CallTo(() => FakeSessionContext.SessionId).Returns("SessionIdValue");
            A.CallTo(() => FakeSessionContext.SessionDateTime).Returns(new DateTime(2017, 1, 1));
            A.CallTo(() => FakeSessionContext.SessionUser).Returns(FakeUser);

            // Fake entity and related entity definition that returns our PropertyDefinitions collection 
            FakeEntity = A.Fake<IEntity>();
            FakeEntityDefinition = A.Fake<IEntityDefinition>();
            PropertyDefinitions = new PropertyDefinitionCollection();
            A.CallTo(() => FakeEntityDefinition.PropertyDefinitions).Returns(PropertyDefinitions);
            FakeEntity.EntityDefinition = FakeEntityDefinition;
        }

    }
}