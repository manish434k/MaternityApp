using System;
using System.Diagnostics.CodeAnalysis;

namespace Symlconnect.DataModel
{
    /// <summary>
    ///     Raised when a Property value changes on an Entity.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class EntityPropertyChangedEventArgs : EventArgs
    {
        public EntityPropertyChangedEventArgs(IEntity entity, string propertyName)
        {
            Entity = entity;
            PropertyName = propertyName;
        }

        public IEntity Entity { get; }
        public string PropertyName { get; }
    }
}