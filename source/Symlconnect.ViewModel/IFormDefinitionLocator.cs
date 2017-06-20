namespace Symlconnect.ViewModel
{
    public interface IFormDefinitionLocator
    {
        IFormDefinition GetFormDefinition(string name);
    }
}