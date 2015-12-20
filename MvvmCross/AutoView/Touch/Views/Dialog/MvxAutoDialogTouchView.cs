// MvxAutoDialogTouchView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Touch.Views.Dialog
{
    using System;

    using CrossUI.Core.Elements.Menu;
    using CrossUI.Touch.Dialog.Elements;

    using MvvmCross.AutoView.ExtensionMethods;
    using MvvmCross.AutoView.Touch.ExtensionMethods;
    using MvvmCross.AutoView.Touch.Interfaces;
    using MvvmCross.Binding.Bindings;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Dialog.Touch;
    using MvvmCross.Platform;
    using MvvmCross.Platform.IoC;

    using UIKit;

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

        public new IMvxViewModel ViewModel
        {
            get { return base.ViewModel as IMvxViewModel; }
            set { base.ViewModel = value; }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Root = this.LoadDialogRoot<Element, RootElement>();
            this._parentMenu = this.LoadMenu();

            if (this._parentMenu != null)
            {
                NavigationItem.SetRightBarButtonItem(
                    new UIBarButtonItem(UIBarButtonSystemItem.Action,
                                        (sender, e) => { this.ShowActionMenu(); }),
                    false);
            }
        }

        private void ShowActionMenu()
        {
            this.ShowOptionsMenu(this._parentMenu);
        }

        public void RegisterBinding(object target, IMvxUpdateableBinding binding)
        {
            BindingContext.RegisterBinding(target, binding);
        }
    }

    [MvxUnconventional]
    public class MvxAutoDialogTouchView<TViewModel>
        : MvxAutoDialogTouchView
          , IMvxTouchAutoView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}