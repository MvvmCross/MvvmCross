// MvxTabActivity.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Runtime;

namespace MvvmCross.Droid.Views
{
    using System.Collections.Generic;

    using Android.Content;
    using Android.OS;
    using Android.Views;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Droid.BindingContext;
    using MvvmCross.Binding.Droid.Views;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Platform.Droid.Views;

    [Obsolete("TabActivity is obsolete. Use ViewPager + Indicator or any other Activity with Toolbar support.")]
    [Register("mvvmcross.droid.views.MvxTabActivity")]
    public abstract class MvxTabActivity
        : MvxEventSourceTabActivity
          , IMvxAndroidView
          , IMvxChildViewModelOwner
          , ViewTreeObserver.IOnGlobalLayoutListener
    {
        private View _view;

        private readonly List<int> _ownedSubViewModelIndicies = new List<int>();

        public List<int> OwnedSubViewModelIndicies => this._ownedSubViewModelIndicies;

        protected MvxTabActivity()
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

        protected virtual void OnViewModelSet()
        {
        }

        public IMvxBindingContext BindingContext { get; set; }

        public override void SetContentView(int layoutResId)
        {
            _view = this.BindingInflate(layoutResId, null);

            _view.ViewTreeObserver.AddOnGlobalLayoutListener(this);

            this.SetContentView(_view);
        }

        protected override void AttachBaseContext(Context @base)
        {
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

        public void OnGlobalLayout()
        {
            if (_view != null)
            {
                if (Build.VERSION.SdkInt<BuildVersionCodes.JellyBean)
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    _view.ViewTreeObserver.RemoveGlobalOnLayoutListener(this);
#pragma warning restore CS0618 // Type or member is obsolete
                }
                else
                {
                    _view.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
                }
                ViewModel?.Appeared();
            }
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