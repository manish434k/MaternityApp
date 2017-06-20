using NUnit.Framework;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel.Factories;

namespace Symlconnect.DataModel.UnitTests.IntegrationTests
{
    [TestFixture]
    public class ChildEntityCollectionPropertyDefinitionFactoryUnitTests :
        FactoryTestBase<ChildEntityCollectionPropertyDefinition>
    {
        protected override IFactory<ChildEntityCollectionPropertyDefinition> CreateFactoryInstance()
        {
            return new ChildEntityCollectionPropertyDefinitionFactory();
        }
    }
}