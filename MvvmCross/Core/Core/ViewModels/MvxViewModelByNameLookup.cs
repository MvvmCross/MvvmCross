// MvxViewModelByNameLookup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.MvvmCross.ViewModels
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
        {
            return _availableViewModelsByName.TryGetValue(name, out viewModelType);
        }

        public bool TryLookupByFullName(string name, out Type viewModelType)
        {
            return _availableViewModelsByFullName.TryGetValue(name, out viewModelType);
        }

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
            var viewModelTypes = from type in assembly.ExceptionSafeGetTypes()
                                 where !type.GetTypeInfo().IsAbstract
                                 where !type.GetTypeInfo().IsInterface
                                 where typeof(IMvxViewModel).IsAssignableFrom(type)
                                 select type;

            foreach (var viewModelType in viewModelTypes)
            {
                Add(viewModelType);
            }
        }
    }
}