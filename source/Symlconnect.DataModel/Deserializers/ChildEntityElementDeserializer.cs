using System;
using System.Xml.Linq;
using Symlconnect.Common.ExtensionMethods;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.DataModel.Deserializers
{
    public class ChildEntityElementDeserializer : IElementDeserializer<IEntity>
    {
        private readonly IValueDeserializer<DateTime> _dateTimeValueDeserializer;
        private readonly IFactory<ChildEntity> _childEntityFactory;

        public ChildEntityElementDeserializer(IValueDeserializer<DateTime> dateTimeValueDeserializer, IFactory<ChildEntity> childEntityFactory)
        {
            _dateTimeValueDeserializer = dateTimeValueDeserializer;
            _childEntityFactory = childEntityFactory;
        }

        public string ElementName => "childentity";

        public object DeserializeFromXElement(XElement element, object parent, IEntity root)
        {
            element.ValidateRequiredAttributes("sessionid", "userid", "createddatetime");

            var instance = _childEntityFactory.CreateInstance();
            instance.SessionId = element.Attribute("sessionid").Value;
            instance.UserId = element.Attribute("userid").Value;
            instance.CreatedDateTime =
                _dateTimeValueDeserializer.DeserializeValue(element.Attribute("createddatetime").Value);
            return instance;
        }
    }
}