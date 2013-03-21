// MvxConsoleDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Console.Views
{
    public class MvxConsoleDispatcher
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
            return RequestMainThreadAction(() => navigation.Navigate(request));
        }

        public bool RequestClose(IMvxViewModel toClose)
        {
            var navigation = Mvx.Resolve<IMvxConsoleNavigation>();
            return RequestMainThreadAction(navigation.GoBack);
        }

        public bool RequestRemoveBackStep()
        {
            var navigation = Mvx.Resolve<IMvxConsoleNavigation>();
            return RequestMainThreadAction(navigation.RemoveBackEntry);
        }
    }
}