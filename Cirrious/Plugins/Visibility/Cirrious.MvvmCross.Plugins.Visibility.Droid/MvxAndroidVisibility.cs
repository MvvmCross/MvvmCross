#region Copyright

// <copyright file="MvxAndroidVisibility.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Android.Views;

namespace Cirrious.MvvmCross.Plugins.Visibility.Droid
{
    public class MvxAndroidVisibility : IMvxNativeVisibility
    {
        #region Implementation of IMvxNativeVisibility

        public object ToNative(MvxVisibility visibility)
        {
            return visibility == MvxVisibility.Visible ? ViewStates.Visible : ViewStates.Gone;
        }

        #endregion
    }
}