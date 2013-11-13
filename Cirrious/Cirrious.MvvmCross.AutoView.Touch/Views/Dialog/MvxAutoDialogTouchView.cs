// MvxAutoDialogTouchView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.AutoView.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Touch.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Touch.Interfaces;
using Cirrious.MvvmCross.Binding.Bindings;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.ViewModels;
using CrossUI.Core.Elements.Menu;
using CrossUI.Touch.Dialog.Elements;
using MonoTouch.UIKit;
using Cirrious.CrossCore.IoC;

namespace Cirrious.MvvmCross.AutoView.Touch.Views.Dialog
{
    [MvxUnconventional]
    public class MvxAutoDialogTouchView
        : MvxDialogViewController
          , IMvxTouchAutoView
    {

        private IParentMenu _parentMenu;

        public MvxAutoDialogTouchView()
            : base(UITableViewStyle.Grouped, null, true)
        {
        }

        public MvxAutoDialogTouchView(IntPtr handle) 
            : base(handle)
        {
            Mvx.Warning("MvxAutoDialogTouchView IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
        }

        public new MvxViewModel ViewModel
        {
			get { return base.ViewModel as MvxViewModel; }
            set { base.ViewModel = value; }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Root = this.LoadDialogRoot<Element, RootElement>();
            _parentMenu = this.LoadMenu();

            if (_parentMenu != null)
            {
                NavigationItem.SetRightBarButtonItem(
                    new UIBarButtonItem(UIBarButtonSystemItem.Action,
                                        (sender, e) => { ShowActionMenu(); }),
                    false);
            }
        }

        private void ShowActionMenu()
        {
            this.ShowOptionsMenu(_parentMenu);
        }

        public void RegisterBinding(IMvxUpdateableBinding binding)
        {
            BindingContext.RegisterBinding(binding);
        }
    }
}