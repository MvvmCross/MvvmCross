﻿// MvxActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Android.Content;
using Android.Runtime;
using System;
using System.Collections.Generic;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;
using MvvmCross.Platform.Droid.Views;

namespace MvvmCross.Droid.FullFragging.Views
{
    [Register("mvvmcross.droid.fullfragging.views.MvxActivity")]
    public abstract class MvxActivity
        : MvxEventSourceActivity
        , IMvxAndroidView
    {
        protected MvxActivity(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {}

        protected MvxActivity()
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
            get { return DataContext as IMvxViewModel; }
            set
            {
                DataContext = value;
                OnViewModelSet();
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

            EventHandler onGlobalLayout = null;
            onGlobalLayout = (sender, args) =>
            {
                view.ViewTreeObserver.GlobalLayout -= onGlobalLayout;
                ViewModel?.Appeared();
            };

            view.ViewTreeObserver.GlobalLayout += onGlobalLayout;

            SetContentView(view);
        }

        protected virtual void OnViewModelSet()
        {
        }

        protected override void AttachBaseContext(Context @base)
        {
            base.AttachBaseContext(MvxContextWrapper.Wrap(@base, this));
        }

        private readonly List<WeakReference<Fragment>> _fragList = new List<WeakReference<Fragment>>();

        public override void OnAttachFragment(Fragment fragment)
        {
            base.OnAttachFragment(fragment);
            _fragList.Add(new WeakReference<Fragment>(fragment));
        }

        public List<Fragment> Fragments
        {
            get
            {
                var ret = new List<Fragment>();
                foreach (var wref in _fragList)
                {
                    Fragment f;
                    if (!wref.TryGetTarget(out f)) continue;
                    if (f.IsVisible)
                        ret.Add(f);
                }

                return ret;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ViewModel?.Destroy();
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