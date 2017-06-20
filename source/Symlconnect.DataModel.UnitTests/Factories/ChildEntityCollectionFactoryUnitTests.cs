using NUnit.Framework;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel.Factories;

namespace Symlconnect.DataModel.UnitTests.IntegrationTests
{
    [TestFixture]
    public class ChildEntityCollectionFactoryUnitTests : FactoryTestBase<ChildEntityCollection>
    {
        protected override IFactory<ChildEntityCollection> CreateFactoryInstance()
        {
            return new ChildEntityCollectionFactory();
        }
    }
}