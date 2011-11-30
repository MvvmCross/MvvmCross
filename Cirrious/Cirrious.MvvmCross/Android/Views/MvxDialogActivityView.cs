#region Copyright

// <copyright file="MvxDialogActivityView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using Android.OS;
using Cirrious.MvvmCross.Android.ExtensionMethods;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModel;
using MonoDroid.Dialog;

namespace Cirrious.MvvmCross.Android.Views
{
    public abstract class MvxDialogActivityView<TViewModel>
        : DialogActivity
          , IMvxAndroidView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        protected MvxDialogActivityView()
            : this(false)
        {
        }

        protected MvxDialogActivityView(bool isSubView)
        {
            _isSubView = isSubView;
        }

        private readonly bool _isSubView;

        public bool IsSubView
        {
            get { return _isSubView; }
        }

        private TViewModel _viewModel;

        public TViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                OnModelSet();
            }
        }

        public Type ViewModelType
        {
            get { return typeof (TViewModel); }
        }

        public void SetViewModel(object viewModel)
        {
            ViewModel = (TViewModel) viewModel;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.OnViewCreate();
        }

        protected abstract void OnModelSet();
    }
}