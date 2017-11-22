// MvxTabActivity.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Droid.Views;

namespace MvvmCross.Droid.Views
{
    [Obsolete("TabActivity is obsolete. Use ViewPager + Indicator or any other Activity with Toolbar support.")]
    [Register("mvvmcross.droid.views.MvxTabActivity")]
    public abstract class MvxTabActivity
        : MvxEventSourceTabActivity, IMvxAndroidView, IMvxChildViewModelOwner
    {
        private View _view;

        private readonly List<int> _ownedSubViewModelIndicies = new List<int>();

        public List<int> OwnedSubViewModelIndicies => _ownedSubViewModelIndicies;

        protected MvxTabActivity()
        {
            BindingContext = new MvxAndroidBindingContext(this, this);
            this.AddEventListeners();
        }

        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get
            {
                return DataContext as IMvxViewModel;
            }
            set
            {
                DataContext = value;
                OnViewModelSet();
            }
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            StartActivityForResult(intent, requestCode);
        }

        protected virtual void OnViewModelSet()
        {
        }

        public IMvxBindingContext BindingContext { get; set; }

        public override void SetContentView(int layoutResId)
        {
            _view = this.BindingInflate(layoutResId, null);

            SetContentView(_view);
        }

        protected override void AttachBaseContext(Context @base)
        {
            base.AttachBaseContext(MvxContextWrapper.Wrap(@base, this));
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            ViewModel?.ViewCreated();
        }
		protected override void OnDestroy()
		{
			base.OnDestroy();
			ViewModel?.ViewDestroy();
		}

		protected override void OnStart()
		{
			base.OnStart();
			ViewModel?.ViewAppearing();
		}

		protected override void OnResume()
		{
			base.OnResume();
			ViewModel?.ViewAppeared();
		}

		protected override void OnPause()
		{
			base.OnPause();
			ViewModel?.ViewDisappearing();
		}

		protected override void OnStop()
		{
			base.OnStop();
			ViewModel?.ViewDisappeared();
		}
    }

    [Obsolete("TabActivity is obsolete. Use ViewPager + Indicator or any other Activity with Toolbar support.")]
    public class MvxTabActivity<TViewModel>
        : MvxTabActivity
        , IMvxAndroidView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}