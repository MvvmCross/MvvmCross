#region Copyright

// <copyright file="IMvxTrackedView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Interfaces.Views
{
    public interface IMvxTrackedView
        : IMvxView
    {
        bool IsVisible { get; }
    }

    public interface IMvxTrackedView<TViewModel>
        : IMvxTrackedView
        , IMvxView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
    }
}