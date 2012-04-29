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

using System.Windows;
using Cirrious.MvvmCross.Interfaces.Converters;

namespace Cirrious.MvvmCross.WindowsPhone.Platform.Converters
{
    public class MvxWindowsPhoneVisibility : IMvxNativeVisibility
    {
        #region Implementation of IMvxNativeVisibility

        public object ToNative(MvxVisibility visibility)
        {
            return visibility == MvxVisibility.Visible ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion
    }
}