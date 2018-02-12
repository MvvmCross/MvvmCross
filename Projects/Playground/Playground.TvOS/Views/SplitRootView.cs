﻿using System;
using MvvmCross.Platform.Tvos.Presenters.Attributes;
using MvvmCross.Platform.Tvos.Views;
using Playground.Core.ViewModels;

namespace Playground.TvOS
{
    [MvxFromStoryboard("Main")]
    [MvxMasterDetailPresentation(Position = MasterDetailPosition.Root)]
	public partial class SplitRootView : MvxSplitViewController<SplitRootViewModel> 
	{
        private bool _isFirstTime = true;

		public SplitRootView (IntPtr handle) : base (handle)
		{
		}

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (ViewModel != null && _isFirstTime)
            {
                _isFirstTime = false;
                ViewModel.ShowInitialMenuCommand.Execute(null);
            }
        }
	}
}
