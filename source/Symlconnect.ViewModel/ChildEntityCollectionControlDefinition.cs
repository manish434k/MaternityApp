namespace Symlconnect.ViewModel
{
    public class ChildEntityCollectionControlDefinition : ControlDefinitionBase
    {
        public string PropertyName { get; set; }
        public string FormDefinitionName { get; set; }
        public bool IsAddAllowed { get; set; }
    }
}