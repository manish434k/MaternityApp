using System;
using Symlconnect.Contracts.Factories;
using Symlconnect.ViewModel.ViewModels;

namespace Symlconnect.ViewModel.Factories
{
    public class ChildEntityCollectionControlDefinitionViewModelFactory :
        IControlDefinitionViewModelFactory
    {
        private readonly IFactory<IFormContext> _formContextFactory;
        private readonly IFormDefinitionLocator _formDefinitionLocator;

        public ChildEntityCollectionControlDefinitionViewModelFactory(IFactory<IFormContext> formContextFactory,
            IFormDefinitionLocator formDefinitionLocator)
        {
            _formContextFactory = formContextFactory;
            _formDefinitionLocator = formDefinitionLocator;
        }

        public bool IsControlDefinitionTypeSupported(Type controlDefinitionType)
        {
            return controlDefinitionType == typeof(ChildEntityCollectionControlDefinition);
        }

        public IControlDefinitionViewModel CreateViewModel()
        {
            return new ChildEntityCollectionControlDefinitionViewModel(_formDefinitionLocator,
                _formContextFactory);
        }
    }
}