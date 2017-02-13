// MvxViewModelByNameLookup.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;

using MvvmCross.Platform;
using MvvmCross.Platform.IoC;

namespace MvvmCross.Core.ViewModels
{
    public class MvxViewModelByNameLookup : IMvxViewModelByNameLookup, IMvxViewModelByNameRegistry
    {
        private readonly Dictionary<string, Type> _availableViewModelsByName;
        private readonly Dictionary<string, Type> _availableViewModelsByFullName;

        public MvxViewModelByNameLookup()
        {
			_availableViewModelsByName = new Dictionary<string, Type>();
			_availableViewModelsByFullName = new Dictionary<string, Type>();
        }

        public bool TryLookupByName(string name, out Type viewModelType)
			=> _availableViewModelsByName.TryGetValue(name, out viewModelType);

        public bool TryLookupByFullName(string name, out Type viewModelType)
			=> _availableViewModelsByFullName.TryGetValue(name, out viewModelType);

        public void Add(Type viewModelType)
        {
			_availableViewModelsByName[viewModelType.Name] = viewModelType;
			_availableViewModelsByFullName[viewModelType.FullName] = viewModelType;
        }

        public void Add<TViewModel>() where TViewModel : IMvxViewModel
        {
			Add(typeof(TViewModel));
        }

        public void AddAll(Assembly assembly)
        {
            foreach (var viewModelType in assembly.ExceptionSafeGetTypes())
            {
				if (viewModelType.GetTypeInfo().IsAbstract) continue;
				if (viewModelType.GetTypeInfo().IsInterface) continue;
				if (!typeof(IMvxViewModel).IsAssignableFrom(viewModelType)) continue;

				Add(viewModelType);
            }
        }
    }
}