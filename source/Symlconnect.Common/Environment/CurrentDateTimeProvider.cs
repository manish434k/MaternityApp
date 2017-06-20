using System;
using Symlconnect.Contracts.Environment;

namespace Symlconnect.Common.Environment
{
    public class CurrentDateTimeProvider : ICurrentDateTimeProvider
    {
        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
    }
}