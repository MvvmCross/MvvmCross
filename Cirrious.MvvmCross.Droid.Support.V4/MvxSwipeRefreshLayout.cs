using System;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Util;

namespace Cirrious.MvvmCross.Droid.Support.V4 
{
	[Register("cirrious.mvvmcross.droid.support.v4.MvxSwipeRefreshLayout")]
	public class MvxSwipeRefreshLayout : SwipeRefreshLayout 
	{	 
		protected MvxSwipeRefreshLayout(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer) { }

		public MvxSwipeRefreshLayout(Context p0)
			: this (p0, null) { }

		public MvxSwipeRefreshLayout(Context p0, IAttributeSet p1)
			: base (p0, p1) { }

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
            Refresh += (sender, args) => ExecuteRefreshCommand(RefreshCommand);
        }

        protected virtual void ExecuteRefreshCommand(ICommand command)
        {
            if (command == null)
                return;

            if (!command.CanExecute(null))
                return;

            command.Execute(null);
        }
	}
}
