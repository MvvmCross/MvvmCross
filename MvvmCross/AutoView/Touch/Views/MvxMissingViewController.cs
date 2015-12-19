// MvxMissingViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Touch.Views
{
    using System;

    using CrossUI.Touch.Dialog.Elements;

    using MvvmCross.AutoView.ExtensionMethods;
    using MvvmCross.AutoView.Touch.Interfaces;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Dialog.Touch;
    using MvvmCross.Platform;
    using MvvmCross.Platform.IoC;

    [MvxUnconventional]
    public class MvxMissingViewController
        : MvxDialogViewController
          , IMvxTouchAutoView
    {
        public MvxMissingViewController()
        {
        }

        public MvxMissingViewController(IntPtr handle)
            : base(handle)
        {
            Mvx.Warning("MvxMissingViewController IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
        }

        public new MvxViewModel ViewModel
        {
            get { return base.ViewModel as MvxViewModel; }
            set { base.ViewModel = value; }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var description = this.ViewModel.CreateMissingDialogDescription();
            var root = this.LoadDialogRoot<Element, RootElement>(description);
            Root = root;
        }
    }
}