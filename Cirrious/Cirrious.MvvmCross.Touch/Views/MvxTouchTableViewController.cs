#region Copyright

// <copyright file="MvxTouchTableViewController.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Views
{
    public class MvxTouchTableViewController<TViewModel>
        : UITableViewController
          , IMvxTouchView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        protected MvxTouchTableViewController(MvxShowViewModelRequest request)
        {
            ShowRequest = request;
        }

        #region Shared code across all Touch ViewControllers

        private TViewModel _viewModel;

        public Type ViewModelType
        {
            get { return typeof (TViewModel); }
        }

        public bool IsVisible
        {
            get { return this.IsVisible(); }
        }

        public TViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                OnViewModelChanged();
            }
        }

        public MvxShowViewModelRequest ShowRequest { get; private set; }

        protected virtual void OnViewModelChanged()
        {
        }

#warning really need to think about how to handle ios6 once ViewDidUnload has been removed
        [Obsolete]
        public override void ViewDidUnload()
        {
            this.OnViewDestroy();
            base.ViewDidUnload();
        }

        public override void ViewDidLoad()
        {
            this.OnViewCreate();
            base.ViewDidLoad();
        }

        #endregion
    }
}