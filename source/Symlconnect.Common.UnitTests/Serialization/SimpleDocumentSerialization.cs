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
    ///     Test serialization of a single object.
    /// </summary>
    [TestFixture]
    public class SimpleDocumentSerialization
    {
        private IElementSerializer<SimpleRootType> _fakeRootSerializer;
        private ILogger _fakeLogger;

        [SetUp]
        public void TestSetup()
        {
            _fakeLogger = A.Fake<ILogger>();

            _fakeRootSerializer = A.Fake<IElementSerializer<SimpleRootType>>();
            A.CallTo(() => _fakeRootSerializer.IsSerializerForType(A<Type>.That.Matches(t => t == typeof(SimpleRootType)
                                                                                             ||
                                                                                             t.IsSubclassOf(
                                                                                                 typeof(SimpleRootType)))))
                .Returns(true);

            // Any call with a SimpleRootType will return a simpleroottype element
            A.CallTo(
                    () =>
                        _fakeRootSerializer.SerializeToXElement(
                            A<object>.Ignored,
                            A<object>.Ignored,
                            A<SimpleRootType>.Ignored))
                .Returns(new XElement("simpleroottype"));
        }

        /// <summary>
        ///     Serialization of a simple object with no children.
        /// </summary>
        [Test]
        public void SimpleRootTypeSerialization()
        {
            // Arrange
            var simpleRootItem = new SimpleRootType();
            var sut = new DocumentSerializer<SimpleRootType>(new[] {_fakeRootSerializer}, _fakeLogger);

            // Act
            var document = sut.SerializeToXDocument(simpleRootItem);

            // Assert
            Assert.IsNotNull(document?.Root);
            Assert.AreEqual("simpleroottype", document.Root.Name.ToString());
        }

        /// <summary>
        ///     Attempt to serialize a null root item value.
        /// </summary>
        [Test]
        public void NullRootItem()
        {
            // Arrange
            var sut = new DocumentSerializer<SimpleRootType>(new[] {_fakeRootSerializer}, _fakeLogger);

            // Act
            var document = sut.SerializeToXDocument(null);

            // Assert
            Assert.IsNull(document);
        }

        /// <summary>
        ///     Missing serializer for the root
        /// </summary>
        [Test]
        public void MissingSerializerForTheRoot()
        {
            // Arrange
            var simpleRootItem = new SimpleRootType();
            var sut = new DocumentSerializer<SimpleRootType>(Enumerable.Empty<IElementSerializer<SimpleRootType>>(),
                _fakeLogger);

            // Act / Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                sut.SerializeToXDocument(simpleRootItem);
            });
        }
    }
}