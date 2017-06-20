using System;
using System.Collections;
using System.Collections.Generic;
using Symlconnect.Contracts.ObjectModel;

namespace Symlconnect.ViewModel
{
    public class LookupDefinition : IChildItemContainer, ILookupDefinition
    {
        public string Id { get; set; }

        private readonly Lazy<List<LookupEntry>> _lookupEntries
            = new Lazy<List<LookupEntry>>(() => new List<LookupEntry>());

        public IList<LookupEntry> LookupEntries => _lookupEntries.Value;

        #region IChildItemContainer

        public bool IsSupportedChildItem(object item)
        {
            return item is LookupEntry;
        }

        public void AddChildItem(object item)
        {
            if (item is LookupEntry)
            {
                LookupEntries.Add((LookupEntry) item);
            }
        }

        public IEnumerable GetChildItems()
        {
            return LookupEntries;
        }

        #endregion
    }
}