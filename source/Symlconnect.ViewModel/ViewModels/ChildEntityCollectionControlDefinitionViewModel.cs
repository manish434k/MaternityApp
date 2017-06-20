using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel;

namespace Symlconnect.ViewModel.ViewModels
{
    public class ChildEntityCollectionControlDefinitionViewModel :
        ControlDefinitionViewModelBase<ChildEntityCollectionControlDefinition>
    {
        private readonly IFormDefinitionLocator _formDefinitionLocator;
        private readonly IFactory<IFormContext> _formContextFactory;

        public ChildEntityCollectionControlDefinitionViewModel(IFormDefinitionLocator formDefinitionLocator,
            IFactory<IFormContext> formContextFactory)
        {
            _formDefinitionLocator = formDefinitionLocator;
            _formContextFactory = formContextFactory;
        }

        private IFormDefinition _childFormDefinition;

        public IFormDefinition ChildFormDefinition
        {
            get
            {
                if (_childFormDefinition == null)
                {
                    _childFormDefinition = _formDefinitionLocator.GetFormDefinition(ControlDefinition.FormDefinitionName);
                }
                return _childFormDefinition;
            }
        }

        private ObservableCollection<FormViewModel> _childFormViewModels;

        public ObservableCollection<FormViewModel> ChildFormViewModels
        {
            get
            {
                if (_childFormViewModels == null && FormContext != null)
                {
                    _childFormViewModels = new ObservableCollection<FormViewModel>();
                    foreach (var childEntity in GetChildEntities())
                    {
                        var viewModel = FormContext.ChildFormViewModelFactory.CreateInstance();
                        viewModel.FormContext = CreateFormContext(_formContextFactory, childEntity, ChildFormDefinition);
                        viewModel.FormDefinition = ChildFormDefinition;
                        _childFormViewModels.Add(viewModel);
                    }
                }
                return _childFormViewModels;
            }
        }

        /// <summary>
        ///     This code is now in 2 places, but until we pin down the SessionContext creation / management I will leave it as
        ///     such.
        /// </summary>
        private static IFormContext CreateFormContext(IFactory<IFormContext> formContextFactory, IEntity entity,
            IFormDefinition formDefinition)
        {
            if (entity == null)
            {
                return null;
            }

            var formContext = formContextFactory.CreateInstance();
            formContext.FormDefinition = formDefinition;
            formContext.Entity = entity;
            formContext.SessionContext = new SessionContext
            {
                SessionDateTime = entity.CreatedDateTime,
                SessionId = Guid.NewGuid().ToString(),
                SessionUser = new User {DisplayName = entity.CreatedByUserDisplayName, UserId = entity.CreatedByUserId}
            };
            return formContext;
        }

        private IEnumerable<IEntity> GetChildEntities()
        {
            return FormContext.Entity.GetChildEntities(ControlDefinition.PropertyName, FormContext.SessionContext);
        }

        private ChildEntityCollectionPropertyDefinition PropertyDefinition
        {
            get
            {
                if (FormContext != null
                    &&
                    FormContext.Entity.EntityDefinition.DataDictionary.PropertyDefinitions.Contains(
                        ControlDefinition.PropertyName))
                {
                    return
                        (ChildEntityCollectionPropertyDefinition)
                        FormContext.Entity.EntityDefinition.DataDictionary.PropertyDefinitions[
                            ControlDefinition.PropertyName];
                }

                return null;
            }
        }
    }
}