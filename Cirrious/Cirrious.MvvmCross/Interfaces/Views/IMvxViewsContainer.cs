#region Copyright
// <copyright file="IMvxViewsContainer.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.Interfaces.ViewModel;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Interfaces.Views
{
    public interface IMvxViewsContainer
    {
        void Add(MxvViewModelAction viewModelAction, Type viewType);
        void Add<TViewModel>(string actionName, Type viewType) where TViewModel : IMvxViewModel;
        void Add<TViewModel>(Type viewType) where TViewModel : IMvxViewModel;
        bool ContainsKey(MxvViewModelAction viewModelAction);
        Type GetViewType(MxvViewModelAction viewModelAction);
    }
}