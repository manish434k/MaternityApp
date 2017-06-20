namespace Symlconnect.DataModel
{
    public interface IUser
    {
        /// <summary>
        ///     Unique identifier for a given user.
        /// </summary>
        string UserId { get; }

        /// <summary>
        ///     Display Name for a given user.
        /// </summary>
        string DisplayName { get; }
    }
}