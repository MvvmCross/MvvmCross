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
using Cirrious.MvvmCross.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModel;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Cirrious.MvvmCross.Touch.Views
{
    public class MvxTouchDialogViewController<TViewModel>
        : DialogViewController
          , IMvxTouchView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        protected MvxTouchDialogViewController(UITableViewStyle style, RootElement root, bool pushing)
            : this(style, root, pushing, MvxTouchViewRole.TopLevelView)
        {
        }

        protected MvxTouchDialogViewController(UITableViewStyle style, RootElement root, bool pushing, MvxTouchViewRole role)
			: base(style, root, pushing)
        {
            _role = role;
        }

        private readonly MvxTouchViewRole _role;

        public MvxTouchViewRole Role
        {
            get { return _role; }
        }

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
            get { return typeof (TViewModel); }
        }

        public void SetViewModel(object viewModel)
        {
            ViewModel = (TViewModel) viewModel;
        }

        protected virtual void OnViewModelChanged() {}
    }
}