// IMvxViewsContainer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Views
{
    public interface IMvxViewsContainer : IMvxViewFinder
    {
        void AddAll(IDictionary<Type, Type> viewModelViewLookup);

        void Add(Type viewModelType, Type viewType);

        void Add<TViewModel, TView>()
            where TViewModel : IMvxViewModel
            where TView : IMvxView;

        void AddSecondary(IMvxViewFinder finder);

        void SetLastResort(IMvxViewFinder finder);
    }
}