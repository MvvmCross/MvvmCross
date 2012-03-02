#region Copyright
// <copyright file="IMvxJsonDictionaryTextLoader.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
namespace Cirrious.MvvmCross.Interfaces.Localization
{
    public interface IMvxJsonDictionaryTextLoader
    {
        void LoadJsonFromResource(string namespaceKey, string typeKey, string resourcePath);
        void LoadJsonFromText(string namespaceKey, string typeKey, string rawJson);
    }
}