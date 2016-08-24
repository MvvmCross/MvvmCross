// MvxPictureChooserExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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