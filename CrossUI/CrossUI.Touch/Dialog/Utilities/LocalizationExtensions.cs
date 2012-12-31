#region Copyright

// <copyright file="LocalizationExtensions.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using MonoTouch.Foundation;

namespace CrossUI.Touch.Dialog.Utilities
{
    internal static class LocalizationExtensions
    {
        /// <summary>
        /// Gets the localized text for the specified string.
        /// </summary>
        public static string GetText(this string text)
        {
            if (String.IsNullOrEmpty(text))
                return text;
            return NSBundle.MainBundle.LocalizedString(text, String.Empty, String.Empty);
        }
    }
}