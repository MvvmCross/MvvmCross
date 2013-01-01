// IMvxCombinedPictureChooserTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;

namespace Cirrious.MvvmCross.Plugins.PictureChooser
{
    public interface IMvxCombinedPictureChooserTask
    {
        void ChooseOrTakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                 Action assumeCancelled);
    }
}