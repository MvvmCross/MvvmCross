﻿using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform.Tvos.Presenters.Attributes;
using MvvmCross.Platform.Tvos.Views;
using Playground.Core.ViewModels;

using UIKit;

namespace Playground.TvOS
{
    [MvxFromStoryboard("Main")]
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class ModalNavView : MvxViewController<ModalNavViewModel>
	{
		public ModalNavView (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Red;

            var set = this.CreateBindingSet<ModalNavView, ModalNavViewModel>();

            set.Bind(btnShowChild).To(vm => vm.ShowChildCommand);
            set.Bind(btnClose).To(vm => vm.CloseCommand);
            set.Bind(btnNestedModal).To(vm => vm.ShowNestedModalCommand);

            set.Apply();
        }
	}
}
