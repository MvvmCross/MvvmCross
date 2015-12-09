// <copyright file="IMvxTouchNavigator.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

namespace MvvmCross.Mac.Views
{
    using global::MvvmCross.Core.ViewModels;

    public interface IMvxMacNavigator
    {
        void NavigateTo(MvxViewModelRequest request);

        void ChangePresentation(MvxPresentationHint hint);
    }
}