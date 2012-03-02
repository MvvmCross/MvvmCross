#region Copyright
// <copyright file="MvxBaseTextProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Interfaces.Localization;

namespace Cirrious.MvvmCross.Localization
{
    public abstract class MvxBaseTextProvider :
        MvxBaseResourceProvider, IMvxTextProvider
    {
        #region Implementation of IMvxTextProvider

        public abstract string GetText(string namespaceKey, string typeKey, string name);

        public string GetText(string namespaceKey, string typeKey, string name, params object[] formatArgs)
        {
            var baseText = GetText(namespaceKey, typeKey, name);
            if (string.IsNullOrEmpty(baseText))
                return baseText;
            return string.Format(baseText, formatArgs);
        }

        #endregion
    }
}