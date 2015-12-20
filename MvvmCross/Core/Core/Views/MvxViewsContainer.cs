// MvxViewsContainer.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.Views
{
    using System;
    using System.Collections.Generic;

    using MvvmCross.Core.ViewModels;

    public abstract class MvxViewsContainer
        : IMvxViewsContainer
    {
        private readonly Dictionary<Type, Type> _bindingMap = new Dictionary<Type, Type>();
        private readonly List<IMvxViewFinder> _secondaryViewFinders;
        private IMvxViewFinder _lastResortViewFinder;

        protected MvxViewsContainer()
        {
            this._secondaryViewFinders = new List<IMvxViewFinder>();
        }

        public void AddAll(IDictionary<Type, Type> lookup)
        {
            foreach (var pair in lookup)
            {
                this.Add(pair.Key, pair.Value);
            }
        }

        public void Add(Type viewModelType, Type viewType)
        {
            this._bindingMap[viewModelType] = viewType;
        }

        public void Add<TViewModel, TView>()
            where TViewModel : IMvxViewModel
            where TView : IMvxView
        {
            this.Add(typeof(TViewModel), typeof(TView));
        }

        public Type GetViewType(Type viewModelType)
        {
            Type binding;
            if (this._bindingMap.TryGetValue(viewModelType, out binding))
            {
                return binding;
            }

            foreach (var viewFinder in this._secondaryViewFinders)
            {
                binding = viewFinder.GetViewType(viewModelType);
                if (binding != null)
                {
                    return binding;
                }
            }

            if (this._lastResortViewFinder != null)
            {
                binding = this._lastResortViewFinder.GetViewType(viewModelType);
                if (binding != null)
                {
                    return binding;
                }
            }

            throw new KeyNotFoundException("Could not find view for " + viewModelType);
        }

        public void AddSecondary(IMvxViewFinder finder)
        {
            this._secondaryViewFinders.Add(finder);
        }

        public void SetLastResort(IMvxViewFinder finder)
        {
            this._lastResortViewFinder = finder;
        }
    }
}