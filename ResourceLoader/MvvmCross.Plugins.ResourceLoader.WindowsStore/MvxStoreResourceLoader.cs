// MvxStoreResourceLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.WindowsCommon.Platform;
using System;
using System.IO;
using Windows.ApplicationModel;

namespace MvvmCross.Plugins.ResourceLoader.WindowsStore
{
    public class MvxStoreResourceLoader : MvxResourceLoader
    {
        public override void GetResourceStream(string resourcePath, Action<Stream> streamAction)
        {
            // we needed to replace the "/" with "\\" - see https://github.com/slodge/MvvmCross/issues/332
            resourcePath = resourcePath.Replace("/", "\\");
            var file = Package.Current.InstalledLocation.GetFileAsync(resourcePath).Await();
            var streamWithContent = file.OpenReadAsync().Await();
            var stream = streamWithContent.AsStreamForRead();
            streamAction?.Invoke(stream);
        }
    }
}