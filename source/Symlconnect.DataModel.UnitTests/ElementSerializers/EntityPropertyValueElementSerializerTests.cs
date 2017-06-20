using System;
using System.Xml.Linq;
using FakeItEasy;
using NUnit.Framework;
using Symlconnect.Common.ExtensionMethods;
using Symlconnect.Contracts.Serialization;
using Symlconnect.DataModel.Serializers;

namespace Symlconnect.DataModel.UnitTests.ElementSerializers
{
    [TestFixture]
    public class EntityPropertyValueElementSerializerTests
    {
        private IValueSerializer<bool> _fakeBoolValueSerializer;
        private IValueSerializer<DateTime> _fakeDateTimeValueSerializer;
        private IValueSerializer<double> _fakeDoubleValueSerializer;
        private IValueSerializer<long> _fakeLongValueSerializer;

        [SetUp]
        public void TestSetup()
        {
            _fakeBoolValueSerializer = A.Fake<IValueSerializer<bool>>();
            A.CallTo(() =>
                    _fakeBoolValueSerializer.SerializeValue(A<bool>.Ignored))
                .ReturnsLazily((bool value) => $"Bool:{value}");

            _fakeDateTimeValueSerializer = A.Fake<IValueSerializer<DateTime>>();
            A.CallTo(() =>
                    _fakeDateTimeValueSerializer.SerializeValue(A<DateTime>.Ignored))
                .ReturnsLazily((DateTime value) => $"DateTime:{value:O}");

            _fakeDoubleValueSerializer = A.Fake<IValueSerializer<double>>();
            A.CallTo(() =>
                    _fakeDoubleValueSerializer.SerializeValue(A<double>.Ignored))
                .ReturnsLazily((double value) => $"Double:{value:N}");

            _fakeLongValueSerializer = A.Fake<IValueSerializer<long>>();
            A.CallTo(() =>
                    _fakeLongValueSerializer.SerializeValue(A<long>.Ignored))
                .ReturnsLazily((long value) => $"Long:{value:D}");
        }

        [TestCase("stringvalue", null, "stringvalue")]
        [TestCase(true, "bool", "Bool:True")]
        [TestCase(1.1, "double", "Double:1.10")]
        [TestCase(1, "long", "Long:1")]
        [TestCase(null, null, null)]
        [TestCase("DateTime:01/01/2017", "datetime", "DateTime:2017-01-01T00:00:00.0000000Z")]
        public void TestPropertyValueSerialization(object providedValue, string expectedValueKind,
            string expectedSerializedValue)
        {
            if (providedValue is string && ((string) providedValue).StartsWith("DateTime:"))
            {
                // Cannot specify a constant date time value so kind of a fudge to include it in the Test Cases
                providedValue = DateTime.Parse(((string) providedValue).Substring(9)).SafeUniversal();
            }

            // Arrange
            var value = new EntityPropertyValue
            {
                SessionId = "SessionIdValue",
                ChangeDateTime = new DateTime(2017, 1, 1),
                UserId = "UserIdValue",
                Value = providedValue
            };

            var sut = new EntityPropertyValueElementSerializer(_fakeBoolValueSerializer,
                _fakeDateTimeValueSerializer,
                _fakeDoubleValueSerializer,
                _fakeLongValueSerializer);

            // Act
            var element = sut.SerializeToXElement(value, null, null);

            // Assert
            var expectedElement =
                XElement.Parse(
                    "<value sessionid=\"SessionIdValue\" userid=\"UserIdValue\" changedatetime=\"DateTime:2017-01-01T00:00:00.0000000\"/>");
            if (expectedValueKind != null)
            {
                expectedElement.SetAttributeValue("valuekind", expectedValueKind);
            }
            if (expectedSerializedValue != null)
            {
                expectedElement.Value = expectedSerializedValue;
            }
            Assert.IsTrue(XNode.DeepEquals(expectedElement, element), $"Expected {expectedElement} but was {element}");
        }

        [Test]
        public void InvalidSerialization()
        {
            // Arrange
            var sut = new EntityPropertyValueElementSerializer(_fakeBoolValueSerializer,
                _fakeDateTimeValueSerializer,
                _fakeDoubleValueSerializer,
                _fakeLongValueSerializer);

            // Act
            var result = sut.SerializeToXElement(A.Fake<object>(), null, null);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void InvalidValue()
        {
            // Arrange
            var value = new EntityPropertyValue
            {
                SessionId = "SessionIdValue",
                ChangeDateTime = new DateTime(2017, 1, 1),
                UserId = "UserIdValue",
                Value = A.Fake<object>()
            };

            var sut = new EntityPropertyValueElementSerializer(_fakeBoolValueSerializer,
                _fakeDateTimeValueSerializer,
                _fakeDoubleValueSerializer,
                _fakeLongValueSerializer);

            // Act
            Assert.Throws<InvalidOperationException>(() => { sut.SerializeToXElement(value, null, null); });
        }
    }
}