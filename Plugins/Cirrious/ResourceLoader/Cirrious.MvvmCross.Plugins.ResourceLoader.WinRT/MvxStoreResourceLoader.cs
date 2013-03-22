// MvxStoreResourceLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using Cirrious.CrossCore.WindowsStore.Platform;
using Windows.ApplicationModel;

namespace Cirrious.MvvmCross.Plugins.ResourceLoader.WindowsStore
{
    public class MvxStoreResourceLoader : MvxResourceLoader
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