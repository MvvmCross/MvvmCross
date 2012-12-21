#region Copyright
// <copyright file="MvxTouchViewController.cs" company="Cirrious">
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
using MonoMac.AppKit;


namespace Cirrious.MvvmCross.Touch.Views
{
    public class MvxMacWindowController<TViewModel>
        : NSWindowController
        , IMvxMacView
        where TViewModel : class, IMvxViewModel
    {		
        protected MvxMacWindowController(MvxShowViewModelRequest request)
        {
            ShowRequest = request;
        }

        protected MvxMacWindowController(MvxShowViewModelRequest request, string nibName)
			: base(nibName)
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

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();

#warning Not sure about positioning of Create/Destory here...
			this.OnViewCreate();
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing) 
			{
#warning Not sure about positioning of Create/Destory here...
				this.OnViewDestroy();
			}
			base.Dispose (disposing);
		}
        #endregion
    }
}