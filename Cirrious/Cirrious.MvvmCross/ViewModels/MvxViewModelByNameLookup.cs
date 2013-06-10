// MvxViewModelByNameLookup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cirrious.CrossCore.IoC;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxViewModelByNameLookup : IMvxViewModelByNameLookup
    {
        private readonly Assembly[] _availableAssemblies;
        private Dictionary<string, Type> _availableViewModelsByName;
        private Dictionary<string, Type> _availableViewModelsByFullName;

        public MvxViewModelByNameLookup(Assembly[] availableAssemblies)
        {
            _availableAssemblies = availableAssemblies;
        }

        public bool TryLookupByName(string name, out Type viewModelType)
        {
            if (_availableViewModelsByName == null)
            {
                BuildViewModelLookup();
            }

            return _availableViewModelsByName.TryGetValue(name, out viewModelType);
        }

        public bool TryLookupByFullName(string name, out Type viewModelType)
        {
            if (_availableViewModelsByFullName == null)
            {
                BuildViewModelLookup();
            }

            return _availableViewModelsByFullName.TryGetValue(name, out viewModelType);
        }

        private void BuildViewModelLookup()
        {
            var viewModels = from assembly in _availableAssemblies
                             from type in assembly.ExceptionSafeGetTypes()
                             where !type.IsAbstract
                             where !type.IsInterface
                             where typeof (IMvxViewModel).IsAssignableFrom(type)
                             select type;

            _availableViewModelsByName = new Dictionary<string, Type>();
            _availableViewModelsByFullName = new Dictionary<string, Type>();
            foreach (var viewModel in viewModels)
            {
                _availableViewModelsByName[viewModel.Name] = viewModel;
                _availableViewModelsByFullName[viewModel.FullName] = viewModel;
            }
        }
    }
}