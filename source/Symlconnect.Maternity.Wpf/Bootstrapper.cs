using System;
using System.Globalization;
using System.IO.Abstractions;
using System.Reflection;
using Microsoft.Practices.Unity;
using Prism.Unity;
using Symlconnect.Maternity.Wpf.Views;
using System.Windows;
using log4net;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Regions;
using Symlconnect.Common.Diagnostics;
using Symlconnect.Contracts.Diagnostics;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel;
using Symlconnect.Maternity.Common;
using Symlconnect.Maternity.Common.Container;
using Symlconnect.Maternity.Common.ViewModels;
using Symlconnect.Maternity.Wpf.Configuration;
using Symlconnect.Maternity.Wpf.DataDictionary;
using Symlconnect.Maternity.Wpf.Factories;
using Symlconnect.Maternity.Wpf.FileSystem;
using Symlconnect.Maternity.Wpf.Patient;
using Symlconnect.ViewModel;
using Symlconnect.ViewModel.ViewModels;

namespace Symlconnect.Maternity.Wpf
{
    class Bootstrapper : UnityBootstrapper
    {
        private readonly Logger _logger;

        public Bootstrapper()
        {
            _logger = new Logger(LogManager.GetLogger(GetType()));
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs ea)
        {
            _logger.Log(ea.ExceptionObject as Exception);
            MessageBox.Show(ea.ExceptionObject.ToString(), "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        // ReSharper disable once RedundantOverriddenMember
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterInstance<ILogger>(_logger);

            // Type registration goes here
            ContainerConfiguration.RegisterCommonTypes(Container);
            ContainerConfiguration.RegisterDataModelTypes(Container);
            ContainerConfiguration.RegisterViewModelTypes(Container);
            ContainerConfiguration.RegisterMaternityTypes(Container);

            Container.RegisterType<IXDocumentFileLoad, XDocumentFileOperations>();
            Container.RegisterType<IXDocumentFileSave, XDocumentFileOperations>();
            Container.RegisterType<IPatientLoader, PatientLoader>();
            Container.RegisterType<IFactory<PatientViewModel>, PatientViewModelFactory>();
            Container.RegisterType<IFileSystemConfiguration, FileSystemConfiguration>();
            Container.RegisterType<IFileSystem, System.IO.Abstractions.FileSystem>();
            Container.RegisterType<IDataDictionaryLocator, DataDictionaryLocator>();

            Container.RegisterType<IFactory<MaternityRecordViewModel>, MaternityRecordViewModelFactory>();

            Container.RegisterType<IFormDefinitionLocator, FormDefinitionLocator>();

            Container.RegisterType<object, HomeView>("Home");
            Container.RegisterType<object, PatientHomeView>("PatientHome");
            Container.RegisterType<object, MaternityRecordView>("MaternityRecord");
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            // Customized ViewModelLocator to allow us to fetch ViewModels from the common assembly
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
            {
                var viewName = viewType.FullName;
                viewName = viewName.Replace(".Views.", ".ViewModels.");
                viewName = viewName.Replace(".Views.", ".ViewModels.");
                var suffix = viewName.EndsWith("View") ? "Model" : "ViewModel";
                var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}{1}", viewName, suffix);

                var assembly = viewType.GetTypeInfo().Assembly;
                var type = assembly.GetType(viewModelName);
                if (type == null)
                {
                    var otherName = viewModelName.Replace(assembly.GetName().Name, typeof(ContainerConfiguration).Assembly.GetName().Name);
                    type = typeof(ContainerConfiguration).Assembly.GetType(otherName);
                }
                if (type == null)
                {
                    var otherName = viewModelName.Replace(assembly.GetName().Name, typeof(FormViewModel).Assembly.GetName().Name);
                    type = typeof(FormViewModel).Assembly.GetType(otherName);
                }

                return type;
            });
        }

        protected override ILoggerFacade CreateLogger()
        {
            return new DebugLogger();
        }

        // Called by app at startup to set the initial view in MainRegion
        public void ShowDefault()
        {
            RegionManager.GetRegionManager(Application.Current.MainWindow).RegisterViewWithRegion("MainRegion", typeof(HomeView));
        }
    }
}
