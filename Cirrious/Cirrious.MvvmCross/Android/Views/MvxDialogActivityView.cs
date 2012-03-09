#region Copyright
// <copyright file="MvxDialogActivityView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
namespace Cirrious.MvvmCross.Android.Views
{
#warning Remove this - too hard to keep multiple activity classes live
#if false
    public abstract class MvxDialogActivityView<TViewModel>
        : DialogActivity
        , IMvxAndroidView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        protected MvxDialogActivityView()
        {
            IsVisible = true;
        }

        #region Common code across all android views - one case for multiple inheritance?

        public Type ViewModelType
        {
            get { return typeof(TViewModel); }
        }

        public bool IsVisible { get; private set; }

        private TViewModel _viewModel;

        public TViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                OnViewModelChanged();
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            this.OnViewCreate();
            base.OnCreate(bundle);
        }

        protected override void OnDestroy()
        {
            this.OnViewDestroy();
            base.OnDestroy();
        }

        protected abstract void OnViewModelChanged();

        protected override void OnResume()
        {
            this.OnViewResume();
            IsVisible = true;
            base.OnResume();
        }

        protected override void OnPause()
        {
            this.OnViewPause();
            IsVisible = false;
            base.OnPause();
        }

        protected override void OnStart()
        {
            this.OnViewStart();
            base.OnStart();
        }

        protected override void OnRestart()
        {
            this.OnViewRestart();
            base.OnRestart();
        }

        protected override void OnStop()
        {
            this.OnViewStop();
            base.OnStop();
        }

        #endregion
    }
#endif

}