// MvxConsoleViewDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using System;

namespace Cirrious.MvvmCross.Console.Views
{
    public class MvxConsoleViewDispatcher
        : MvxMainThreadDispatcher
        , IMvxViewDispatcher
    {
        public bool RequestMainThreadAction(Action action)
        {
            action();
            return true;
        }

        public bool ShowViewModel(MvxViewModelRequest request)
        {
            var navigation = Mvx.Resolve<IMvxConsoleNavigation>();
            return RequestMainThreadAction(() => navigation.Show(request));
        }

        public bool ChangePresentation(MvxPresentationHint hint)
        {
            var navigation = Mvx.Resolve<IMvxConsoleNavigation>();
            return RequestMainThreadAction(() => navigation.ChangePresentation(hint));
        }
    }
}