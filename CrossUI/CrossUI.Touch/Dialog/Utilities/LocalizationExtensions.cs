// LocalizationExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Foundation;
using System;

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