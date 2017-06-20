using System;
using System.Collections.Generic;

namespace Symlconnect.DataModel
{
    /// <summary>
    ///     Represents a single instance of a particular kind of Entity, identified by the EntityName and EntityDefinition.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        ///     The EntityDefinition instance for the Entity.
        /// </summary>
        IEntityDefinition EntityDefinition { get; set; }

        string Id { get; set; }
        string CreatedByUserId { get; set; }
        string CreatedByUserDisplayName { get; set; }
        DateTime CreatedDateTime { get; set; }

        event EventHandler<EntityPropertyChangedEventArgs> EntityPropertyChanged;

        /// <summary>
        ///     Updates the value stored in this Entity for a specific named property.
        /// </summary>
        /// <param name="propertyName">The property to store a new value for.</param>
        /// <param name="value">The new value to store.</param>
        /// <param name="sessionContext">The session context to store the value under.</param>
        /// <returns>true if storing the property succeeded, false if not.</returns>
        EntitySetValueResult SetValue(string propertyName, object value, ISessionContext sessionContext);

        /// <summary>
        ///     Gets the value stored in this entity for a specific named property.
        /// </summary>
        /// <param name="propertyName">The property to get the value of.</param>
        /// <param name="sessionContext">The session context to get the value for.</param>
        /// <returns>The value of the named property, as currently stored in this Entity for the passed session context.</returns>
        object GetValue(string propertyName, ISessionContext sessionContext);

        /// <summary>
        ///     Associate an Entity as a child of this Entity.
        /// </summary>
        /// <param name="entityPropertyName">The name of the Child Entity Property.</param>
        /// <param name="childEntity">The Entity instance being added.</param>
        /// <param name="sessionContext">The session context associated with adding the new child Entity.</param>
        /// <returns></returns>
        bool AddChildEntity(string entityPropertyName, IEntity childEntity, ISessionContext sessionContext);

        /// <summary>
        ///     Returns a collection containing all Child entities of a given type (childEntityName) within a given session
        ///     context.
        /// </summary>
        /// <param name="entityPropertyName">The name of the Child Entity Property.</param>
        /// <param name="sessionContext">The session context for which Child Entities should be returned.</param>
        /// <returns></returns>
        IEnumerable<IEntity> GetChildEntities(string entityPropertyName, ISessionContext sessionContext);

        /// <summary>
        ///     Resolves an Entity logically related to this one.
        /// </summary>
        /// <param name="entityName">The EntityName of the related Entity to return.</param>
        /// <remarks>If the Entity is logically a child, the most recent child entity is returned.</remarks>
        IEntity ResolveRelatedEntity(string entityName);
    }
}