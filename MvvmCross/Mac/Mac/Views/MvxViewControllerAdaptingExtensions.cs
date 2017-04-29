// MvxViewControllerAdaptingExtensions.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.Mac.Views;

namespace MvvmCross.Mac.Views
{
    public static class MvxViewControllerAdaptingExtensions
    {
        public static void AdaptForBinding(this IMvxEventSourceViewController view)
        {
            new MvxViewControllerAdapter(view);
            new MvxBindingViewControllerAdapter(view);
        }
    }
}