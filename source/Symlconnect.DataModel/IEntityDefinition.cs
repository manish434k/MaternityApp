namespace Symlconnect.DataModel
{
    /// <summary>
    ///     Describes the properties of a given Entity type, defined by the <see cref="EntityName" /> property.
    /// </summary>
    public interface IEntityDefinition
    {
        /// <summary>
        ///     The Type of Entity this EntityDefinition describes.
        /// </summary>
        string EntityName { get; }

        /// <summary>
        ///     The collection of Properties defined in this EntityDefinition.
        /// </summary>
        PropertyDefinitionCollection PropertyDefinitions { get; }

        /// <summary>
        ///     The Data Dictionary to which this EntityDefinition belongs.
        /// </summary>
        IDataDictionary DataDictionary { get; }
    }
}