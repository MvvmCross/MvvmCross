#region Copyright
// <copyright file="MvxTouchDialogViewController.cs" company="Cirrious">
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
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Touch.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Views;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Dialog
{
    public class MvxTouchDialogViewController<TViewModel>
        : DialogViewController
        , IMvxTouchView<TViewModel>
        , IMvxBindingTouchView
        where TViewModel : class, IMvxViewModel
    {
        protected MvxTouchDialogViewController(MvxShowViewModelRequest request, UITableViewStyle style, RootElement root, bool pushing)
            : base(style, root, pushing)
        {
            ShowRequest = request;
        }

        #region Shared code across all Touch ViewControllers

        private TViewModel _viewModel;

        public Type ViewModelType
        {
            get { return typeof(TViewModel); }
        }

        public bool IsVisible { get { return this.IsVisible(); } }

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

        protected virtual void OnViewModelChanged() { }

        public override void DismissViewController(bool animated, NSAction completionHandler)
        {
            base.DismissViewController(animated, completionHandler);
#warning Not sure about positioning of Create/Destory here...
            this.OnViewDestroy();
        }

        public override void ViewDidLoad()
        {
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

        #endregion

        #region Shared area needed by all binding controllers

        private readonly List<IMvxUpdateableBinding> _bindings = new List<IMvxUpdateableBinding>();
        public List<IMvxUpdateableBinding> Bindings
        {
            get { return _bindings; }
        }

        public object DefaultBindingSource { get { return ViewModel; } }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.ClearBindings();
            }

            base.Dispose(disposing);
        }

        public override void ViewDidUnload()
        {
            this.ClearBindings();
            base.ViewDidUnload();
        }

        #endregion
    }
}