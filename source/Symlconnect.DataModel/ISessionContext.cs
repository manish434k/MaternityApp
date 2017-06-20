using System;

namespace Symlconnect.DataModel
{
    /// <summary>
    ///     Provides a logical context for a Session. A Session is unique by SessionId.
    /// </summary>
    /// <remarks>
    ///     Additional context is provided by the SessionUser and SessionDateTime values, but they are not part of the unique
    ///     signature of a Session.
    /// </remarks>
    public interface ISessionContext
    {
        /// <summary>
        ///     A unique identifier for this Session.
        /// </summary>
        string SessionId { get; }

        /// <summary>
        ///     Date and Time the Session was created.
        /// </summary>
        DateTime SessionDateTime { get; }

        /// <summary>
        ///     User that owns the Session.
        /// </summary>
        IUser SessionUser { get; }
    }
}