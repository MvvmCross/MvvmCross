using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Platform
{
    public interface IMvxViewModelByNameLookup
    {
        bool TryLookup(string name, out Type viewModelType);
    }

    public class MvxViewModelByNameLookup : IMvxViewModelByNameLookup
    {
        private readonly Assembly[] _availableAssemblies;
        private Dictionary<string, Type> _availableViewModels;

        public MvxViewModelByNameLookup(Assembly[] availableAssemblies)
        {
            _availableAssemblies = availableAssemblies;
        }

        public bool TryLookup(string name, out Type viewModelType)
        {
            if (_availableViewModels == null)
            {
                BuildViewModelLookup();
            }

            return _availableViewModels.TryGetValue(name, out viewModelType);
        }

        private void BuildViewModelLookup()
        {
            var viewModels = from assembly in _availableAssemblies
                             from type in assembly.GetTypes()
                             where !type.IsAbstract
                             where !type.IsInterface
                             where typeof (IMvxViewModel).IsAssignableFrom(type)
                             select type;

            _availableViewModels = new Dictionary<string, Type>();
            foreach (var viewModel in viewModels)
            {
                _availableViewModels[viewModel.Name] = viewModel;
            }
        }
    }

    public class MvxAssociatedViewModelViewModelTypeFinder
        : IMvxAssociatedViewModelTypeFinder
    {
        private readonly IMvxViewModelByNameLookup _viewModelByNameLookup;

        public MvxAssociatedViewModelViewModelTypeFinder(IMvxViewModelByNameLookup viewModelByNameLookup)
        {
            _viewModelByNameLookup = viewModelByNameLookup;
        }

        public virtual Type FindAssociatedTypeOrNull(Type candidateType)
        {
            if (!CheckCandidateTypeIsAView(candidateType)) 
                return null;

            if (!CheckUnconventionalAttributes(candidateType)) 
                return null;

            if (!CheckConditionalAttribributes(candidateType)) 
                return null;

            var typeByAttribute = LookupAttributedViewModelType(candidateType);
            if (typeByAttribute != null)
                return typeByAttribute;

            var concrete = LookupAssociatedConcreteViewModelType(candidateType);
            if (concrete != null)
                return concrete;

            var typeByName = LookupNamedViewModelType(candidateType);
            if (typeByName != null)
                return typeByName;

            MvxTrace.Trace("No view model association found for candidate view {0}", candidateType.Name);
            return null;
        }

        protected virtual Type LookupAttributedViewModelType(Type candidateType)
        {
            var attribute = candidateType
                .GetCustomAttributes(typeof(MvxViewForAttribute), false)
                .FirstOrDefault() as MvxViewForAttribute;

            if (attribute == null)
                return null;

            return attribute.ViewModel;
        }

        protected virtual Type LookupNamedViewModelType(Type candidateType)
        {
            var viewName = candidateType.Name;
            var viewModelName = viewName + "Model";

            Type toReturn;
            _viewModelByNameLookup.TryLookup(viewModelName, out toReturn);
            return toReturn;
        }

        protected virtual Type LookupAssociatedConcreteViewModelType(Type candidateType)
        {
            var viewModelPropertyInfo = candidateType
                .GetProperties()
                .FirstOrDefault(x => x.Name == "ViewModel"
                                     && !x.PropertyType.IsInterface
                                     && !x.PropertyType.IsAbstract);

            if (viewModelPropertyInfo == null)
                return null;

            return viewModelPropertyInfo.PropertyType;
        }

        protected virtual bool CheckCandidateTypeIsAView(Type candidateType)
        {
            if (candidateType == null)
                return false;

            if (candidateType.IsAbstract)
                return false;

            if (!typeof (IMvxView).IsAssignableFrom(candidateType))
                return false;

            return true;
        }

        protected virtual bool CheckUnconventionalAttributes(Type candidateType)
        {
            var unconventionalAttributes = candidateType.GetCustomAttributes(typeof (MvxUnconventionalViewAttribute),
                                                                             true);
            if (unconventionalAttributes.Length > 0)
                return false;

            return true;
        }

        protected virtual bool CheckConditionalAttribributes(Type candidateType)
        {
            var conditionalAttributes =
                candidateType.GetCustomAttributes(typeof (MvxConditionalConventionalViewAttribute), true);

            foreach (MvxConditionalConventionalViewAttribute conditional in conditionalAttributes)
            {
                var result = conditional.IsConventional;
                if (!result)
                    return false;
            }
            return true;
        }
    }
}