#region Copyright
// <copyright file="MvxPssDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#region using

using System;
using Cirrious.MvvmCross.Pss.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Views;

#endregion

namespace Cirrious.MvvmCross.Pss.Views
{
    public class MvxPssDispatcher 
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
            var navigation = this.GetService<IMvxPssNavigation>();
            return InvokeOrBeginInvoke(() => navigation.Navigate(request));
        }

        public bool RequestClose(IMvxViewModel toClose)
        {
            var navigation = this.GetService<IMvxPssNavigation>();
            return InvokeOrBeginInvoke(navigation.GoBack);
        }

        public bool RequestRemoveBackStep()
        {
            var navigation = this.GetService<IMvxPssNavigation>();
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