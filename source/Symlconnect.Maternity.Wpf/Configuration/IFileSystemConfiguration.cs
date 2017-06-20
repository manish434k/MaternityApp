namespace Symlconnect.Maternity.Wpf.Configuration
{
    /// <summary>
    /// Configuration information about local file system directories.
    /// </summary>
    public interface IFileSystemConfiguration
    {
        string ReadOnlyConfigurationDirectory { get; }
        string WritableDataDirectory { get; }
    }
}