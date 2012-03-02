#region Copyright
// <copyright file="IMvxView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Interfaces.Views
{
    public interface IMvxView
    {
        bool IsVisible { get; }
    }

    public interface IMvxView<TViewModel>
        : IMvxView
        where TViewModel : class, IMvxViewModel
    {
        TViewModel ViewModel { get; set; }
    }
}