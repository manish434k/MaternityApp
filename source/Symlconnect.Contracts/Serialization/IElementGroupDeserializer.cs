namespace Symlconnect.Contracts.Serialization
{
    /// <summary>
    /// Identifies that this type supports an Element Group name. Element Groups exist in the Xml only to support
    /// grouping of child Elements for readability and do not form a logical part of the eventual object graph
    /// </summary>
    public interface IElementGroupDeserializer
    {
        /// <summary>
        /// Identifies the name of an Element group that is logically an Element Group when deserializing.
        /// </summary>
        string ElementGroupName { get; }
    }
}