namespace Symlconnect.Contracts.Serialization
{
    /// <summary>
    ///     Serialization of a given Type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The Type this serializer handles.</typeparam>
    public interface IValueSerializer<in T>
    {
        /// <summary>
        ///     Serialize an instance of type <typeparamref name="T" /> to a string value.
        /// </summary>
        /// <param name="value">The instance of type <typeparamref name="T" /> to serialize.</param>
        /// <returns>A string representing the serialized value.</returns>
        string SerializeValue(T value);
    }
}