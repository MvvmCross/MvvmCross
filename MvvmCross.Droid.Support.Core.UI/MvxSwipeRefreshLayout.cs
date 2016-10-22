// MvxSwipeRefreshLayout.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Util;

namespace MvvmCross.Droid.Support.V4 
{
    [Register("mvvmcross.droid.support.v4.MvxSwipeRefreshLayout")]
    public class MvxSwipeRefreshLayout : SwipeRefreshLayout 
    {	 
        protected MvxSwipeRefreshLayout(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer) { }

        public MvxSwipeRefreshLayout(Context context)
            : this (context, null) { }

        public MvxSwipeRefreshLayout(Context context, IAttributeSet attributes)
            : base (context, attributes) { }
    
        private ICommand _refreshCommand;
        private bool _refreshOverloaded;

        public ICommand RefreshCommand
        {
            get { return _refreshCommand; }
            set
            {
                _refreshCommand = value;
                if (_refreshCommand != null)
                    EnsureRefreshCommandOverloaded();
            }		
        }

        private void EnsureRefreshCommandOverloaded()
        {
            if (_refreshOverloaded)
                return;

            _refreshOverloaded = true;
            Refresh += OnRefresh;
        }

        protected virtual void ExecuteRefreshCommand(ICommand command)
        {
            if (command == null)
                return;

            if (!command.CanExecute(null))
                return;

            command.Execute(null);
        }

        private void OnRefresh(object sender, EventArgs args)
        {
            this.ExecuteRefreshCommand(this.RefreshCommand);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Refresh -= this.OnRefresh;
            }

            base.Dispose(disposing);
        }
    }
}
