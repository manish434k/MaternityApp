using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Contracts.Environment;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel.Factories;

namespace Symlconnect.DataModel.UnitTests.IntegrationTests
{
    [TestFixture]
    public class AuditPropertyDefinitionFactoryUnitTests : FactoryTestBase<AuditPropertyDefinition>
    {
        protected override IFactory<AuditPropertyDefinition> CreateFactoryInstance()
        {
            return new AuditPropertyDefinitionFactory(A.Fake<ICurrentDateTimeProvider>());
        }
    }
}