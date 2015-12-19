// MvxLinearDialogActivity.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Dialog.Droid.Views
{
    using Android.Content;

    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Droid.BindingContext;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Droid.Views;

    public abstract class MvxLinearDialogActivity
        : EventSourceLinearDialogActivity
          , IMvxDialogActivityView
    {
        protected MvxLinearDialogActivity()
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
            SetContentView(view);
        }

        protected virtual void OnViewModelSet()
        {
        }
    }
}