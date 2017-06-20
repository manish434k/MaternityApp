using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Symlconnect.Contracts.Diagnostics;
using Symlconnect.Contracts.ObjectModel;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.Common.Serialization
{
    /// <summary>
    ///     Base DocumentDeserializer class.
    /// </summary>
    /// <remarks>
    ///     Takes as a dependency a collection of IElementDeserializer and IElementGroupDeserializer instances that do the
    ///     heavy lifting of deserializing elements in the Xml document. The IChildContainer interface when implemented on
    ///     created instances is used to assemble an object graph by adding instances created from child elements in the
    ///     document to a parent item.
    /// </remarks>
    /// <typeparam name="TRoot">
    ///     The type of instance the DocumentDeserializer will create when
    ///     <see cref="DeserializeElement" /> is called.
    /// </typeparam>
    public class DocumentDeserializer<TRoot>
        : IDocumentDeserializer<TRoot>
        where TRoot : class
    {
        private readonly Dictionary<string, IElementDeserializer<TRoot>> _elementDeserializers;
        private readonly Dictionary<string, IElementGroupDeserializer> _elementGroupDeserializers;
        private readonly ILogger _logger;

        public DocumentDeserializer(IEnumerable<IElementDeserializer<TRoot>> elementDeserializers,
            IEnumerable<IElementGroupDeserializer> elementGroupDeserializers,
            ILogger logger)
        {
            _elementDeserializers = elementDeserializers.ToDictionary(d => d.ElementName);
            _elementGroupDeserializers = elementGroupDeserializers.ToDictionary(d => d.ElementGroupName);
            _logger = logger;
        }

        private object DeserializeElement(XElement element, object parent, TRoot root)
        {
            // Find an element deserializer using the element name
            if (_elementDeserializers.ContainsKey(element.Name.ToString()))
            {
                var elementDeserializer = _elementDeserializers[element.Name.ToString()];
                var deserializationResult = elementDeserializer.DeserializeFromXElement(element, parent, root);

                if (deserializationResult != null)
                {
                    var newInstances = new List<object>();
                    if (deserializationResult is DeserializedItemSet)
                    {
                        // Multiple items
                        newInstances = (DeserializedItemSet) deserializationResult;
                    }
                    else
                    {
                        // Single item
                        newInstances.Add(deserializationResult);
                    }

                    foreach (var newInstance in newInstances)
                    {
                        // If this is the root (i.e. root is currently unset), validate it is the correct type for a root instance
                        if (root == null)
                        {
                            if (!(newInstance.GetType() == typeof(TRoot)
                                  || typeof(TRoot).GetTypeInfo().IsAssignableFrom(newInstance.GetType().GetTypeInfo())))
                            {
                                throw new InvalidOperationException(
                                    $"Root element deserializer returned invalid type. Expected ${typeof(TRoot).Name} but root element deserializer returned ${newInstance.GetType().Name}");
                            }

                            root = (TRoot) newInstance;
                        }

                        var isValidItem = true;
                        // Is parent a IChildItemContainer? If so, check if this is a supported child item and add
                        if (parent is IChildItemContainer)
                        {
                            var parentContainer = (IChildItemContainer) parent;
                            if (parentContainer.IsSupportedChildItem(newInstance))
                            {
                                parentContainer.AddChildItem(newInstance);
                            }
                            else
                            {
                                _logger.Log(LoggingEventType.Warning,
                                    $"Found an unsupported child item. Parent type ${parent.GetType().Name}, child type ${newInstance.GetType().Name}");
                                // Discard the new instance (i.e. don't go on and process children)
                                isValidItem = false;
                            }
                        }

                        if (isValidItem)
                        {
                            // If the newly created instance is a child item container, process child elements
                            if (newInstance is IChildItemContainer)
                            {
                                foreach (var childElement in element.Elements())
                                {
                                    DeserializeElement(childElement, newInstance, root);
                                }
                            }
                            else
                            {
                                if (element.Elements().Any())
                                {
                                    _logger.Log(LoggingEventType.Warning,
                                        "Found child elements below an instance that does not implement IChildItemContainer");
                                }
                            }
                        }
                    }

                    if (newInstances.Count == 1)
                    {
                        return newInstances[0];
                    }
                    return newInstances;
                }
            }
            else if (IsGroupingElement(element))
            {
                // For convenience and readability in the Xml, we support grouping nodes which exist only to make the 
                //  xml more readable. We treat those elements as if they are not there, skipping down to and processing 
                //  the child elements.
                foreach (var childElement in element.Elements())
                {
                    DeserializeElement(childElement, parent, root);
                }
            }
            else
            {
                _logger.Log(LoggingEventType.Warning, $"No child serializer found for {element.Name} element");
            }

            return null;
        }

        protected bool IsGroupingElement(XElement element)
        {
            return _elementGroupDeserializers.ContainsKey(element.Name.ToString());
        }

        /// <summary>
        ///     Deserializes an instance of <typeparamref name="TRoot" /> from the passed <paramref name="document" />.
        /// </summary>
        /// <param name="document">The XDocument instance to deserialize from.</param>
        public TRoot DeserializeFromXDocument(XDocument document)
        {
            if (document?.Root == null)
            {
                throw new ArgumentException("Document root element cannot be null", nameof(document));
            }

            var possibleRootInstance = DeserializeElement(document.Root, null, null);
            if (possibleRootInstance == null)
            {
                throw new InvalidOperationException($"Could not deserialize the root element {document.Root.Name}");
            }

            return (TRoot) possibleRootInstance;
        }
    }
}