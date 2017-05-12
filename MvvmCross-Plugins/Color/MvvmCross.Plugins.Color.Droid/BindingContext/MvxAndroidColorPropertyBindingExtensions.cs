// MvxAndroidColorPropertyBindingExtensions.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Views;
using Android.Widget;
using MvvmCross.Plugins.Color.Droid.Binding;

namespace MvvmCross.Binding.BindingContext
{
    public static class MvxAndroidColorPropertyBindingExtensions
    {
        public static string BindBackgroundColor(this View view)
           => MvxAndroidColorPropertyBinding.View_BackgroundColor;

        public static string BindTextColor(this TextView view)
           => MvxAndroidColorPropertyBinding.TextView_TextColor;
    }
}
