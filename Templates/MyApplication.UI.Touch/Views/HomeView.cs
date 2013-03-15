

using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using MyApplication.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace MyApplication.UI.Touch
{
	public partial class HomeView : MvxViewController
	{
		public new HomeViewModel ViewModel 
		{
			get { return (HomeViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		public HomeView () : base ("HomeView", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			var tableSource = new MvxSimpleTableViewSource(ResultsTable, "SimpleItem");
			this.ResultsTable.Source = tableSource;

			this.AddBindings (
				new Dictionary<object,string> ()
			    {
					{ this.KeyTextField, "Text Key" },
		    		{ this.FetchButton, "TouchUpInside FetchItemsCommand" },
					{ tableSource, "ItemsSource Items" }
				});

			this.ResultsTable.ReloadData ();
		}
	}
}

