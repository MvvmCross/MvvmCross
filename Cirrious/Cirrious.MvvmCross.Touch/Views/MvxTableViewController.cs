// MvxTouchTableViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Touch.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Views;
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
		, IMvxBindingTouchView
	{
		protected MvxTableViewController (UITableViewStyle style = UITableViewStyle.Plain)
			: base(style)
		{
            this.AdaptForBinding();
        }

		public virtual object DataContext { get;set; }
		
		public IMvxViewModel ViewModel
		{
			get { return (IMvxViewModel)DataContext; }
			set { DataContext = value; }
		}

		public bool IsVisible
		{
			get { return this.IsVisible(); }
		}
		
		public MvxShowViewModelRequest ShowRequest { get; set; }

        private readonly List<IMvxUpdateableBinding> _bindings = new List<IMvxUpdateableBinding>();

        public List<IMvxUpdateableBinding> Bindings
        {
            get { return _bindings; }
        }
    }
}