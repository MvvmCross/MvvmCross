#region Copyright
// <copyright file="MvxWindowsPhoneResourceLoader.cs" company="Cirrious">
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
using Cirrious.MvvmCross.Platform;

namespace Cirrious.MvvmCross.WindowsPhone.Platform
{
    public class MvxWindowsPhoneResourceLoader : MvxBaseResourceLoader
    {
        #region Implementation of IMvxResourceLoader

        public override void GetResourceStream(string resourcePath, Action<Stream> streamAction)
        {
#warning ? need to check and clarify what exceptions can be thrown here!
            var streamInfo = System.Windows.Application.GetResourceStream(new Uri(resourcePath, UriKind.Relative));
            if (streamInfo != null)
                streamAction(streamInfo.Stream);
            else
                streamAction(null);
        }

        #endregion
    }
}
