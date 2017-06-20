using System;
using System.Collections.Generic;
using System.Xml.Linq;
using FakeItEasy;
using NUnit.Framework;
using Symlconnect.Common.Serialization;
using Symlconnect.Common.UnitTests.Serialization.TestTypes;
using Symlconnect.Contracts.Diagnostics;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.Common.UnitTests.Serialization
{
    /// <summary>
    ///     Test serialization of a complex object graph (one with a root and child objects).
    /// </summary>
    [TestFixture]
    public class ComplexDocumentSerialization
    {
        private IElementSerializer<ComplexRootType> _fakeChildSerializer;
        private ComplexRootType _fakeComplexRoot;
        private ILogger _fakeLogger;
        private IElementSerializer<ComplexRootType> _fakeRootSerializer;
        private List<SimpleChildType> _childItems;

        [SetUp]
        public void TestSetup()
        {
            _fakeLogger = A.Fake<ILogger>();

            // Fake a complex root that supports adding SimpleChildType instances
            _fakeComplexRoot = A.Fake<ComplexRootType>();
            A.CallTo(() => _fakeComplexRoot.IsSupportedChildItem(A<object>.That.IsInstanceOf(typeof(SimpleChildType))))
                .Returns(true);
            _childItems = new List<SimpleChildType> {A.Fake<SimpleChildType>()};
            A.CallTo(() => _fakeComplexRoot.GetChildItems())
                .Returns(_childItems);

            // Fake "root" Serializer
            _fakeRootSerializer = A.Fake<IElementSerializer<ComplexRootType>>();
            A.CallTo(
                    () =>
                        _fakeRootSerializer.IsSerializerForType(
                            A<Type>.That.Matches(
                                t => t == typeof(ComplexRootType) || t.IsSubclassOf(typeof(ComplexRootType)))))
                .Returns(true);
            A.CallTo(
                    () =>
                        _fakeRootSerializer.SerializeToXElement(
                            A<object>.Ignored,
                            A<object>.Ignored,
                            A<ComplexRootType>.Ignored))
                .Returns(new XElement("root"));

            // Fake "child" Serializer
            _fakeChildSerializer = A.Fake<IElementSerializer<ComplexRootType>>();
            A.CallTo(
                    () =>
                        _fakeChildSerializer.IsSerializerForType(
                            A<Type>.That.Matches(
                                t => t == typeof(SimpleChildType) || t.IsSubclassOf(typeof(SimpleChildType)))))
                .Returns(true);
            A.CallTo(
                    () =>
                        _fakeChildSerializer.SerializeToXElement(
                            A<object>.Ignored,
                            A<object>.Ignored,
                            A<ComplexRootType>.Ignored))
                .Returns(new XElement("child"));
        }

        /// <summary>
        ///     Serialise an object graph with a root and a single child object.
        /// </summary>
        [Test]
        public void SerializeSimpleObjectGraph()
        {
            // Arrange
            var sut = new DocumentSerializer<ComplexRootType>(new[] {_fakeRootSerializer, _fakeChildSerializer},
                _fakeLogger);

            // Act
            var document = sut.SerializeToXDocument(_fakeComplexRoot);

            // Assert
            A.CallTo(
                    () =>
                        _fakeRootSerializer.SerializeToXElement(A<object>.Ignored, A<object>.Ignored,
                            A<ComplexRootType>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(
                    () =>
                        _fakeChildSerializer.SerializeToXElement(A<object>.Ignored, A<object>.Ignored,
                            A<ComplexRootType>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);

            Assert.IsNotNull(document);
            var expectedDocument = XDocument.Parse("<root><child/></root>");
            Assert.IsTrue(XNode.DeepEquals(expectedDocument, document),
                "Documents did not return true when compared with DeepEquals");
        }
    }
}