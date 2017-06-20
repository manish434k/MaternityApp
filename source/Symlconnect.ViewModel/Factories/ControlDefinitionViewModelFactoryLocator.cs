using System;
using System.Collections.Generic;
using System.Linq;
using Symlconnect.ViewModel.ViewModels;

namespace Symlconnect.ViewModel.Factories
{
    public class ControlDefinitionViewModelFactoryLocator : IControlDefinitionViewModelFactoryLocator
    {
        private readonly IEnumerable<IControlDefinitionViewModelFactory> _controlDefinitionViewModelFactories;

        public ControlDefinitionViewModelFactoryLocator(
            IEnumerable<IControlDefinitionViewModelFactory> controlDefinitionViewModelFactories)
        {
            _controlDefinitionViewModelFactories = controlDefinitionViewModelFactories;
        }

        private Dictionary<Type, IControlDefinitionViewModelFactory> _controlDefinitionViewModelFactoryDictionary;

        private Dictionary<Type, IControlDefinitionViewModelFactory> ControlDefinitionViewModelFactoryDictionary
            =>
                _controlDefinitionViewModelFactoryDictionary ??
                (_controlDefinitionViewModelFactoryDictionary =
                    new Dictionary<Type, IControlDefinitionViewModelFactory>());

        public IControlDefinitionViewModelFactory LocateFactory(Type controlDefinitionType)
        {
            IControlDefinitionViewModelFactory factory;
            if (ControlDefinitionViewModelFactoryDictionary.ContainsKey(controlDefinitionType))
            {
                factory = ControlDefinitionViewModelFactoryDictionary[controlDefinitionType];
            }
            else
            {
                factory =
                    _controlDefinitionViewModelFactories.FirstOrDefault(
                        f => f.IsControlDefinitionTypeSupported(controlDefinitionType));
                ControlDefinitionViewModelFactoryDictionary.Add(controlDefinitionType, factory);
            }
            return factory;
        }
    }
}