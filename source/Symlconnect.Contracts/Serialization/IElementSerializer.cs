using System;
using System.Xml.Linq;

namespace Symlconnect.Contracts.Serialization
{
    /// <summary>
    ///     Serializes an individual object instance to an XElement.
    /// </summary>
    /// <typeparam name="TRoot">The root type of the object graph this element serializer operates on.</typeparam>
    public interface IElementSerializer<in TRoot> where TRoot : class
    {
        /// <summary>
        ///     Returns true if the passed type is handled by this serializer.
        /// </summary>
        bool IsSerializerForType(Type possibleType);

        /// <summary>
        ///     Serializes the passed item to an XElement.
        /// </summary>
        /// <param name="item">The item to serialize.</param>
        /// <param name="parent">The logical parent of the item being serialized (or null).</param>
        /// <param name="root">The logical root of the object graph being serialized (or null).</param>
        /// <returns>An XElement representing the item and any child serialized items.</returns>
        XElement SerializeToXElement(object item, object parent, TRoot root);
    }
}