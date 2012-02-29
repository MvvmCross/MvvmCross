#region Copyright

// <copyright file="MvxTouchTabBarViewController.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Views
{
    public class MvxTouchTabBarViewController<TViewModel>
        : UITabBarController
        , IMvxTouchView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        protected MvxTouchTabBarViewController(MvxShowViewModelRequest request)
        {
            ShowRequest = request;
        }

        #region Shared code across all Touch ViewControllers

        public bool IsVisible { get { return this.IsVisible(); } }

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

        public Type ViewModelType
        {
            get { return typeof(TViewModel); }
        }
		
        protected virtual void OnViewModelChanged() { }

        public override void DismissViewController(bool animated, MonoTouch.Foundation.NSAction completionHandler)
        {
            base.DismissViewController(animated, completionHandler);
#warning Not sure about positioning of Create/Destory here...
            this.OnViewDestroy();
        }

        public override void ViewDidLoad()
        {
#warning This ViewDidLoad code for tab bar is kludgey
            if (ShowRequest == null)
                return;
#warning Not sure about positioning of Create/Destory here...
            this.OnViewCreate();
            base.ViewDidLoad();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TODO - remove from list....
            }
            base.Dispose(disposing);
        }

        public MvxShowViewModelRequest ShowRequest { get; private set; }

        #endregion
    }
}