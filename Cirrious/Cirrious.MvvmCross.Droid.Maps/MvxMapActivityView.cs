// MvxMapActivityView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.GoogleMaps;

namespace Cirrious.MvvmCross.Droid.Maps
{
    public abstract class MvxMapActivityView<TViewModel> : MapActivity
    {
        protected MvxMapActivityView()
        {
            IsVisible = true;
        }
    }
}