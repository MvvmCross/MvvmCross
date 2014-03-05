// IMvxPictureChooserTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;

namespace Cirrious.MvvmCross.Plugins.PictureChooser
{
    public interface IMvxPictureChooserTask
    {
        // maybe ShowCamera as parameter? Does iOS/Droid supports this?
        void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                      Action assumeCancelled);
        /// <summary>
        /// Will set PixelHeight and PixelWidth to maxPixelDimension, creating a "crop"  interface on device
        /// </summary>
        void ChoosePictureFromLibraryWithCrop(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                      Action assumeCancelled);

        void TakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                         Action assumeCancelled);
    }
}