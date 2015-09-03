// IMvxPictureChooserTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using System.Threading.Tasks;

namespace MvvmCross.Plugins.PictureChooser
{
    public interface IMvxPictureChooserTask
    {
        void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                      Action assumeCancelled);

        void TakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                         Action assumeCancelled);

        /// <summary>
        /// Returns null if cancelled
        /// </summary>
        Task<Stream> ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality);

        /// <summary>
        /// Returns null if cancelled
        /// </summary>
        Task<Stream> TakePicture(int maxPixelDimension, int percentQuality);

        void ContinueFileOpenPicker(object args);
    }
}
