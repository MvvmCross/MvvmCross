// MvxViewControllerAdaptingExtensions.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.tvOS.Views
{
    using MvvmCross.Platform.tvOS.Views;

    public static class MvxViewControllerAdaptingExtensions
    {
        public static void AdaptForBinding(this IMvxEventSourceViewController view)
        {
            var adapter = new MvxViewControllerAdapter(view);
            var binding = new MvxBindingViewControllerAdapter(view);
        }
    }
}