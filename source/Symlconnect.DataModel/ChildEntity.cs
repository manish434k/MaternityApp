using System;
using System.Collections;
using System.Linq;
using Symlconnect.Contracts.ObjectModel;

namespace Symlconnect.DataModel
{
    /// <summary>
    ///     A container for a Child Entity.
    /// </summary>
    public class ChildEntity: IChildItemContainer
    {
        public string SessionId { get; internal set; }
        public string UserId { get; internal set; }
        public DateTime CreatedDateTime { get; set; }
        public IEntity Entity { get; set; }

        #region IChildItemContainer

        bool IChildItemContainer.IsSupportedChildItem(object item)
        {
            return item is IEntity;
        }

        void IChildItemContainer.AddChildItem(object item)
        {
            if (item is IEntity)
            {
                Entity = (IEntity)item;
            }
        }

        IEnumerable IChildItemContainer.GetChildItems()
        {
            return Entity != null ? new[] {Entity} : Enumerable.Empty<IEntity>();
        }

        #endregion
    }
}