using System;

namespace Symlconnect.DataModel
{
    /// <summary>
    ///     Represents the value of a property in an entity for a specific SessionId.
    /// </summary>
    public class EntityPropertyValue
    {
        public string SessionId { get; internal set; }
        public string UserId { get; internal set; }
        public DateTime ChangeDateTime { get; set; }
        public object Value { get; set; }
    }
}