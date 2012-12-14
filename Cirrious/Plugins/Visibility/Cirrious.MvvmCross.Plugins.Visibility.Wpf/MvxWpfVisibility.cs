#region Copyright
// <copyright file="MvxWindowsPhoneVisibility.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

namespace Cirrious.MvvmCross.Plugins.Visibility.Wpf
{
    public class MvxWpfVisibility : IMvxNativeVisibility
    {
        #region Implementation of IMvxNativeVisibility

        public object ToNative(MvxVisibility visibility)
        {
            return visibility == MvxVisibility.Visible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        #endregion
    }
}