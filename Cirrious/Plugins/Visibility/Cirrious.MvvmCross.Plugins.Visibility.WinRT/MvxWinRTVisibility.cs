#region Copyright

// <copyright file="MvxWinRTVisibility.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

namespace Cirrious.MvvmCross.Plugins.Visibility.WinRT
{
    public class MvxWinRTVisibility : IMvxNativeVisibility
    {
        #region Implementation of IMvxNativeVisibility

        public object ToNative(MvxVisibility visibility)
        {
            return visibility == MvxVisibility.Visible
                       ? Windows.UI.Xaml.Visibility.Visible
                       : Windows.UI.Xaml.Visibility.Collapsed;
        }

        #endregion
    }
}