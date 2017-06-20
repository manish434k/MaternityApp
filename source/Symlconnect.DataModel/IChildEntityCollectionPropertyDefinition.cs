namespace Symlconnect.DataModel
{
    public interface IChildEntityCollectionPropertyDefinition : IPropertyDefinition
    {
        IEntityDefinition EntityDefinition { get; set; }
    }
}