using System;
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
    ///     Tests for deserializing from a simple document with just a root element.
    /// </summary>
    [TestFixture]
    public class SimpleDocumentDeserialization
    {
        [SetUp]
        public void TestSetup()
        {
            _fakeLogger = A.Fake<ILogger>();

            _fakeRootDeserializer = A.Fake<IElementDeserializer<SimpleRootType>>();
            A.CallTo(() => _fakeRootDeserializer.ElementName).Returns("root");

            // Any call with a "root" element will return a SimpleRootType
            A.CallTo(
                    () =>
                        _fakeRootDeserializer.DeserializeFromXElement(
                            A<XElement>.That.Matches(e => e.Name.ToString() == "root"), A<object>.Ignored,
                            A<SimpleRootType>.Ignored))
                .Returns(new SimpleRootType());
        }

        private IElementDeserializer<SimpleRootType> _fakeRootDeserializer;
        private ILogger _fakeLogger;

        /// <summary>
        ///     Deserialize a document with nested elements where the root element is not expected to be a IChildContainer
        /// </summary>
        [Test]
        public void ChildElementsBelowItemThatIsNotContainer()
        {
            // Arrange
            var sut = new DocumentDeserializer<SimpleRootType>(new[] {_fakeRootDeserializer},
                Enumerable.Empty<IElementGroupDeserializer>(), _fakeLogger);
            var document = XDocument.Parse("<root><child/></root>");

            // Act 
            sut.DeserializeFromXDocument(document);

            // Assert
            A.CallTo(() => _fakeLogger.Log(A<LogEntry>.That.Matches(e => e.Severity == LoggingEventType.Warning)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        /// <summary>
        ///     Deserialize a simple document with just a root element.
        /// </summary>
        [Test]
        public void DeserializeSimpleDocument()
        {
            // Arrange
            var sut = new DocumentDeserializer<SimpleRootType>(new[] {_fakeRootDeserializer},
                Enumerable.Empty<IElementGroupDeserializer>(), _fakeLogger);
            var document = XDocument.Parse("<root/>");

            // Act
            var rootInstance = sut.DeserializeFromXDocument(document);

            // Assert
            A.CallTo(
                    () =>
                        _fakeRootDeserializer.DeserializeFromXElement(A<XElement>.Ignored, A<object>.Ignored,
                            A<SimpleRootType>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);

            Assert.IsNotNull(rootInstance);
        }

        /// <summary>
        ///     Deserialize a simple document with just a root element.
        /// </summary>
        [Test]
        public void DeserializeSimpleDocumentWithDeserializerThatReturnsNull()
        {
            // Arrange
            A.CallTo(
                    () =>
                        _fakeRootDeserializer.DeserializeFromXElement(
                            A<XElement>.That.Matches(e => e.Name.ToString() == "root"), A<object>.Ignored,
                            A<SimpleRootType>.Ignored))
                .Returns(null);

            var sut = new DocumentDeserializer<SimpleRootType>(new[] {_fakeRootDeserializer},
                Enumerable.Empty<IElementGroupDeserializer>(), _fakeLogger);
            var document = XDocument.Parse("<root/>");

            // Act
            Assert.Throws<InvalidOperationException>(() =>
            {
                sut.DeserializeFromXDocument(document);
            });
        }

        /// <summary>
        ///     Deserialize an XDocument instance without a Root element.
        /// </summary>
        [Test]
        public void EmptyDocumentRootElement()
        {
            // Arrange
            var sut = new DocumentDeserializer<SimpleRootType>(new[] {_fakeRootDeserializer},
                Enumerable.Empty<IElementGroupDeserializer>(), _fakeLogger);
            var document = new XDocument();

            // Act / Assert
            Assert.Throws<ArgumentException>(() => { sut.DeserializeFromXDocument(document); });
        }

        /// <summary>
        ///     Deserialize a null XDocument instance.
        /// </summary>
        [Test]
        public void NullDocument()
        {
            // Arrange
            var sut = new DocumentDeserializer<SimpleRootType>(new[] {_fakeRootDeserializer},
                Enumerable.Empty<IElementGroupDeserializer>(), _fakeLogger);

            // Act / Assert
            Assert.Throws<ArgumentException>(() => { sut.DeserializeFromXDocument(null); });
        }

        /// <summary>
        ///     Deserialize a document with an unrecognized root element name (one for which we have no registered
        ///     deserializer).
        /// </summary>
        [Test]
        public void UnrecognizedRootElement()
        {
            // Arrange
            var sut = new DocumentDeserializer<SimpleRootType>(new[] {_fakeRootDeserializer},
                Enumerable.Empty<IElementGroupDeserializer>(), _fakeLogger);
            var document = XDocument.Parse("<unrecognizedelementname/>");

            // Act / assert
            Assert.Throws<InvalidOperationException>(() => { sut.DeserializeFromXDocument(document); });
        }
    }
}