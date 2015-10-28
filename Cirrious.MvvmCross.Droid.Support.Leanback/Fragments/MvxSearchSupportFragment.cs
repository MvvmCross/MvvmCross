using System;
using Android.OS;
using Android.Runtime;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Droid.Support.Fragging.Fragments;
using Cirrious.MvvmCross.Droid.Support.Leanback.Fragments.EventSource;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Droid.Support.Leanback.Fragments
{
	public class MvxSearchSupportFragment
		: MvxEventSourceSearchSupportFragment
			, IMvxFragmentView
	{
		/// <summary>
		/// Create new instance of a MvxSearchSupportFragment
		/// </summary>
		/// <param name="bundle">Usually this would be MvxViewModelRequest serialized</param>
		/// <returns>Returns an instance of a MvxFragment</returns>
		public static MvxSearchSupportFragment NewInstance(Bundle bundle)
		{
			// Setting Arguments needs to happen before Fragment is attached
			// to Activity. Arguments are persisted when Fragment is recreated!
			var fragment = new MvxSearchSupportFragment { Arguments = bundle};

			return fragment;
		}

		protected MvxSearchSupportFragment()
		{
			this.AddEventListeners();
		}

		protected MvxSearchSupportFragment(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
		{
			this.AddEventListeners();
		}

		public IMvxBindingContext BindingContext { get; set; }

		private object _dataContext;

		public object DataContext
		{
			get { return _dataContext; }
			set
			{
				_dataContext = value;
				if (BindingContext != null)
					BindingContext.DataContext = value;
			}
		}

		public virtual IMvxViewModel ViewModel
		{
			get { return DataContext as IMvxViewModel; }
			set
			{
				DataContext = value;
				OnViewModelSet();
			}
		}

		public virtual void OnViewModelSet()
		{
		}
	}
}