#region Copyright
// <copyright file="MvxBaseResourceProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
namespace Cirrious.MvvmCross.Localization
{
    public abstract class MvxBaseResourceProvider
    {
        protected static string MakeLookupKey(string namespaceKey, string typeKey)
        {
            return string.Format("{0}|{1}", namespaceKey, typeKey);
        }

        protected static string MakeLookupKey(string namespaceKey, string typeKey, string name)
        {
            return string.Format("{0}|{1}|{2}", namespaceKey, typeKey, name);
        }
    }
}