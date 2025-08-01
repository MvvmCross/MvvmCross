// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MvvmCross.IoC;

namespace MvvmCross.ViewModels
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

        public bool TryLookupByName(string name, out Type? viewModelType)
        {
            return _availableViewModelsByName.TryGetValue(name, out viewModelType);
        }

        public bool TryLookupByFullName(string name, out Type? viewModelType)
        {
            return _availableViewModelsByFullName.TryGetValue(name, out viewModelType);
        }

        public void Add(Type viewModelType)
        {
            _availableViewModelsByName.TryAdd(viewModelType.Name, viewModelType);
            if (viewModelType.FullName != null)
                _availableViewModelsByFullName.TryAdd(viewModelType.FullName, viewModelType);
        }

        public void Add<TViewModel>() where TViewModel : IMvxViewModel
        {
            Add(typeof(TViewModel));
        }

        [RequiresUnreferencedCode("This method registers view models that may not be preserved by trimming")]
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
