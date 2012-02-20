#region Copyright

// <copyright file="MvxViewModel.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxViewModel
        : MvxApplicationObject
        , IMvxViewModel
    {
        protected MvxViewModel()
        {
            RequestedBy = MvxRequestedBy.Unknown;
        }

        public MvxRequestedBy RequestedBy { get; set; }
        private readonly Dictionary<IMvxTrackedView, bool> _views = new Dictionary<IMvxTrackedView, bool>();

        #region Implementation of IMvxViewTracker

        public void RegisterView(IMvxTrackedView view)
        {
            lock (this)
            {
                _views[view] = true;
                SafeFireEvent(ViewRegistered);
            }
        }

        public void UnRegisterView(IMvxTrackedView view)
        {
            lock (this)
            {
                _views.Remove(view);
                SafeFireEvent(ViewUnRegistered);
            }
        }

        #endregion

        protected event EventHandler ViewRegistered;
        protected event EventHandler ViewUnRegistered;

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

        private void SafeFireEvent(EventHandler h)
        {
            var handler = h;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}