// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MvvmCross.ViewModels;

namespace MvvmCross.Views
{
#nullable enable
    public abstract class MvxViewsContainer
        : IMvxViewsContainer
    {
        private readonly Dictionary<Type, Type> _bindingMap = new Dictionary<Type, Type>();
        private readonly List<IMvxViewFinder> _secondaryViewFinders;
        private IMvxViewFinder? _lastResortViewFinder;

        protected MvxViewsContainer()
        {
            _secondaryViewFinders = new List<IMvxViewFinder>();
        }

        public void AddAll(IDictionary<Type, Type> viewModelViewLookup)
        {
            foreach (var pair in viewModelViewLookup)
            {
                Add(pair.Key, pair.Value);
            }
        }

        public void Add(Type viewModelType, Type viewType)
        {
            _bindingMap[viewModelType] = viewType;
        }

        public void Add<TViewModel, TView>()
            where TViewModel : IMvxViewModel
            where TView : IMvxView
        {
            Add(typeof(TViewModel), typeof(TView));
        }

        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.Interfaces)]
        public Type GetViewType(Type? viewModelType)
        {
            Type? binding;
            if (viewModelType != null && _bindingMap.TryGetValue(viewModelType, out binding))
            {
                return binding;
            }

            foreach (var viewFinder in _secondaryViewFinders)
            {
                binding = viewFinder.GetViewType(viewModelType);
                if (binding != null)
                {
                    return binding;
                }
            }

            if (_lastResortViewFinder != null)
            {
                binding = _lastResortViewFinder.GetViewType(viewModelType);
                if (binding != null)
                {
                    return binding;
                }
            }

            throw new KeyNotFoundException("Could not find view for " + viewModelType);
        }

        public void AddSecondary(IMvxViewFinder finder)
        {
            _secondaryViewFinders.Add(finder);
        }

        public void SetLastResort(IMvxViewFinder finder)
        {
            _lastResortViewFinder = finder;
        }
    }
#nullable restore
}
