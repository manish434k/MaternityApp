using Symlconnect.Contracts.Factories;

namespace Symlconnect.Common.Factories
{
    public class SimpleFactory<T> : IFactory<T>
        where T : class, new()
    {
        public T CreateInstance()
        {
            return new T();
        }
    }
}