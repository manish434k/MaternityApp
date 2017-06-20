using System.Collections.Generic;

namespace Symlconnect.Common.Serialization
{
    /// <summary>
    ///     This type can be returned by element deserializers if they create multiple items from a single element.
    /// </summary>
    public class DeserializedItemSet : List<object>
    {
    }
}