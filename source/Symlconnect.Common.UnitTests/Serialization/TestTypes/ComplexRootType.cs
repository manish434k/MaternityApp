using System.Collections;
using Symlconnect.Contracts.ObjectModel;

namespace Symlconnect.Common.UnitTests.Serialization.TestTypes
{
    public abstract class ComplexRootType : IChildItemContainer
    {
        public abstract bool IsSupportedChildItem(object item);

        public abstract void AddChildItem(object item);

        public abstract IEnumerable GetChildItems();
    }
}