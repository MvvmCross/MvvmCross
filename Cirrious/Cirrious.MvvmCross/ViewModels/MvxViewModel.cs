// MvxViewModel.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.Commands;
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

        public void ActOnRegisteredViews(Action<IMvxView> action)
        {
            lock (this)
            {
                foreach (var view in _views)
                {
                    action(view.Key);
                }
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
                    _closeCommandImpl = new MvxRelayCommand(DoClose, CanClose);
                }
                return _closeCommandImpl;
            }
        }

        public ICommand CloseCommand
        {
            get { return CloseCommandImpl; }
        }

        public virtual bool CanClose()
        {
            return true;
        }

        public virtual void DoClose()
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