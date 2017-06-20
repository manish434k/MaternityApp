using System.Collections.Generic;

namespace Symlconnect.ViewModel
{
    public interface ILookupDefinition
    {
        string Id { get; set; }
        IList<LookupEntry> LookupEntries { get; }
    }
}