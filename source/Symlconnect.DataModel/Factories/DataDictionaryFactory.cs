using Symlconnect.Contracts.Factories;

namespace Symlconnect.DataModel.Factories
{
    public class DataDictionaryFactory: IFactory<DataDictionary>
    {
        public DataDictionary CreateInstance()
        {
            return new DataDictionary();
        }
    }
}