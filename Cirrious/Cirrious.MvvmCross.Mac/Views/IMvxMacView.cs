#region Copyright

// <copyright file="IMvxTouchView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;

#endregion Copyright

using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Mac.Views
{
    public interface IMvxMacView
        : IMvxView
            , IMvxCanCreateMacView
            , IMvxBindingContextOwner
    {
        MvxViewModelRequest Request { get; set; }
    }
}