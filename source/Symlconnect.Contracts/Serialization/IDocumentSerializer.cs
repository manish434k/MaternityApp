using System.Xml.Linq;

namespace Symlconnect.Contracts.Serialization
{
    /// <summary>
    ///     Implemented by types that can serialize an instance of <typeparamref name="TRoot" /> to an XDocument.
    /// </summary>
    /// <typeparam name="TRoot">The instance returned by this document serializer.</typeparam>
    public interface IDocumentSerializer<in TRoot> where TRoot : class
    {
        /// <summary>
        ///     Serializes an instance of <typeparamref name="TRoot" /> to an XDocument.
        /// </summary>
        /// <param name="root">The root instance to serialize.</param>
        /// <returns>An XDocument that represents the passed root instance.</returns>
        XDocument SerializeToXDocument(TRoot root);
    }
}