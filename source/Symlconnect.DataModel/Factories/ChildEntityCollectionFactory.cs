using Symlconnect.Contracts.Factories;

namespace Symlconnect.DataModel.Factories
{
    public class ChildEntityCollectionFactory : IFactory<ChildEntityCollection>
    {
        public ChildEntityCollection CreateInstance()
        {
            return new ChildEntityCollection();
        }
    }
}