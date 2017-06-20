using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FakeItEasy;
using NUnit.Framework;
using Symlconnect.Common.Serialization;
using Symlconnect.Common.UnitTests.Serialization.TestTypes;
using Symlconnect.Contracts.Diagnostics;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.Common.UnitTests
{
    /// <summary>
    ///     Tests for deserializing from a document with child elements.
    /// </summary>
    public class ComplexDocumentDeserialization
    {
        private List<object> _addedChildItems;
        private IElementDeserializer<ComplexRootType> _fakeChildDeserializer;
        private ComplexRootType _fakeComplexRoot;
        private ILogger _fakeLogger;
        private IElementDeserializer<ComplexRootType> _fakeRootDeserializer;

        [SetUp]
        public void TestSetup()
        {
            _addedChildItems = new List<object>();
            _fakeLogger = A.Fake<ILogger>();

            // Fake a complex root that supports adding SimpleChildType instances
            _fakeComplexRoot = A.Fake<ComplexRootType>();
            A.CallTo(() => _fakeComplexRoot.IsSupportedChildItem(A<object>.That.IsInstanceOf(typeof(SimpleChildType))))
                .Returns(true);
            A.CallTo(() => _fakeComplexRoot.AddChildItem(A<object>.That.IsInstanceOf(typeof(SimpleChildType))))
                .Invokes((object newChildItem) => { _addedChildItems.Add(newChildItem); });

            // Fake "root" deserializer
            _fakeRootDeserializer = A.Fake<IElementDeserializer<ComplexRootType>>();
            A.CallTo(() => _fakeRootDeserializer.ElementName).Returns("root");
            A.CallTo(
                    () =>
                        _fakeRootDeserializer.DeserializeFromXElement(
                            A<XElement>.That.Matches(e => e.Name.ToString() == "root"), A<object>.That.IsNull(),
                            A<ComplexRootType>.That.IsNull()))
                .Returns(_fakeComplexRoot);

            // Fake "child" deserializer
            _fakeChildDeserializer = A.Fake<IElementDeserializer<ComplexRootType>>();
            A.CallTo(() => _fakeChildDeserializer.ElementName).Returns("child");
            A.CallTo(
                    () =>
                        _fakeChildDeserializer.DeserializeFromXElement(
                            A<XElement>.That.Matches(e => e.Name.ToString() == "child"),
                            A<object>.That.IsInstanceOf(typeof(ComplexRootType)),
                            A<ComplexRootType>.That.IsInstanceOf(typeof(ComplexRootType))))
                .Returns(A.Fake<SimpleChildType>());
        }

        /// <summary>
        ///     Deserialise a simple document with a root and a single child element.
        /// </summary>
        [Test]
        public void DeserializeSimpleObjectGraph()
        {
            // Arrange
            var sut = new DocumentDeserializer<ComplexRootType>(new[] {_fakeRootDeserializer, _fakeChildDeserializer},
                Enumerable.Empty<IElementGroupDeserializer>(), _fakeLogger);
            var document = XDocument.Parse("<root><child/></root>");

            // Act
            var rootInstance = sut.DeserializeFromXDocument(document);

            // Assert
            A.CallTo(
                    () =>
                        _fakeRootDeserializer.DeserializeFromXElement(A<XElement>.Ignored, A<object>.Ignored,
                            A<ComplexRootType>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);

            Assert.IsNotNull(rootInstance);

            Assert.AreEqual(1, _addedChildItems.Count);
        }

        /// <summary>
        ///     Deserialise a simple document with an invalid child element; i.e. one where the deserializer for the root element
        ///     is not the expected
        ///     root element type
        /// </summary>
        [Test]
        public void InappropriateRootElement()
        {
            // Arrange
            var sut = new DocumentDeserializer<ComplexRootType>(new[] {_fakeRootDeserializer, _fakeChildDeserializer},
                Enumerable.Empty<IElementGroupDeserializer>(), _fakeLogger);
            var document = XDocument.Parse("<child/>");

            // Act / assert
            Assert.Throws<InvalidOperationException>(() => { sut.DeserializeFromXDocument(document); });
        }

        /// <summary>
        ///     Deserialize a document with nested elements where the nested elements are not supported by the parent
        /// </summary>
        [Test]
        public void InappropriateChildItems()
        {
            // Arrange
            var sut = new DocumentDeserializer<ComplexRootType>(new[] {_fakeRootDeserializer, _fakeChildDeserializer},
                Enumerable.Empty<IElementGroupDeserializer>(), _fakeLogger);
            var document = XDocument.Parse("<root><root/></root>");

            // Act 
            sut.DeserializeFromXDocument(document);

            // Assert
            A.CallTo(() => _fakeLogger.Log(A<LogEntry>.That.Matches(e => e.Severity == LoggingEventType.Warning)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }


        [Test]
        public void DeserializerReturnsMultipleItems()
        {
            // Arrange

            // Fake "multi" deserializer
            var fakeMultiItemDeserializer = A.Fake<IElementDeserializer<ComplexRootType>>();
            A.CallTo(() => fakeMultiItemDeserializer.ElementName).Returns("multi");
            A.CallTo(
                    () =>
                        fakeMultiItemDeserializer.DeserializeFromXElement(
                            A<XElement>.That.Matches(e => e.Name.ToString() == "multi"),
                            A<object>.That.IsInstanceOf(typeof(ComplexRootType)),
                            A<ComplexRootType>.That.IsInstanceOf(typeof(ComplexRootType))))
                .Returns(new DeserializedItemSet {A.Fake<SimpleChildType>(), A.Fake<SimpleChildType>()});

            var sut = new DocumentDeserializer<ComplexRootType>(
                new[] {_fakeRootDeserializer, fakeMultiItemDeserializer},
                Enumerable.Empty<IElementGroupDeserializer>(), _fakeLogger);
            var document = XDocument.Parse("<root><multi/></root>");

            // Act 
            sut.DeserializeFromXDocument(document);

            // Assert
            Assert.AreEqual(2, _addedChildItems.Count);
        }
    }
}