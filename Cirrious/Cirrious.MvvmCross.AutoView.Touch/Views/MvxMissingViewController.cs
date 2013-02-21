// MvxMissingViewController.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.AutoView.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Touch.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Touch.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Views.Attributes;
using CrossUI.Touch.Dialog.Elements;

namespace Cirrious.MvvmCross.AutoView.Touch.Views
{
    [MvxUnconventionalView]
    public class MvxMissingViewController
        : MvxDialogViewController
          , IMvxTouchAutoView
    {
        public MvxMissingViewController() : base()
        {
        }

        public new MvxViewModel ViewModel
        {
            get { return (MvxViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public void RegisterBinding(IMvxUpdateableBinding binding)
        {
            Bindings.Add(binding);
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var description = this.ViewModel.CreateMissingDialogDescription();
            var root = this.LoadDialogRoot<Element,RootElement>(description);
            Root = root;
        }
    }
}