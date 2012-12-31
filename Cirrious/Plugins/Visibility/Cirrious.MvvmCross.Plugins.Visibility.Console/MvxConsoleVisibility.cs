#region Copyright

// <copyright file="MvxConsoleVisibility.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

namespace Cirrious.MvvmCross.Plugins.Visibility.Console
{
    public class MvxConsoleVisibility : IMvxNativeVisibility
    {
        #region Implementation of IMvxNativeVisibility

        public object ToNative(MvxVisibility visibility)
        {
            return visibility == MvxVisibility.Visible;
        }

        #endregion
    }
}