using System;
using NUnit.Framework;
using FakeItEasy;

namespace Symlconnect.DataModel.UnitTests.PropertyDefinitions
{
    /// <summary>
    ///     Base class for property definition tests.
    /// </summary>
    /// <remarks>
    ///     Not hugely keen on class hierarchies in tests but this is simple enough and reduces a great deal of
    ///     duplication.
    /// </remarks>
    [TestFixture]
    public abstract class PropertyDefinitionTestsBase
    {
        public ISessionContext FakeSessionContext { get; private set; }
        public IUser FakeUser { get; private set; }
        public IEntity FakeEntity { get; private set; }
        public IEntityDefinition FakeEntityDefinition { get; private set; }
        public PropertyDefinitionCollection PropertyDefinitions { get; private set; }

        [SetUp]
        public virtual void TestSetup()
        {
            FakeUser = A.Fake<IUser>();
            A.CallTo(() => FakeUser.UserId).Returns("FakeUserId");

            FakeSessionContext = A.Fake<ISessionContext>();

            A.CallTo(() => FakeSessionContext.SessionId).Returns("SessionIdValue");
            A.CallTo(() => FakeSessionContext.SessionDateTime).Returns(new DateTime(2017, 1, 1));
            A.CallTo(() => FakeSessionContext.SessionUser).Returns(FakeUser);

            FakeEntity = A.Fake<IEntity>();
            FakeEntityDefinition = A.Fake<IEntityDefinition>();
            PropertyDefinitions = new PropertyDefinitionCollection();
            A.CallTo(() => FakeEntityDefinition.PropertyDefinitions).Returns(PropertyDefinitions);
            FakeEntity.EntityDefinition = FakeEntityDefinition;
        }
    }
}