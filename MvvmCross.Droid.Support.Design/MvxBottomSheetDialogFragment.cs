// MvxBottomSheetDialogFragment.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.OS;
using Android.Runtime;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Shared.Fragments;
using MvvmCross.Droid.Support.Design.EventSource;
using MvvmCross.Droid.Support.V4;

namespace MvvmCross.Droid.Support.Design
{
    [Register("mvvmcross.droid.support.design.MvxBottomSheetDialogFragment")]
    public abstract class MvxBottomSheetDialogFragment
		: MvxEventSourceBottomSheetDialogFragment
		, IMvxFragmentView
	{
		protected MvxBottomSheetDialogFragment()
		{
			this.AddEventListeners();
		}

		protected MvxBottomSheetDialogFragment(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
		{ }

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
			set { DataContext = value; }
		}

		protected void EnsureBindingContextSet(Bundle b0)
		{
			this.EnsureBindingContextIsSet(b0);
		}

		public virtual string UniqueImmutableCacheTag => Tag;
	}

	public abstract class MvxBottomSheetDialogFragment<TViewModel>
		: MvxBottomSheetDialogFragment
		, IMvxFragmentView<TViewModel> where TViewModel : class, IMvxViewModel
	{
		public new TViewModel ViewModel
		{
			get { return (TViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}
	}
}
