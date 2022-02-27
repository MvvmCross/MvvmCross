// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Util;

namespace MvvmCross.DroidX
{
    [Register("mvvmcross.droidx.MvxSwipeRefreshLayout")]
    public class MvxSwipeRefreshLayout : AndroidX.SwipeRefreshLayout.Widget.SwipeRefreshLayout
    {
        protected MvxSwipeRefreshLayout(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public MvxSwipeRefreshLayout(Context context)
            : this(context, null)
        {
        }

        public MvxSwipeRefreshLayout(Context context, IAttributeSet attributes)
            : base(context, attributes)
        {
        }

        private ICommand _refreshCommand;
        private bool _refreshOverloaded;

        public ICommand RefreshCommand
        {
            get
            {
                return _refreshCommand;
            }
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
            ExecuteRefreshCommand(RefreshCommand);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Refresh -= OnRefresh;
            }

            base.Dispose(disposing);
        }
    }
}
