// MvxViewModelByNameLookup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using MvvmCross.Platform;
    using MvvmCross.Platform.IoC;

    public class MvxViewModelByNameLookup : IMvxViewModelByNameLookup, IMvxViewModelByNameRegistry
    {
        private readonly Dictionary<string, Type> _availableViewModelsByName;
        private readonly Dictionary<string, Type> _availableViewModelsByFullName;

        public MvxViewModelByNameLookup()
        {
            this._availableViewModelsByName = new Dictionary<string, Type>();
            this._availableViewModelsByFullName = new Dictionary<string, Type>();
        }

        public bool TryLookupByName(string name, out Type viewModelType)
        {
            return this._availableViewModelsByName.TryGetValue(name, out viewModelType);
        }

        public bool TryLookupByFullName(string name, out Type viewModelType)
        {
            return this._availableViewModelsByFullName.TryGetValue(name, out viewModelType);
        }

        public void Add(Type viewModelType)
        {
            this._availableViewModelsByName[viewModelType.Name] = viewModelType;
            this._availableViewModelsByFullName[viewModelType.FullName] = viewModelType;
        }

        public void Add<TViewModel>() where TViewModel : IMvxViewModel
        {
            this.Add(typeof(TViewModel));
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
                this.Add(viewModelType);
            }
        }
    }
}