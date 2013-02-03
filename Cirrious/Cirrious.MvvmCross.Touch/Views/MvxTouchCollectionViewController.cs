// MvxTouchCollectionViewController.cs
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
	public class MvxCollectionViewController 
		: EventSourceCollectionViewController
		, IMvxTouchView
	{
		protected MvxCollectionViewController(UICollectionViewLayout layout)
			: base(layout)
		{
			var adapter = new MvxViewControllerAdapter(this);	
		}
		
		private IMvxViewModel _viewModel;
		
		public IMvxViewModel ViewModel
		{
			get { return _viewModel; }
			set
			{
				_viewModel = value;
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
	}

	public class MvxTouchCollectionViewController<TViewModel>
		: UICollectionViewController
		, IMvxTouchView<TViewModel>
		where TViewModel : class, IMvxViewModel
	{
		protected MvxTouchCollectionViewController(MvxShowViewModelRequest request, UICollectionViewLayout layout)
			: base(layout)
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