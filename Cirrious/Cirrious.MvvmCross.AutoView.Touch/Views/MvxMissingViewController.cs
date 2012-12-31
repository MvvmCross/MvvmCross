#region Copyright

// <copyright file="MvxMissingViewController.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Cirrious.MvvmCross.AutoView.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Touch.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Touch.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Views.Attributes;

namespace Cirrious.MvvmCross.AutoView.Touch.Views
{
    [MvxUnconventionalView]
    public class MvxMissingViewController
        : MvxTouchDialogViewController<MvxViewModel>
          , IMvxTouchAutoView<MvxViewModel>
    {
        public MvxMissingViewController(MvxShowViewModelRequest request) : base(request)
        {
        }

        public void RegisterBinding(IMvxUpdateableBinding binding)
        {
            Bindings.Add(binding);
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var description = this.ViewModel.CreateMissingDialogDescription();
            var root = this.LoadDialogRoot(description);
            Root = root;
        }
    }
}