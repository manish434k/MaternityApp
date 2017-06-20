namespace Symlconnect.Contracts.Serialization
{
    /// <summary>
    ///     Deserialization for a given Type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The Type this deserializer handles.</typeparam>
    public interface IValueDeserializer<out T>
    {
        /// <summary>
        ///     Deserialize an instance of type T from a string value.
        /// </summary>
        /// <param name="text">The text to deserialize from.</param>
        /// <returns>A new instance of type T.</returns>
        T DeserializeValue(string text);
    }
}