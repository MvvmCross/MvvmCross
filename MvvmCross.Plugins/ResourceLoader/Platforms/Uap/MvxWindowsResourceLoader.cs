// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading.Tasks;
using MvvmCross.Platforms.Uap;
using Windows.ApplicationModel;

namespace MvvmCross.Plugin.ResourceLoader.Platforms.Uap
{
    public class MvxStoreResourceLoader : MvxResourceLoader
    {
        public override async ValueTask GetResourceStream(string resourcePath, Action<Stream> streamAction)
        {
            // we needed to replace the "/" with "\\" - see https://github.com/slodge/MvvmCross/issues/332
            resourcePath = resourcePath.Replace("/", "\\");
            var file = await Package.Current.InstalledLocation.GetFileAsync(resourcePath);
            var streamWithContent = await file.OpenReadAsync();
            var stream = streamWithContent.AsStreamForRead();
            streamAction?.Invoke(stream);
        }
    }
}
