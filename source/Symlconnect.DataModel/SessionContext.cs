using System;

namespace Symlconnect.DataModel
{
    public class SessionContext : ISessionContext
    {
        public string SessionId { get;  set; }
        public DateTime SessionDateTime { get;  set; }
        public IUser SessionUser { get;  set; }
    }
}