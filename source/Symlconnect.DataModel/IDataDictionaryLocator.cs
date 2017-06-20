namespace Symlconnect.DataModel
{
    public interface IDataDictionaryLocator
    {
        IDataDictionary GetDataDictionary(string name);
    }
}