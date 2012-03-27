#region Copyright
// <copyright file="MvxViewModel.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces.Commands;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxViewModel
        : MvxApplicationObject
        , IMvxViewModel
    {
        private readonly Dictionary<IMvxView, bool> _views = new Dictionary<IMvxView, bool>();

        #region Implementation of IMvxViewTracker

        public void RegisterView(IMvxView view)
        {
            lock (this)
            {
                _views[view] = true;
                SafeFireEvent(ViewRegistered);
            }
        }

        public void UnRegisterView(IMvxView view)
        {
            lock (this)
            {
                _views.Remove(view);
                SafeFireEvent(ViewUnRegistered);
            }
        }

        #endregion

        protected MvxViewModel()
        {
            RequestedBy = MvxRequestedBy.Unknown;
        }

        #region Back functionality - required for iOS which has no hardware back button

        private MvxRelayCommand _closeCommandImpl;
        protected MvxRelayCommand CloseCommandImpl
        {
            get
            {
                if (_closeCommandImpl == null)
                {
                    _closeCommandImpl = new MvxRelayCommand(Close, CanClose);
                }
                return _closeCommandImpl;
            }
        }

        public IMvxCommand CloseCommand
        {
            get { return CloseCommandImpl; }
        }

        protected virtual bool CanClose()
        {
            return true;
        }

        protected virtual void Close()
        {
            RequestClose(this);
        }

        #endregion

        protected bool HasViews
        {
            get
            {
                lock (this)
                {
                    return _views.Any();
                }
            }
        }

        protected bool IsVisible
        {
            get
            {
                lock (this)
                {
                    return _views.Keys.Any(view => view.IsVisible);
                }
            }
        }

        #region IMvxViewModel Members

        public MvxRequestedBy RequestedBy { get; set; }

        #endregion

        protected event EventHandler ViewRegistered;
        protected event EventHandler ViewUnRegistered;

        private void SafeFireEvent(EventHandler h)
        {
            var handler = h;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}