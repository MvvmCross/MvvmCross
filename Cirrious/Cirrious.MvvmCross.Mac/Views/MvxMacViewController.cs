#region Copyright
// <copyright file="MvxTouchViewController.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
using MonoMac.Foundation;


#endregion

using System;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Mac.ExtensionMethods;
using Cirrious.MvvmCross.Mac.Interfaces;
using Cirrious.MvvmCross.Views;
using MonoMac.AppKit;


namespace Cirrious.MvvmCross.Mac.Views
{
	public abstract class MvxMacViewController
		: NSViewController ,IMvxMacView
	{
		private IMvxViewModel _viewModel;

		#region Constructors
		
		// Called when created from unmanaged code
		public MvxMacViewController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MvxMacViewController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Call to load from the XIB/NIB file
		public MvxMacViewController (string viewName) : base (viewName, NSBundle.MainBundle)
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
		}
		
		#endregion

		#region IMvxMacView

		public bool IsVisible { get; set; }

		public void ClearBackStack()
		{
			throw new NotImplementedException();
			/*
            // note - we do *not* use CanGoBack here - as that seems to always returns true!
            while (NavigationService.BackStack.Any())
                NavigationService.RemoveBackEntry();
         */
		}
		
		public IMvxViewModel ViewModel
		{
			get { return _viewModel; }
			set
			{
				if (_viewModel == value)
					return;
				
				_viewModel = value;
			}
		}

		public MvxShowViewModelRequest ViewModelRequest
		{
			get; set;
		}

		#endregion

		public override void LoadView ()
		{
			base.LoadView ();
			this.OnViewCreate(ViewModelRequest);
		}

	}
}