using System;
using System.IO;

namespace Cirrious.MvvmCross.Interfaces.Platform.Tasks
{
    public interface IMvxCombinedPictureChooserTask
    {
        void ChooseOrTakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled);
    }
}