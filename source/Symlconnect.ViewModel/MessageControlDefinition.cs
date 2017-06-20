namespace Symlconnect.ViewModel
{
    public abstract class MessageControlDefinitionBase : ControlDefinitionBase
    {
        public string Content { get; set; }
        public string ContentPropertyName { get; set; }
    }
}