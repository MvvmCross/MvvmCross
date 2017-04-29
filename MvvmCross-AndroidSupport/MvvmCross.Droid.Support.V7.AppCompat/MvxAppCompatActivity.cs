// MvxAppCompatActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat.EventSource;
using MvvmCross.Droid.Views;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    [Register("mvvmcross.droid.support.v7.appcompat.MvxAppCompatActivity")]
    public class MvxAppCompatActivity
        : MvxEventSourceAppCompatActivity
            , IMvxAndroidView
    {
        protected MvxAppCompatActivity()
        {
            BindingContext = new MvxAndroidBindingContext(this, this);
            this.AddEventListeners();
        }

        protected MvxAppCompatActivity(IntPtr javaReference, JniHandleOwnership transfer)
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
            SetContentView(view);
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

        public override View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
        {
            var view = MvxAppCompatActivityHelper.OnCreateView(parent, name, context, attrs);
            return view ?? base.OnCreateView(parent, name, context, attrs);
        }
    }

    public abstract class MvxAppCompatActivity<TViewModel>
        : MvxAppCompatActivity
            , IMvxAndroidView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        protected MvxAppCompatActivity(IntPtr ptr, JniHandleOwnership ownership) : base(ptr, ownership)
        {
        }

        protected MvxAppCompatActivity()
        {
        }

        public new TViewModel ViewModel
        {
            get => (TViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}