#region Copyright

// <copyright file="MvxWinRTResourceLoader.cs" company="Cirrious">
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
using Cirrious.MvvmCross.WinRT.ExtensionMethods;
using Windows.ApplicationModel;

namespace Cirrious.MvvmCross.Plugins.ResourceLoader.WinRT
{
    public class MvxWinRTResourceLoader : MvxBaseResourceLoader
    {
        #region Implementation of IMvxResourceLoader

        public override void GetResourceStream(string resourcePath, Action<Stream> streamAction)
        {
            var file = Package.Current.InstalledLocation.GetFileAsync(resourcePath).Await();
            var streamWithContent = file.OpenReadAsync().Await();
            var stream = streamWithContent.AsStreamForRead();
            streamAction(stream);
        }

        #endregion
    }
}