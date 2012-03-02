#region Copyright
// <copyright file="IMvxLanguageBinder.cs" company="Cirrious">
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
#warning What namepsace does this go in?!
    public interface IMvxLanguageBinder
    {
        string GetText(string entryKey);
        string GetText(string entryKey, params object[] args);
    }
}