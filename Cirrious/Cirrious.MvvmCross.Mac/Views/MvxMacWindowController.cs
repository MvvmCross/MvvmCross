// MvxMacViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.ViewModels;

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

        public MvxShowViewModelRequest ShowRequest { get; set; }

        protected virtual void OnViewModelChanged()
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

#warning Not sure about positioning of Create/Destory here...
            this.OnViewCreate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
#warning Not sure about positioning of Create/Destory here...
#warning NEED TO COMMENT BACK IN THIS HACK SOMEHOW!
                //this.OnViewDestroy();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}