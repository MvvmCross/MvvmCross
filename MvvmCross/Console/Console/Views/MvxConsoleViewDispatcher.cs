// MvxConsoleViewDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Console.Views
{
    using System;

    using MvvmCross.Platform;
    using MvvmCross.Platform.Core;

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
            return this.RequestMainThreadAction(() => navigation.Show(request));
        }

        public bool ChangePresentation(MvxPresentationHint hint)
        {
            var navigation = Mvx.Resolve<IMvxConsoleNavigation>();
            return this.RequestMainThreadAction(() => navigation.ChangePresentation(hint));
        }
    }
}