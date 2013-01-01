// IMvxViewsContainer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Interfaces.Views
{
    public interface IMvxViewsContainer : IMvxViewFinder
    {
        void Add(Type viewModelType, Type viewType);
        void Add<TViewModel>(Type viewType) where TViewModel : IMvxViewModel;
        void AddSecondary(IMvxViewFinder finder);
        void SetLastResort(IMvxViewFinder finder);
    }
}