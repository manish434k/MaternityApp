using System;
using System.IO.Abstractions;
using Symlconnect.Contracts.Serialization;
using Symlconnect.DataModel;
using Symlconnect.Maternity.Wpf.Configuration;
using Symlconnect.Maternity.Wpf.FileSystem;

namespace Symlconnect.Maternity.Wpf.DataDictionary
{
    /// <summary>
    ///     Loads a DataDictionary from the local file system.
    /// </summary>
    public class DataDictionaryLocator : IDataDictionaryLocator
    {
        private readonly IFileSystem _fileSystem;
        private readonly IFileSystemConfiguration _fileSystemConfiguration;
        private readonly IXDocumentFileLoad _documentFileLoader;
        private readonly IDocumentDeserializer<IDataDictionary> _dataDictionaryDocumentDeserializer;

        public DataDictionaryLocator(IFileSystem fileSystem,
            IFileSystemConfiguration fileSystemConfiguration,
            IXDocumentFileLoad documentFileLoader,
            IDocumentDeserializer<IDataDictionary> dataDictionaryDocumentDeserializer)
        {
            _fileSystem = fileSystem;
            _fileSystemConfiguration = fileSystemConfiguration;
            _documentFileLoader = documentFileLoader;
            _dataDictionaryDocumentDeserializer = dataDictionaryDocumentDeserializer;
        }

        public IDataDictionary GetDataDictionary(string name)
        {
            var dataDictionariesDirectory =
                _fileSystem.DirectoryInfo.FromDirectoryName(
                    _fileSystem.Path.Combine(_fileSystemConfiguration.ReadOnlyConfigurationDirectory, "datadictionaries"));
            if (!dataDictionariesDirectory.Exists)
            {
                throw new InvalidOperationException(
                    $"The Data Dictionaries directory {dataDictionariesDirectory.FullName} could not be found.");
            }
            var dataDictionaryFile =
                _fileSystem.FileInfo.FromFileName(_fileSystem.Path.Combine(dataDictionariesDirectory.FullName,
                    $"{name}.datadictionary"));
            if (!dataDictionaryFile.Exists)
            {
                throw new InvalidOperationException($"A Data Dictionary named {name} could not be found.");
            }
            var document = _documentFileLoader.LoadFromFile(dataDictionaryFile);
            var instance = _dataDictionaryDocumentDeserializer.DeserializeFromXDocument(document);
            return instance;
        }
    }
}