using Symlconnect.Contracts.Factories;
using Symlconnect.ViewModel.ViewModels;

namespace Symlconnect.ViewModel.Factories
{
    public class FormViewModelFactory : IFactory<FormViewModel>
    {
        private readonly IFactory<FormSectionViewModel> _formSectionViewModelFactory;

        public FormViewModelFactory(IFactory<FormSectionViewModel> formSectionViewModelFactory)
        {
            _formSectionViewModelFactory = formSectionViewModelFactory;
        }

        public FormViewModel CreateInstance()
        {
            return new FormViewModel(_formSectionViewModelFactory);
        }
    }
}