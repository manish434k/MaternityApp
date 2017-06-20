using System.Collections;

namespace Symlconnect.Contracts.ObjectModel
{
    /// <summary>
    ///     Implemented by objects that contain logical child items.
    /// </summary>
    public interface IChildItemContainer
    {
        /// <summary>
        ///     Returns true if a given item can be a valid child of this object.
        /// </summary>
        /// <param name="item">Possible child item.</param>
        bool IsSupportedChildItem(object item);

        /// <summary>
        ///     Adds the passed item as a logical child of this object.
        /// </summary>
        /// <param name="item">New child item.</param>
        void AddChildItem(object item);

        /// <summary>
        ///     Returns the collection of all items that are logical children of this object.
        /// </summary>
        IEnumerable GetChildItems();
    }
}