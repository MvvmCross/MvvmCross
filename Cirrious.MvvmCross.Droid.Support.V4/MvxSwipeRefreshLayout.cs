using System;
using System.Windows.Input;
using Android.Content;
using Android.Support.V4.Widget;
using Android.Util;

namespace Cirrious.MvvmCross.Droid.Support.V4 {
	public class MvxSwipeRefreshLayout : SwipeRefreshLayout {

		/*
		 * Credits for the implementation go out to James Montemagno
		 * http://motzcod.es/post/82102717747/xamarin-android-swiperefreshlayout-for-mvvmcross
		 */ 

		public MvxSwipeRefreshLayout ( Context p0 )
			: this ( p0, null ) {
			Init ();
		}

		public MvxSwipeRefreshLayout ( Context p0, IAttributeSet p1 )
			: base ( p0, p1 ) {
			Init ();
		}

		public ICommand RefreshCommand { get; set; }

		private void Init() { 
			//This gets called when we pull down to refresh to trigger command 
			this.Refresh += (object sender, EventArgs e ) => {
				var command = RefreshCommand;
				if ( command == null )
					return;
				command.Execute ( null );
			};
		}
	}
}

