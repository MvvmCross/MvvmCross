// MvxTouchTableViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Touch.Views
{
	public class MvxTableViewController
		: EventSourceTableViewController
		, IMvxTouchView
	{
		protected MvxTableViewController (UITableViewStyle style = UITableViewStyle.Plain)
			: base(style)
		{
			var adapter = new MvxViewControllerAdapter(this);
		}

		public IMvxViewModel ViewModel { get;set; }
		
		public bool IsVisible
		{
			get { return this.IsVisible(); }
		}
		
		public MvxShowViewModelRequest ShowRequest { get; set; }
	}

    public class MvxTouchTableViewController<TViewModel>
        : UITableViewController
          , IMvxTouchView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        protected MvxTouchTableViewController(MvxShowViewModelRequest request)
        {
            ShowRequest = request;
        }

		protected MvxTouchTableViewController(MvxShowViewModelRequest request, UITableViewStyle style = UITableViewStyle.Plain)
			: base(style)
		{
			ShowRequest = request;
		}

        #region Shared code across all Touch ViewControllers

		private IMvxViewModel _viewModel;
		
		public Type ViewModelType
		{
			get { return typeof (TViewModel); }
		}
		
		public TViewModel ViewModel
		{
			get { return (TViewModel)((IMvxView)this).ViewModel; }
			set
			{
				((IMvxView)this).ViewModel = value;
			}
		}
		
		IMvxViewModel IMvxView.ViewModel
		{
			get { return _viewModel; }
			set
			{
				_viewModel = (TViewModel)value;
				OnViewModelChanged();
			}
		}
		
		public bool IsVisible
		{
			get { return this.IsVisible(); }
		}		

        public MvxShowViewModelRequest ShowRequest { get; set; }

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