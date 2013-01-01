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
    public abstract class MvxSimpleTouchDialogViewController : MvxTouchDialogViewController<MvxNullViewModel>
    {
        protected MvxSimpleTouchDialogViewController()
            : this(UITableViewStyle.Grouped, null, false)
        {
        }

        protected MvxSimpleTouchDialogViewController(UITableViewStyle tableViewStyle, RootElement root, bool pushing)
            : base(MvxShowViewModelRequest<MvxNullViewModel>.GetDefaultRequest(), tableViewStyle, root, pushing)
        {
        }

        public new object ViewModel { get; set; }

        public override object DefaultBindingSource
        {
            get { return ViewModel; }
        }
    }
}