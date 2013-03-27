// MvxMissingViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.AutoView.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Touch.Interfaces;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using CrossUI.Touch.Dialog.Elements;
using Cirrious.CrossCore.IoC;

namespace Cirrious.MvvmCross.AutoView.Touch.Views
{
    [MvxUnconventional]
    public class MvxMissingViewController
        : MvxDialogViewController
          , IMvxTouchAutoView
    {
        public new MvxViewModel ViewModel
        {
            get { return (MvxViewModel) base.ViewModel; }
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