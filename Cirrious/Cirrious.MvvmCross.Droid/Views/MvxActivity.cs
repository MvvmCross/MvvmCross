// MvxActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Runtime;
using Cirrious.CrossCore.Droid.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.ViewModels;
using System;

namespace Cirrious.MvvmCross.Droid.Views
{
    public abstract class MvxActivity
        : MvxEventSourceActivity
        , IMvxAndroidView
    {

        protected MvxActivity(IntPtr javaReference, JniHandleOwnership transfer)
        {
            BindingContext = new MvxAndroidBindingContext(this, this);
            this.AddEventListeners();
        }

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
            SetContentView(view);
        }

        protected virtual void OnViewModelSet()
        {
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