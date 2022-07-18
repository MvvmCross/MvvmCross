// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Windows.ApplicationModel;

namespace MvvmCross.Plugin.ResourceLoader.Platforms.WinUi
{
    public class MvxStoreResourceLoader : MvxResourceLoader
    {
        public override void GetResourceStream(string resourcePath, Action<Stream> streamAction)
        {
            // we needed to replace the "/" with "\\" - see https://github.com/slodge/MvvmCross/issues/332
            resourcePath = resourcePath.Replace("/", "\\");
            var file = Package.Current.InstalledLocation.GetFileAsync(resourcePath).GetResults();
            var streamWithContent = file.OpenReadAsync().GetResults();
            var stream = streamWithContent.AsStreamForRead();
            streamAction?.Invoke(stream);
        }
    }
}
