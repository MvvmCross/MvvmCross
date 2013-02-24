// MvxConsoleDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region using

using System;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Console.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.ViewModels;

#endregion

namespace Cirrious.MvvmCross.Console.Views
{
    public class MvxConsoleDispatcher
        : IMvxViewDispatcher
    {
        #region IMvxViewDispatcher Members

        public bool RequestMainThreadAction(Action action)
        {
            return InvokeOrBeginInvoke(action);
        }

        public bool RequestNavigate(MvxShowViewModelRequest request)
        {
            var navigation = Mvx.Resolve<IMvxConsoleNavigation>();
            return InvokeOrBeginInvoke(() => navigation.Navigate(request));
        }

        public bool RequestClose(IMvxViewModel toClose)
        {
            var navigation = Mvx.Resolve<IMvxConsoleNavigation>();
            return InvokeOrBeginInvoke(navigation.GoBack);
        }

        public bool RequestRemoveBackStep()
        {
            var navigation = Mvx.Resolve<IMvxConsoleNavigation>();
            return InvokeOrBeginInvoke(navigation.RemoveBackEntry);
        }

        #endregion

        private bool InvokeOrBeginInvoke(Action action)
        {
            action();
            return true;
        }
    }
}