#region Copyright

// <copyright file="MvxTouchDialogViewController.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Cirrious.MvvmCross.Touch.Dialog
{
    public class MvxTouchDialogViewController<TViewModel>
        : DialogViewController
        , IMvxTouchView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        private readonly List<IMvxBinding> _bindings = new List<IMvxBinding>();

        protected MvxTouchDialogViewController(MvxShowViewModelRequest request, UITableViewStyle style, RootElement root, bool pushing)
            : base(style, root, pushing)
        {
            ShowRequest = request;
        }

        public void AddBindings(IEnumerable<IMvxBinding> bindings)
        {
            _bindings.AddRange(bindings);
        }

        public void AddBinding(IMvxBinding binding)
        {
            _bindings.Add(binding);    
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
		
		public MvxTouchViewDisplayType DisplayType { get { return MvxTouchViewDisplayType.Master; } }

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
                // note that Dispose(true) should be called on the UI thread so we remain thread safe here
                _bindings.ForEach(x => x.Dispose());    
                _bindings.Clear();
            }
            base.Dispose(disposing);
        }

        public MvxShowViewModelRequest ShowRequest { get; private set; }

        #endregion
    }
}