// MvxTabActivity.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
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
        : MvxEventSourceTabActivity
            , IMvxAndroidView
            , IMvxChildViewModelOwner
    {
        protected MvxTabActivity()
        {
            BindingContext = new MvxAndroidBindingContext(this, this);
            this.AddEventListeners();
        }

        public object DataContext
        {
            get => BindingContext.DataContext;
            set => BindingContext.DataContext = value;
        }

        public IMvxViewModel ViewModel
        {
            get => DataContext as IMvxViewModel;
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

        public IMvxBindingContext BindingContext { get; set; }

        public List<int> OwnedSubViewModelIndicies { get; } = new List<int>();

        protected virtual void OnViewModelSet()
        {
        }

        public override void SetContentView(int layoutResId)
        {
            var view = this.BindingInflate(layoutResId, null);

            EventHandler onGlobalLayout = null;
            onGlobalLayout = (sender, args) =>
            {
                view.ViewTreeObserver.GlobalLayout -= onGlobalLayout;
                ViewModel.Appeared();
            };

            view.ViewTreeObserver.GlobalLayout += onGlobalLayout;

            SetContentView(view);
        }

        protected override void AttachBaseContext(Context @base)
        {
            base.AttachBaseContext(MvxContextWrapper.Wrap(@base, this));
        }

        public override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            ViewModel.Appearing();
        }

        public override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            ViewModel.Disappearing(); // we don't have anywhere to get this info
            ViewModel.Disappeared();
        }
    }

    [Obsolete("TabActivity is obsolete. Use ViewPager + Indicator or any other Activity with Toolbar support.")]
    public class MvxTabActivity<TViewModel>
        : MvxTabActivity
            , IMvxAndroidView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get => (TViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}