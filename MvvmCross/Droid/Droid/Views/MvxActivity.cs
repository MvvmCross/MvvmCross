// MvxActivity.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Views
{
    using System;

    using Android.Content;
    using Android.Runtime;

    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Droid.BindingContext;
    using MvvmCross.Binding.Droid.Views;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Platform.Droid.Views;

    public abstract class MvxActivity
        : MvxEventSourceActivity
        , IMvxAndroidView
    {
        protected MvxActivity(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            this.BindingContext = new MvxAndroidBindingContext(this, this);
            this.AddEventListeners();
        }

        protected MvxActivity()
        {
            this.BindingContext = new MvxAndroidBindingContext(this, this);
            this.AddEventListeners();
        }

        public object DataContext
        {
            get { return this.BindingContext.DataContext; }
            set { this.BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get { return this.DataContext as IMvxViewModel; }
            set
            {
                this.DataContext = value;
                this.OnViewModelSet();
            }
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            base.StartActivityForResult(intent, requestCode);
        }

        public IMvxBindingContext BindingContext { get; set; }

        public override void SetContentView(int layoutResId)
        {
            var view = this.BindingInflate(layoutResId, null);
            this.SetContentView(view);
        }

        protected virtual void OnViewModelSet()
        {
        }

        protected override void AttachBaseContext(Context @base)
        {
            if (this is IMvxAndroidSplashScreenActivity)
            {
                // Do not attach our inflater to splash screens.
                base.AttachBaseContext(@base);
                return;
            }
            base.AttachBaseContext(MvxContextWrapper.Wrap(@base, this));
        }
    }

    public abstract class MvxActivity<TViewModel>
        : MvxActivity
        , IMvxAndroidView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}