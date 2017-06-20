using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel.ValueProviders;

namespace Symlconnect.DataModel.Factories
{
    public class PropertyValueProviderFactory : IFactory<PropertyValueProvider>
    {
        public PropertyValueProvider CreateInstance()
        {
            return new PropertyValueProvider();
        }
    }
}