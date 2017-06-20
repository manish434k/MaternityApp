using Symlconnect.Contracts.Factories;
using Symlconnect.ViewModel.ViewModels;

namespace Symlconnect.ViewModel.Factories
{
    public class FormSectionViewModelFactory : IFactory<FormSectionViewModel>
    {
        private readonly IControlDefinitionViewModelFactoryLocator _controlDefinitionViewModelFactoryLocator;

        public FormSectionViewModelFactory(
            IControlDefinitionViewModelFactoryLocator controlDefinitionViewModelFactoryLocator)
        {
            _controlDefinitionViewModelFactoryLocator = controlDefinitionViewModelFactoryLocator;
        }

        public FormSectionViewModel CreateInstance()
        {
            return new FormSectionViewModel(this, _controlDefinitionViewModelFactoryLocator);
        }
    }
}