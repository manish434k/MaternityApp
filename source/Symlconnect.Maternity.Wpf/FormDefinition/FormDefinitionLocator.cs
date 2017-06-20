using System;
using System.IO.Abstractions;
using Symlconnect.Contracts.Serialization;
using Symlconnect.Maternity.Wpf.Configuration;
using Symlconnect.Maternity.Wpf.FileSystem;
using Symlconnect.ViewModel;

namespace Symlconnect.Maternity.Wpf.DataDictionary
{
    public class FormDefinitionLocator : IFormDefinitionLocator
    {
        private readonly IFileSystem _fileSystem;
        private readonly IFileSystemConfiguration _fileSystemConfiguration;
        private readonly IXDocumentFileLoad _documentFileLoader;
        private readonly IDocumentDeserializer<IFormDefinition> _formDefinitionDocumentDeserializer;

        public FormDefinitionLocator(IFileSystem fileSystem,
            IFileSystemConfiguration fileSystemConfiguration,
            IXDocumentFileLoad documentFileLoader,
            IDocumentDeserializer<IFormDefinition> formDefinitionDocumentDeserializer)
        {
            _fileSystem = fileSystem;
            _fileSystemConfiguration = fileSystemConfiguration;
            _documentFileLoader = documentFileLoader;
            _formDefinitionDocumentDeserializer = formDefinitionDocumentDeserializer;
        }

        public IFormDefinition GetFormDefinition(string name)
        {
            var formDefinitionsDirectory =
                _fileSystem.DirectoryInfo.FromDirectoryName(
                    _fileSystem.Path.Combine(_fileSystemConfiguration.ReadOnlyConfigurationDirectory, "formdefinitions"));
            if (!formDefinitionsDirectory.Exists)
            {
                throw new InvalidOperationException(
                    $"The Form Definitions directory {formDefinitionsDirectory.FullName} could not be found.");
            }
            var dataDictionaryFile =
                _fileSystem.FileInfo.FromFileName(_fileSystem.Path.Combine(formDefinitionsDirectory.FullName,
                    $"{name}.form"));
            if (!dataDictionaryFile.Exists)
            {
                throw new InvalidOperationException($"A Data Dictionary named {name} could not be found.");
            }
            var document = _documentFileLoader.LoadFromFile(dataDictionaryFile);
            var instance = _formDefinitionDocumentDeserializer.DeserializeFromXDocument(document);
            return instance;
        }
    }
}