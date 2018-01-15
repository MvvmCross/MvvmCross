using System;

using Foundation;
using UIKit;

namespace MvvmCross.tvOS.Views
{
    public interface IMvxTvosViewSegue
    {
        object PrepareViewModelParametersForSegue(UIStoryboardSegue segue, NSObject sender);
    }
}
