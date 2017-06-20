using Symlconnect.Contracts.Factories;

namespace Symlconnect.DataModel.Factories
{
    public class ChildEntityCollectionPropertyDefinitionFactory : IFactory<ChildEntityCollectionPropertyDefinition>
    {
        public ChildEntityCollectionPropertyDefinition CreateInstance()
        {
            return new ChildEntityCollectionPropertyDefinition();
        }
    }
}