// MvxConsoleDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region using

using System;
using Cirrious.MvvmCross.Console.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Views;

#endregion

namespace Cirrious.MvvmCross.Console.Views
{
    public class MvxConsoleDispatcher
        : IMvxViewDispatcher
          , IMvxServiceConsumer
    {
        #region IMvxViewDispatcher Members

        public bool RequestMainThreadAction(Action action)
        {
            return InvokeOrBeginInvoke(action);
        }

        public bool RequestNavigate(MvxShowViewModelRequest request)
        {
			var navigation = this.GetService<IMvxConsoleNavigation>();
            return InvokeOrBeginInvoke(() => navigation.Navigate(request));
        }

        public bool RequestClose(IMvxViewModel toClose)
        {
            var navigation = this.GetService<IMvxConsoleNavigation>();
            return InvokeOrBeginInvoke(navigation.GoBack);
        }

        public bool RequestRemoveBackStep()
        {
			var navigation = this.GetService<IMvxConsoleNavigation>();
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