using System.Xml.Linq;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Contracts.Diagnostics;
using Symlconnect.DataModel;
using Symlconnect.DataModel.Deserializers;
using Symlconnect.Maternity.Common.Container;
using Symlconnect.Maternity.Common.UnitTests.Properties;

namespace Symlconnect.Maternity.Common.UnitTests
{
    [TestFixture]
    public class DataDictionaryUnitTests
    {
        [Test]
        public void TestLoadLiveDataDictionary()
        {
            // Arrange / Act
            var dataDictionary = LoadLiveDataDictionary();

            CollectionAssert.IsNotEmpty(dataDictionary.PropertyDefinitions);
            CollectionAssert.IsNotEmpty(dataDictionary.EntityDefinitions);
            CollectionAssert.IsNotEmpty(dataDictionary.RuleDefinitions);
            Assert.AreEqual(2,dataDictionary.RuleDefinitions.Count);
        }

        public IDataDictionary LoadLiveDataDictionary()
        {
            var document = XDocument.Parse(Resources.maternity_datadictionary);
            var elementDeserializers = DesignTimeConfiguration.GetDefaultDataDictionaryElementDeserializers();
            var elementGroupDeserializers = DesignTimeConfiguration.GetDefaultDataDictionaryElementGroupDeserializers();
            var deserializer = new DataDictionaryDocumentDeserializer(elementDeserializers, elementGroupDeserializers,
                A.Fake<ILogger>());

            var dataDictionary = deserializer.DeserializeFromXDocument(document);

            return dataDictionary;
        }
    }
}