﻿using System;
using MvvmCross.Platforms.Tvos.Presenters.Attributes;
using MvvmCross.Platforms.Tvos.Views;
using Playground.Core.ViewModels;

namespace Playground.TvOS
{
    [MvxFromStoryboard("Main")]
    [MvxMasterDetailPresentation (Position = MasterDetailPosition.Master)]
	public partial class SplitMasterView : MvxViewController<SplitMasterViewModel> 
	{
		public SplitMasterView (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var bindingSet = CreateBindingSet();
            bindingSet.Bind(btnDetail).To(vm => vm.OpenDetailCommand);
            bindingSet.Bind(btnDetailNav).To(vm => vm.OpenDetailNavCommand);
            bindingSet.Bind(btnStackNav).To(vm => vm.ShowRootViewModel);
            bindingSet.Apply();
        }
	}
}
