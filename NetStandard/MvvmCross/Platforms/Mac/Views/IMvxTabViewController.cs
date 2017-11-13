using System;
using AppKit;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Mac.Views
{
    public interface IMvxTabViewController
    {
        void ShowTabView(NSViewController viewController, string tabTitle);

        bool CloseTabView(IMvxViewModel viewModel);
    }
}
