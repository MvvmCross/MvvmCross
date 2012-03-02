#region Copyright
// <copyright file="IMvxViewsContainer.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Interfaces.Views
{
    public interface IMvxViewsContainer
    {
        void Add(Type viewModelType, Type viewType);
        void Add<TViewModel>(Type viewType) where TViewModel : IMvxViewModel;
        bool ContainsKey(Type viewModelType);
        Type GetViewType(Type viewModelType);
    }
}