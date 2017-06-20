namespace Symlconnect.DataModel
{
    public interface IPropertyReferenceContainer
    {
        bool IsPropertyReferenced(string entityName, string propertyName);
    }
}