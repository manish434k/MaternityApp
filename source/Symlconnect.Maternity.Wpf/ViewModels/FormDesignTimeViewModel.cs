using System;
using Symlconnect.Common.Diagnostics;
using Symlconnect.DataModel;
using Symlconnect.DataModel.Deserializers;
using Symlconnect.Maternity.Common.Container;
using Symlconnect.Maternity.Wpf.Configuration;
using Symlconnect.Maternity.Wpf.DataDictionary;
using Symlconnect.Maternity.Wpf.FileSystem;
using Symlconnect.ViewModel.Deserializers;
using Symlconnect.ViewModel.Factories;
using Symlconnect.ViewModel.ViewModels;

namespace Symlconnect.Maternity.Wpf.ViewModels
{
    public class FormDesignTimeViewModel : FormViewModel
    {
        public FormDesignTimeViewModel()
            : base(
                new FormSectionViewModelFactory(
                    new ControlDefinitionViewModelFactoryLocator(
                        DesignTimeConfiguration.GetDefaultControlDefinitionViewModelFactories())))
        {
            var fileSystem = new System.IO.Abstractions.FileSystem();

            var dataDictionaryLocator = new DataDictionaryLocator(fileSystem,
                new FileSystemConfiguration(fileSystem), new XDocumentFileOperations(),
                new DataDictionaryDocumentDeserializer(
                    DesignTimeConfiguration.GetDefaultDataDictionaryElementDeserializers(),
                    DesignTimeConfiguration.GetDefaultDataDictionaryElementGroupDeserializers(), new NullLogger()));

            var formDefinitionLocator = new FormDefinitionLocator(fileSystem,
                new FileSystemConfiguration(fileSystem), new XDocumentFileOperations(),
                new FormDefinitionDocumentDeserializer(
                    DesignTimeConfiguration.GetDefaultFormDefinitionElementDeserializers(),
                    DesignTimeConfiguration.GetDefaultFormDefinitionElementGroupDeserializers(), new NullLogger()));

            var dataDictionary = dataDictionaryLocator.GetDataDictionary("maternity");
            var patient = DesignTimeConfiguration.CreateDummyPatient();
            var entity = DesignTimeConfiguration.CreateDummyPatientEntity(dataDictionary, "record", patient);

            FormDefinition = formDefinitionLocator.GetFormDefinition("maternity");
            FormContext = new FormContext
            {
                Entity = entity,
                SessionContext = new SessionContext
                {
                    SessionDateTime = entity.CreatedDateTime,
                    SessionId = Guid.NewGuid().ToString(),
                    SessionUser =
                        new User {DisplayName = entity.CreatedByUserDisplayName, UserId = entity.CreatedByUserId}
                }
            };

            entity.SetValue("Name", "Jane Doe", FormContext.SessionContext);
        }
    }
}