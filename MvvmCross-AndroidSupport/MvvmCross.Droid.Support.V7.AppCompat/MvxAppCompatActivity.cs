// MvxAppCompatActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using Android.OS;
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
        : MvxEventSourceAppCompatActivity, IMvxAndroidView, ViewTreeObserver.IOnGlobalLayoutListener 
    {
        private View _view;

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

			_view.ViewTreeObserver.AddOnGlobalLayoutListener(this);

			SetContentView(_view);
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

		public override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();
			ViewModel?.Appearing();
		}

		public override void OnDetachedFromWindow()
		{
            base.OnDetachedFromWindow();
            ViewModel?.Disappearing(); // we don't have anywhere to get this info
            ViewModel?.Disappeared();
		}

        public override View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
        {
            var view = MvxAppCompatActivityHelper.OnCreateView(parent, name, context, attrs);
            return view ?? base.OnCreateView(parent, name, context, attrs);
        }

        public void OnGlobalLayout()
        {
            if (_view != null)
            {
                if (_view.ViewTreeObserver.IsAlive)
                {
                    if (Build.VERSION.SdkInt < BuildVersionCodes.JellyBean)
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        _view.ViewTreeObserver.RemoveGlobalOnLayoutListener(this);
#pragma warning restore CS0618 // Type or member is obsolete
                    }
                    else
                    {
                        _view.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
                    }
                }
                _view = null;
                ViewModel?.Appeared();
            }
        }
    }

    public abstract class MvxAppCompatActivity<TViewModel>
        : MvxAppCompatActivity, IMvxAndroidView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        protected MvxAppCompatActivity(IntPtr ptr, JniHandleOwnership ownership) 
            : base(ptr, ownership)
        {
        }

        protected MvxAppCompatActivity()
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}