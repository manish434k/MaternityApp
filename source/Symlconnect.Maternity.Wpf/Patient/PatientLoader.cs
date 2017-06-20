using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;
using Symlconnect.DataModel;
using Symlconnect.Maternity.Common;
using Symlconnect.Maternity.Wpf.Configuration;
using Symlconnect.Maternity.Wpf.FileSystem;

namespace Symlconnect.Maternity.Wpf.Patient
{
    public class PatientLoader : IPatientLoader
    {
        private readonly IFileSystem _fileSystem;
        private readonly IFileSystemConfiguration _fileSystemConfiguration;
        private readonly IXDocumentFileLoad _documentFileLoader;
        private readonly IFactory<IPatient> _patientFactory;
        private readonly IDocumentDeserializer<IEntity> _entityDocumentDeserializer;
        private readonly IValueDeserializer<DateTime> _dateValueDeserializer;

        public PatientLoader(IFileSystem fileSystem, 
            IFileSystemConfiguration fileSystemConfiguration,
            IXDocumentFileLoad documentFileLoader,
            IFactory<IPatient> patientFactory, 
            IDocumentDeserializer<IEntity> entityDocumentDeserializer,
            IValueDeserializer<DateTime> dateValueDeserializer)
        {
            _fileSystem = fileSystem;
            _fileSystemConfiguration = fileSystemConfiguration;
            _documentFileLoader = documentFileLoader;
            _patientFactory = patientFactory;
            _entityDocumentDeserializer = entityDocumentDeserializer;
            _dateValueDeserializer = dateValueDeserializer;
        }

        public IEnumerable<IPatient> LoadPatients()
        {
            var patients = new List<IPatient>();
            var patientFiles =
                _fileSystem.DirectoryInfo.FromDirectoryName(
                        _fileSystem.Path.Combine(_fileSystemConfiguration.WritableDataDirectory, "patients"))
                    .GetFiles("*.patient");
            foreach (var patientFile in patientFiles)
            {
                var patientDocument = _documentFileLoader.LoadFromFile(patientFile);
                var newPatient = _patientFactory.CreateInstance();
                if (patientDocument.Root != null)
                {
                    newPatient.Id = patientDocument.Root.Attribute("id")?.Value;
                    newPatient.Name = patientDocument.Root.Attribute("name")?.Value;
                    newPatient.PatientNumber = patientDocument.Root.Attribute("patientnumber")?.Value;
                    newPatient.DateOfBirth =
                        _dateValueDeserializer.DeserializeValue(patientDocument.Root.Attribute("dateofbirth")?.Value);
                }
                patients.Add(newPatient);
            }
            return patients;
        }

        public IEnumerable<IEntity> LoadPatientEntities(IPatient patient, string entityName)
        {
            var entities = new List<IEntity>();
            var patientDirectory =
                _fileSystem.DirectoryInfo.FromDirectoryName(
                    _fileSystem.Path.Combine(_fileSystemConfiguration.WritableDataDirectory, "patients", patient.Id,
                        entityName));
            if (patientDirectory.Exists)
            {
                foreach (var entityFile in patientDirectory.GetFiles("*.entity"))
                {
                    var entityDocument = _documentFileLoader.LoadFromFile(entityFile);
                    var instance = _entityDocumentDeserializer.DeserializeFromXDocument(entityDocument);
                    if (instance != null)
                    {
                        entities.Add(instance);
                    }
                }
            }
            return entities;
        }
    }
}