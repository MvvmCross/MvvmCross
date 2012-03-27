#region Copyright
// <copyright file="MvxTabActivityView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Cirrious.MvvmCross.Android.ExtensionMethods;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Android.Views
{
    public abstract class MvxTabActivityView<TViewModel>
        : TabActivity
        , IMvxAndroidView<TViewModel>
        , IMvxServiceConsumer<IMvxAndroidSubViewModelCache>
        where TViewModel : class, IMvxViewModel
    {
        private readonly List<int> _ownedSubViewModelIndicies = new List<int>();

        protected MvxTabActivityView()
        {
            IsVisible = true;
        }

        protected Intent CreateIntentFor<TTargetViewModel>(object parameterObject)
            where TTargetViewModel : class, IMvxViewModel
        {
            return CreateIntentFor<TTargetViewModel>(parameterObject.ToSimplePropertyDictionary());
        }

        protected Intent CreateIntentFor<TTargetViewModel>(IDictionary<string, string> parameterValues = null)
            where TTargetViewModel : class, IMvxViewModel
        {
            parameterValues = parameterValues ?? new Dictionary<string, string>();
            var request = new MvxShowViewModelRequest<TTargetViewModel>(parameterValues, false, MvxRequestedBy.UserAction);
            return CreateIntentFor(request);
        }

        protected Intent CreateIntentFor(MvxShowViewModelRequest request)
        {
            return this.GetService<IMvxAndroidViewModelRequestTranslator>().GetIntentFor(request);
        }

        protected Intent CreateIntentFor(IMvxViewModel subViewModel)
        {
            var intentWithKey = this.GetService<IMvxAndroidViewModelRequestTranslator>().GetIntentWithKeyFor(subViewModel);
            _ownedSubViewModelIndicies.Add(intentWithKey.Item2);
            return intentWithKey.Item1;
        }

        private void ClearOwnedSubIndicies()
        {
            var translator = this.GetService<IMvxAndroidViewModelRequestTranslator>();
            foreach (var ownedSubViewModelIndex in _ownedSubViewModelIndicies)
            {
                translator.RemoveSubViewModelWithKey(ownedSubViewModelIndex);
            }
            _ownedSubViewModelIndicies.Clear();
        }

        #region Common code across all android views - one case for multiple inheritance?

        private TViewModel _viewModel;

        public Type ViewModelType
        {
            get { return typeof(TViewModel); }
        }

        public bool IsVisible { get; private set; }

        public TViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                OnViewModelSet();
            }
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            base.StartActivityForResult(intent, requestCode);
        }

        public event EventHandler<MvxIntentResultEventArgs> MvxIntentResultReceived;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.OnViewCreate();
        }

        protected override void OnDestroy()
        {
            this.OnViewDestroy();
            base.OnDestroy();
            ClearOwnedSubIndicies();
        }

        protected abstract void OnViewModelSet();

        protected override void OnResume()
        {
            base.OnResume();
            IsVisible = true;
            this.OnViewResume();
        }

        protected override void OnPause()
        {
            this.OnViewPause();
            IsVisible = false;
            base.OnPause();
        }

        protected override void OnStart()
        {
            base.OnStart();
            this.OnViewStart();
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            this.OnViewRestart();
        }

        protected override void OnStop()
        {
            this.OnViewStop();
            base.OnStop();
        }

        public override void StartActivityForResult(Intent intent, int requestCode)
        {
            switch (requestCode)
            {
                case (int)MvxIntentRequestCode.PickFromFile:
                    MvxTrace.Trace("Warning - activity request code may clash with Mvx code for {0}", (MvxIntentRequestCode)requestCode);
                    break;
                default:
                    // ok...
                    break;
            }
            base.StartActivityForResult(intent, requestCode);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            var handler = MvxIntentResultReceived;
            if (handler != null)
                handler(this, new MvxIntentResultEventArgs(requestCode, resultCode, data));
            base.OnActivityResult(requestCode, resultCode, data);
        }

        #endregion
    }
}