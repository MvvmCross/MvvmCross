// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Threading.Tasks;

namespace MvvmCross.Plugins.PictureChooser
{
    public static class MvxPictureChooserExtensionMethods
    {
        public static Task<Stream> ChoosePictureFromLibraryAsync(this IMvxPictureChooserTask chooser, int maxPixelDimension, int percentQuality)
        {
            var task = new TaskCompletionSource<Stream>();
            chooser.ChoosePictureFromLibrary(maxPixelDimension, percentQuality, task.SetResult, () => task.SetResult(null));
            return task.Task;
        }

        public static Task<Stream> TakePictureAsync(this IMvxPictureChooserTask chooser, int maxPixelDimension, int percentQuality)
        {
            var task = new TaskCompletionSource<Stream>();
            chooser.TakePicture(maxPixelDimension, percentQuality, task.SetResult, () => task.SetResult(null));
            return task.Task;
        }
    }
}