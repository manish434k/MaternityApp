using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using FakeItEasy;
using Symlconnect.Common.Environment;
using Symlconnect.Common.Serialization;
using Symlconnect.Contracts.Serialization;
using Symlconnect.DataModel;
using Symlconnect.DataModel.Deserializers;
using Symlconnect.DataModel.Factories;
using Symlconnect.Maternity.Common.UnitTests.Properties;
using Symlconnect.UnitTests.Framework;

namespace Symlconnect.Maternity.Common.UnitTests
{
    [TestFixture]
    public class MaternityRecordEntityUnitTests
    {
        [Test]
        public void LoadLiveRecordEntity()
        {
            var document = XDocument.Parse(Resources.maternity_record);
            var commonValueDeserializers = new CommonValueDeserializers();
            var dataDictionaryTests = new DataDictionaryUnitTests();
            var dataDictionary = dataDictionaryTests.LoadLiveDataDictionary();
            var dataDictionaryLocator = A.Fake<IDataDictionaryLocator>();
            A.CallTo(() => dataDictionaryLocator.GetDataDictionary(A<string>.That.Matches(s => s == "maternity")))
                .Returns(dataDictionary);
            var elementDeserializers = new List<IElementDeserializer<IEntity>>
            {
                new ChildEntityCollectionElementDeserializer(new ChildEntityCollectionFactory()),
                new ChildEntityElementDeserializer(commonValueDeserializers, new ChildEntityFactory()),
                new EntityElementDeserializer(
                    new EntityFactory(
                        new EntityPropertyValueCollectionFactory(new CurrentDateTimeProvider(),
                            new EntityPropertyValueFactory()), new ChildEntityCollectionFactory(),
                        new ChildEntityFactory(), new CurrentDateTimeProvider()), dataDictionaryLocator,
                    commonValueDeserializers),
                new EntityPropertyValueElementDeserializer(new EntityPropertyValueFactory(), commonValueDeserializers,
                    commonValueDeserializers, commonValueDeserializers, commonValueDeserializers),
                new EntityPropertyValueCollectionElementDeserializer(
                    new EntityPropertyValueCollectionFactory(new CurrentDateTimeProvider(),
                        new EntityPropertyValueFactory()))
            };
            var entityDocumentDeserializer = new EntityDocumentDeSerializer(elementDeserializers,
                Enumerable.Empty<IElementGroupDeserializer>(), new AssertLogger());

            var instance = entityDocumentDeserializer.DeserializeFromXDocument(document);

            Assert.IsNotNull(instance);
            var sessionContext = new SessionContext
            {
                SessionDateTime = DateTime.Now,
                SessionId = "SessionId2",
                SessionUser = new User
                {
                    DisplayName = "User",
                    UserId = "UserId"
                }
            };
            Assert.AreEqual("John Doe", instance.GetValue("Name", sessionContext));

            var consultations = instance.GetChildEntities("AntenatalConsultations", sessionContext);
            CollectionAssert.IsNotEmpty(consultations);
        }
    }
}