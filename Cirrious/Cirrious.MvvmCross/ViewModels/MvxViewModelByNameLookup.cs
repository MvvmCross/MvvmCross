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

namespace Cirrious.MvvmCross.ViewModels
{
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
}