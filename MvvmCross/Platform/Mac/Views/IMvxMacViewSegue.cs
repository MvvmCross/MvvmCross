using System;
using AppKit;
using Foundation;

namespace MvvmCross.Mac.Views
{
    public interface IMvxMacViewSegue
    {
        object PrepareViewModelParametersForSegue(NSStoryboardSegue segue, NSObject sender);
    }
}
