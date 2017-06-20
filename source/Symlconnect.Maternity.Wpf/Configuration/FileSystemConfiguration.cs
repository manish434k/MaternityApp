using System;
using System.IO.Abstractions;

namespace Symlconnect.Maternity.Wpf.Configuration
{
    public class FileSystemConfiguration : IFileSystemConfiguration
    {
        private readonly IFileSystem _fileSystem;

        public FileSystemConfiguration(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        // Environment.SpecialFolder.CommonApplicationData = C:\ProgramData
        //public string ReadOnlyConfigurationDirectory
        //    => _fileSystem.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        //        "Symlconnect", "maternity");
        public string ReadOnlyConfigurationDirectory
            => _fileSystem.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "Sample", "maternity");


        // Environment.SpecialFolder.ApplicationData = <User>\AppData\Roaming\
        //public string WritableDataDirectory
        //    => _fileSystem.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        //        "Symlconnect", "maternity");

        public string WritableDataDirectory
            => _fileSystem.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "Sample", "maternity");
    }
}