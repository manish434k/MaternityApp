using System;

namespace Symlconnect.Contracts.Environment
{
    /// <summary>
    ///     Provides an abstraction for getting the current system Date and Time.
    /// </summary>
    public interface ICurrentDateTimeProvider
    {
        /// <summary>
        ///     Returns the current Date and Time.
        /// </summary>
        DateTime GetCurrentDateTime();
    }
}