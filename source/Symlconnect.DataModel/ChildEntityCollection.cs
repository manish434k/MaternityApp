using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Symlconnect.Contracts.ObjectModel;

namespace Symlconnect.DataModel
{
    public class ChildEntityCollection : List<ChildEntity>, IChildItemContainer
    {
        public virtual string PropertyName { get; set; }

        #region IChildItemContainer

        bool IChildItemContainer.IsSupportedChildItem(object item)
        {
            return item is ChildEntity;
        }

        void IChildItemContainer.AddChildItem(object item)
        {
            if (item is ChildEntity)
            {
                Add((ChildEntity)item);
            }
        }

        IEnumerable IChildItemContainer.GetChildItems()
        {
            return this;
        }

        #endregion

        /// <summary>
        /// Return any Child Entities in this collection that belong to this session, or that were created before this session.
        /// </summary>
        /// <param name="sessionContext">The session context that determines which Child Entities to return.</param>
        public IEnumerable<ChildEntity> GetChildItems(ISessionContext sessionContext)
        {
            return
                this.Where(
                    ce =>
                        ce.SessionId == sessionContext.SessionId || ce.CreatedDateTime <= sessionContext.SessionDateTime);
        }
    }
}