using System;
using Android.OS;
using Android.Runtime;
using Cirrious.MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.Fragging.Fragments;
using MvvmCross.Droid.Support.Leanback.V17.Fragments.EventSource;
using Cirrious.MvvmCross.ViewModels;

namespace MvvmCross.Droid.Support.Leanback.V17.Fragments
{
    public class MvxHeadersSupportFragment
		: MvxEventSourceHeadersSupportFragment
			, IMvxFragmentView
	{
		/// <summary>
		/// Create new instance of a MvxHeadersSupportFragment
		/// </summary>
		/// <param name="bundle">Usually this would be MvxViewModelRequest serialized</param>
		/// <returns>Returns an instance of a MvxFragment</returns>
		public static MvxHeadersSupportFragment NewInstance(Bundle bundle)
		{
			// Setting Arguments needs to happen before Fragment is attached
			// to Activity. Arguments are persisted when Fragment is recreated!
			var fragment = new MvxHeadersSupportFragment { Arguments = bundle };

			return fragment;
		}

		protected MvxHeadersSupportFragment()
		{
			this.AddEventListeners();
		}

		protected MvxHeadersSupportFragment(IntPtr javaReference, JniHandleOwnership transfer)
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

        public string UniqueImmutableCacheTag => Tag;
    }

	public abstract class MvxHeadersSupportFragment<TViewModel>
		: MvxHeadersSupportFragment
			, IMvxFragmentView<TViewModel> where TViewModel : class, IMvxViewModel
	{

		protected MvxHeadersSupportFragment()
		{

		}

		protected MvxHeadersSupportFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}


		public new TViewModel ViewModel
		{
			get { return (TViewModel) base.ViewModel; }
			set { base.ViewModel = value; }
		}

    }
}