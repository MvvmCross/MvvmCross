// MvxSimpleTouchDialogViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using CrossUI.Touch.Dialog.Elements;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.Simple
{
#warning The Simple code should go... replace with MvvmLight (and even lighter)
    public abstract class MvxSimpleTouchDialogViewController : MvxBindingDialogViewController
    {
        protected MvxSimpleTouchDialogViewController()
            : base(UITableViewStyle.Grouped, null, false)
        {
			ViewModel = new MvxNullViewModel();
		}

		public override object DataContext { get; set; }
    }
}