// <copyright file="MvxTouchViewDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

namespace MvvmCross.Mac.Views
{
    using System;

    using global::MvvmCross.Core.ViewModels;
    using global::MvvmCross.Core.Views;
    using global::MvvmCross.Platform.Platform;

    using MvvmCross.Mac.Views.Presenters;

    public class MvxMacViewDispatcher
        : MvxMacUIThreadDispatcher
        , IMvxViewDispatcher
    {
        private readonly IMvxMacViewPresenter _presenter;

        public MvxMacViewDispatcher(IMvxMacViewPresenter presenter)
        {
            this._presenter = presenter;
        }

        public bool ShowViewModel(MvxViewModelRequest request)
        {
            Action action = () =>
            {
                MvxTrace.TaggedTrace("MacNavigation", "Navigate requested");
                this._presenter.Show(request);
            };
            return this.RequestMainThreadAction(action);
        }

        public bool ChangePresentation(MvxPresentationHint hint)
        {
            Action action = () =>
                                {
                                    MvxTrace.TaggedTrace("MacNavigation", "Change presentation requested");
                                    this._presenter.ChangePresentation(hint);
                                };
            return this.RequestMainThreadAction(action);
        }
    }
}