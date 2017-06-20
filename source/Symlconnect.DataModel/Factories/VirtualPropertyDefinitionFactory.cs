using Symlconnect.Contracts.Factories;

namespace Symlconnect.DataModel.Factories
{
    public class VirtualPropertyDefinitionFactory : IFactory<VirtualPropertyDefinition>
    {
        public VirtualPropertyDefinition CreateInstance()
        {
            return new VirtualPropertyDefinition();
        }
    }
}