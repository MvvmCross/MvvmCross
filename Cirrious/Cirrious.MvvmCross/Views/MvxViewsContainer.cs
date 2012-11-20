#region Copyright
// <copyright file="MvxViewsContainer.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Views
{
    public abstract class MvxViewsContainer
        : IMvxViewsContainer
    {
        private readonly Dictionary<Type, Type> _bindingMap = new Dictionary<Type, Type>();
        private readonly List<IMvxViewFinder> _secondaryViewFinders;
        private IMvxViewFinder _lastResortViewFinder;

        protected MvxViewsContainer()
        {
            _secondaryViewFinders = new List<IMvxViewFinder>();
        }

        #region IMvxViewsContainer Members

        public void Add(Type viewModelType, Type viewType)
        {
            _bindingMap[viewModelType] = viewType;
        }

        public void Add<TViewModel>(Type viewType) where TViewModel : IMvxViewModel
        {
            Add(typeof (TViewModel), viewType);
        }

        public Type GetViewType(Type viewModelType)
        {
            Type binding;
            if (_bindingMap.TryGetValue(viewModelType, out binding))
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

        #endregion
    }
}