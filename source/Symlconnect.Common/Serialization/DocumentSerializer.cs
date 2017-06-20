using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Symlconnect.Contracts.Diagnostics;
using Symlconnect.Contracts.ObjectModel;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.Common.Serialization
{
    /// <summary>
    ///     Base document serializer class.
    /// </summary>
    /// <remarks>
    ///     Uses a set of IElementSerializer types to serialize an object graph, starting at the root object and
    ///     traversing through children using the IChildItemContainer interface.
    /// </remarks>
    /// <typeparam name="TRoot">The Type of the root object in the object graph the DocumentSerializer serializes.</typeparam>
    public class DocumentSerializer<TRoot> : IDocumentSerializer<TRoot> where TRoot : class
    {
        private readonly List<IElementSerializer<TRoot>> _elementSerializers;
        private readonly Dictionary<Type, IElementSerializer<TRoot>> _elementSerializersByType;
        private readonly ILogger _logger;

        public DocumentSerializer(IEnumerable<IElementSerializer<TRoot>> elementSerializers,
            ILogger logger)
        {
            _elementSerializers = elementSerializers.ToList();
            _elementSerializersByType = new Dictionary<Type, IElementSerializer<TRoot>>();
            _logger = logger;
        }

        public XDocument SerializeToXDocument(TRoot root)
        {
            if (root == null)
            {
                return null;
            }

            var possibleRootElement = SerializeItemToElement(root, null, null);
            if (possibleRootElement == null)
            {
                var errorMessage = $"Could not serialize the root item {root.GetType().Name}";
                _logger.Log(LoggingEventType.Error, errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            return new XDocument(possibleRootElement);
        }

        /// <summary>
        ///     Serializes a single item to an element by finding and invoking the appropriate IElementSerializer.
        /// </summary>
        /// <param name="item">The item to serialize.</param>
        /// <param name="parent">The logical parent of the item in the object graph.</param>
        /// <param name="root">The root of the object graph.</param>
        /// <returns></returns>
        private object SerializeItemToElement(object item, object parent, TRoot root)
        {
            IElementSerializer<TRoot> elementSerializer;
            if (_elementSerializersByType.ContainsKey(item.GetType()))
            {
                // We have already located the IElementSerializer for this type, grab from the cache
                elementSerializer = _elementSerializersByType[item.GetType()];
            }
            else
            {
                // Look for an IElementSerializer for this type and if found add it to the cache for next time
                elementSerializer =
                    _elementSerializers.FirstOrDefault(s => s.IsSerializerForType(item.GetType()));
                if (elementSerializer != null)
                {
                    _elementSerializersByType.Add(item.GetType(), elementSerializer);
                }
            }

            if (elementSerializer != null)
            {
                var element = elementSerializer.SerializeToXElement(item, parent, root);
                if (element != null)
                {
                    if (item is IChildItemContainer)
                    {
                        var parentContainer = (IChildItemContainer) item;
                        if (root == null && item is TRoot)
                        {
                            root = (TRoot) item;
                        }
                        foreach (object childItem in parentContainer.GetChildItems())
                        {
                            var childElement = SerializeItemToElement(childItem, item, root);
                            if (childElement != null)
                            {
                                element.Add(childElement);
                            }
                        }
                    }
                }

                // No error for a null element as the serializer might decide to only serialize some items

                return element;
            }

            // No serializer found. We don't throw an error as this might be expected (some objects in an object graph might 
            //  not need to be serialized).
            _logger.Log(LoggingEventType.Information, $"No serializer found for the type {item.GetType().Name}");
            return null;
        }
    }
}