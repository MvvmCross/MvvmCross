#region Copyright
// <copyright file="IMvxResourceLoader.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.IO;

namespace Cirrious.MvvmCross.Interfaces.Localization
{
    public interface IMvxResourceLoader
    {
        string GetTextResource(string resourcePath);
        void GetResourceStream(string resourcePath, Action<Stream> streamAction);
    }
}