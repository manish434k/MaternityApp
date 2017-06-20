using Symlconnect.Contracts.Factories;

namespace Symlconnect.ViewModel.Deserializers
{
    public class EditableDropdownControlDefinitionElementDeserializer : DropdownControlDefinitionElementDeserializer
    {
        public EditableDropdownControlDefinitionElementDeserializer(IFactory<EditableDropdownControlDefinition> factory)
            : base(factory, "editabledropdown")
        {
        }
    }
}