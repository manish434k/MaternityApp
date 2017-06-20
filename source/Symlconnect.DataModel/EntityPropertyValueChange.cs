namespace Symlconnect.DataModel
{
    /// <summary>
    ///     Represents a single property change to be committed.
    /// </summary>
    public class EntityPropertyValueChange
    {
        public string EntityName { get; set; }
        public string PropertyName { get; set; }
        public object NewValue { get; set; }
        public bool WasStoreUpdated { get; set; }
    }
}