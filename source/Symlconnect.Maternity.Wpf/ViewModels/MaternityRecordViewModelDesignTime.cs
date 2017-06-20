using Symlconnect.Common.Diagnostics;
using Symlconnect.Common.Environment;
using Symlconnect.Common.Factories;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel.Deserializers;
using Symlconnect.Maternity.Common.Container;
using Symlconnect.Maternity.Wpf.Configuration;
using Symlconnect.Maternity.Wpf.DataDictionary;
using Symlconnect.Maternity.Wpf.Factories;
using Symlconnect.Maternity.Wpf.FileSystem;
using Symlconnect.ViewModel.Deserializers;
using Symlconnect.ViewModel.Factories;
using Symlconnect.ViewModel.ViewModels;

namespace Symlconnect.Maternity.Wpf.ViewModels
{
    public class MaternityRecordDesignTimeViewModel : MaternityRecordViewModel
    {
        public MaternityRecordDesignTimeViewModel()
            : base(
                new FormDefinitionLocator(new System.IO.Abstractions.FileSystem(),
                    new FileSystemConfiguration(new System.IO.Abstractions.FileSystem()),
                    new XDocumentFileOperations(),
                    new FormDefinitionDocumentDeserializer(
                        DesignTimeConfiguration.GetDefaultFormDefinitionElementDeserializers(),
                        DesignTimeConfiguration.GetDefaultFormDefinitionElementGroupDeserializers(),
                        new NullLogger())), new SimpleFactory<FormContext>(),
                new PatientViewModelFactory(new CurrentDateTimeProvider()), null, CreateFormViewModelFactory())
        {
            // Load a dummy maternity record
            var dataDictionaryLocator = new DataDictionaryLocator(new System.IO.Abstractions.FileSystem(),
                new FileSystemConfiguration(new System.IO.Abstractions.FileSystem()), new XDocumentFileOperations(),
                new DataDictionaryDocumentDeserializer(
                    DesignTimeConfiguration.GetDefaultDataDictionaryElementDeserializers(),
                    DesignTimeConfiguration.GetDefaultDataDictionaryElementGroupDeserializers(), new NullLogger()));
            var dataDictionary = dataDictionaryLocator.GetDataDictionary("maternity");

            Patient = DesignTimeConfiguration.CreateDummyPatient();
            Entity = DesignTimeConfiguration.CreateDummyPatientEntity(dataDictionary, "record", Patient);
            Entity.SetValue("Name", "Jane Doe", FormViewModel.FormContext.SessionContext);
        }

        private static IFactory<FormViewModel> CreateFormViewModelFactory()
        {
            var controlDefinitionViewModelFactories =
                DesignTimeConfiguration.GetDefaultControlDefinitionViewModelFactories();
            return
                new FormViewModelFactory(new FormSectionViewModelFactory(
                    new ControlDefinitionViewModelFactoryLocator(controlDefinitionViewModelFactories)));
        }
    }
}