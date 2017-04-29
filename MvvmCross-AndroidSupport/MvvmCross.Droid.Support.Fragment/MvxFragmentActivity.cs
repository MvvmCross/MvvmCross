// MvxFragmentActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using Android.Runtime;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V4.EventSource;
using MvvmCross.Droid.Views;

namespace MvvmCross.Droid.Support.V4
{
    [Register("mvvmcross.droid.support.v4.MvxFragmentActivity")]
    public class MvxFragmentActivity
        : MvxEventSourceFragmentActivity, IMvxAndroidView
    {
        protected MvxFragmentActivity()
        {
            BindingContext = new MvxAndroidBindingContext(this, this);
            this.AddEventListeners();
        }

        protected MvxFragmentActivity(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
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

    public abstract class MvxFragmentActivity<TViewModel>
        : MvxFragmentActivity
            , IMvxAndroidView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get => (TViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}