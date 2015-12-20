// MvxViewControllerAdaptingExtensions.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Touch.Views
{
    using MvvmCross.Platform.Touch.Views;

    public static class MvxViewControllerAdaptingExtensions
    {
        public static void AdaptForBinding(this IMvxEventSourceViewController view)
        {
            var adapter = new MvxViewControllerAdapter(view);
            var binding = new MvxBindingViewControllerAdapter(view);
        }
    }
}