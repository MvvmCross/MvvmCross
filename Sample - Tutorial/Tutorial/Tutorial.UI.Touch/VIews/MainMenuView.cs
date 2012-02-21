using System;
using Tutorial.Core.ViewModels;
using Cirrious.MvvmCross.Touch.Views;

namespace Tutorial.UI.Touch
{
	public class MainMenuView
		: MvxTouchTableViewController<MainMenuViewModel>
		, IMvxServiceConsumer<IMvxBinder>
	{
		private readonly List<IMvxUpdateableBinding> _bindings;
		
		public MainMenuView ()
		{
			_bindings = new List<IMvxUpdateableBinding>();
		}
		
		public override void Dispose (bool disposing)
		{
			if (disposing)
			{
			}
			
			base.Dispose(disposing);
		}
	}
}

