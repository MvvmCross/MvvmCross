// MvxDroidVisibility.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Views;
using MvvmCross.Platform.UI;

namespace MvvmCross.Plugins.Visibility.Droid
{
    [Preserve(AllMembers = true)]
	public class MvxDroidVisibility : IMvxNativeVisibility
    {
        public object ToNative(MvxVisibility visibility)
        {
            return visibility == MvxVisibility.Visible ? ViewStates.Visible : ViewStates.Gone;
        }
    }
}