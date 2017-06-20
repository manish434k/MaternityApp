using System;

namespace Symlconnect.Common.ExtensionMethods
{
    public static class DateTimeExtensionMethods
    {
        /// <summary>
        ///     Safely converts a date to UTC. Handles the problem scenario where the DateTimeKind is Unspecified which can result
        ///     in unwanted adjustment to date values.
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public static DateTime SafeUniversal(this DateTime dateTimeValue)
        {
            return DateTimeKind.Unspecified == dateTimeValue.Kind
                ? new DateTime(dateTimeValue.Ticks, DateTimeKind.Utc)
                : dateTimeValue.ToUniversalTime();
        }
    }
}