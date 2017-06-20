using System;
using NUnit.Framework;
using Symlconnect.ViewModel.Media;

namespace Symlconnect.DataModel.UnitTests.IntegrationTests
{
    [TestFixture]
    public class ColorUnitTests
    {
        [Test]
        public void ConvertFromHexBlack()
        {
            // Arrange/Act
            var sut = Color.FromHexString("#000000");

            // Assert
            Assert.AreEqual(255, sut.A);
            Assert.AreEqual(0, sut.R);
            Assert.AreEqual(0, sut.G);
            Assert.AreEqual(0, sut.B);
        }

        [Test]
        public void ConvertFromHexWhite()
        {
            // Arrange/Act
            var sut = Color.FromHexString("#FFFFFF");

            // Assert
            Assert.AreEqual(255, sut.A);
            Assert.AreEqual(255, sut.R);
            Assert.AreEqual(255, sut.G);
            Assert.AreEqual(255, sut.B);
        }

        [Test]
        public void ConvertFromHexWhiteWithAlpha()
        {
            // Arrange/Act
            var sut = Color.FromHexString("#00FFFFFF");

            // Assert
            Assert.AreEqual(0, sut.A);
            Assert.AreEqual(255, sut.R);
            Assert.AreEqual(255, sut.G);
            Assert.AreEqual(255, sut.B);
        }

        [Test]
        public void ConvertToHex()
        {
            // Arrange
            Color sut;
            sut.A = 50;
            sut.R = 100;
            sut.G = 150;
            sut.B = 200;

            // Act
            var result = sut.ToHexString();

            // Assert
            Assert.AreEqual("#326496C8", result);
        }

        [Test]
        public void ConvertFromHexInvalidLength()
        {
            // Arrange/Act/Asssert
            Assert.Throws<ArgumentOutOfRangeException>(() => { Color.FromHexString("#FFFFF"); });
        }

        [Test]
        public void ConvertFromHexInvalidCharacters()
        {
            // Arrange/Act/Asssert
            Assert.Throws<FormatException>(() => { Color.FromHexString("#LLLLLL"); });
        }
    }
}