using Symlconnect.Contracts.Factories;

namespace Symlconnect.DataModel.Factories
{
    public class ChildEntityFactory : IFactory<ChildEntity>
    {
        public ChildEntity CreateInstance()
        {
            return new ChildEntity();
        }
    }
}