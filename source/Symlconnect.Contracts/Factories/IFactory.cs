namespace Symlconnect.Contracts.Factories
{
    public interface IFactory<out T>
    {
        T CreateInstance();
    }
}