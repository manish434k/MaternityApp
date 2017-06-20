namespace Symlconnect.DataModel
{
    /// <summary>
    ///     Describes a single Property within a EntityDefinition.
    /// </summary>
    public interface IPropertyDefinition
    {
        /// <summary>
        ///     An Name that uniquely identifies this property within a EntityDefinition.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     The property kind (i.e. data type).
        /// </summary>
        ValueKind PropertyDefinitionKind { get; set; }

        /// <summary>
        ///     Process a passed changset, potentially modifying the changes or adding additional.
        /// </summary>
        /// <param name="entity">The Entity this changeset is associated with.</param>
        /// <param name="changeset">The set of changes that will be commited.</param>
        void TargetToSource(IEntity entity, EntityPropertyValueChangeset changeset);

        /// <summary>
        ///     Process a passed source value, returning the finalized target value for editing.
        /// </summary>
        /// <param name="entity">The Entity this value is associated with.</param>
        /// <param name="value">The target value.</param>
        /// <param name="sessionContext">Context for the session.</param>
        /// <returns>The target value, ready for editing</returns>
        object SourceToTarget(IEntity entity, object value, ISessionContext sessionContext);
    }
}