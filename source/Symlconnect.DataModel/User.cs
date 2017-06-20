using System.Diagnostics.CodeAnalysis;

namespace Symlconnect.DataModel
{
    [ExcludeFromCodeCoverage] // Simple Object
    public class User : IUser
    {
        public string UserId { get; set; }
        public string DisplayName { get; set; }
    }
}