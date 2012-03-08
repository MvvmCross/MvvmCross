#region Copyright
// <copyright file="MvxSimpleTouchDialogViewController.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
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
            get
            {
                return ViewModel;
            }
        }
    }
}