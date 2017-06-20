using NUnit.Framework;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel.Factories;

namespace Symlconnect.DataModel.UnitTests.IntegrationTests
{
    [TestFixture]
    public class ChildEntityFactoryUnitTests : FactoryTestBase<ChildEntity>
    {
        protected override IFactory<ChildEntity> CreateFactoryInstance()
        {
            return new ChildEntityFactory();
        }
    }
}