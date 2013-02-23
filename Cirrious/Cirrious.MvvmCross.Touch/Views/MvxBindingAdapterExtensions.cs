// MvxBindingAdapterExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Touch.Views;
using Cirrious.MvvmCross.Binding.Touch.Views;

namespace Cirrious.MvvmCross.Touch.Views
{
    public static class MvxBindingAdapterExtensions
    {
        public static void AdaptForBinding(this IMvxEventSourceViewController view)
        {
            var adapter = new MvxViewControllerAdapter(view);
            var binding = new MvxBindingViewControllerAdapter(view);
        }
    }
}