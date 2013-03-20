// MvxImagePickerTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Touch.Platform;
using Cirrious.CrossCore.Touch.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Plugins.PictureChooser.Touch
{
    public class MvxImagePickerTask
        : MvxTouchTask
          , IMvxPictureChooserTask          
    {
        private readonly UIImagePickerController _picker;
        private readonly IMvxTouchModalHost _modalHost;

        public MvxImagePickerTask()
        {
            _modalHost = Mvx.Resolve<IMvxTouchModalHost>();
            _picker = new UIImagePickerController();
        }

        #region IMvxPictureChooserTask Members

        public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                             Action assumeCancelled)
        {
            _picker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            ChoosePictureCommon(maxPixelDimension, percentQuality, pictureAvailable, assumeCancelled);
        }

        public void TakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                Action assumeCancelled)
        {
            _picker.SourceType = UIImagePickerControllerSourceType.Camera;
            ChoosePictureCommon(maxPixelDimension, percentQuality, pictureAvailable, assumeCancelled);
        }

        #endregion

        private void ChoosePictureCommon(int maxPixelDimension, int percentQuality,
                                         Action<Stream> pictureAvailable, Action assumeCancelled)
        {
            _picker.FinishedPickingMedia += (sender, e) =>
                {
                    var image = e.EditedImage ?? e.OriginalImage;
                    HandleImagePick(image, maxPixelDimension, percentQuality, pictureAvailable, assumeCancelled);
                };

            _picker.FinishedPickingImage += (sender, e) =>
                {
                    var image = e.Image;
                    HandleImagePick(image, maxPixelDimension, percentQuality, pictureAvailable, assumeCancelled);
                };

            _picker.Canceled += (sender, e) =>
                {
                    assumeCancelled();
                    _picker.DismissViewController(true, () => { });
                    _modalHost.NativeModalViewControllerDisappearedOnItsOwn();
                };
            _modalHost.PresentModalViewController(_picker, true);
        }

        private void HandleImagePick(UIImage image, int maxPixelDimension, int percentQuality,
                                     Action<Stream> pictureAvailable, Action assumeCancelled)
        {
            if (image != null)
            {
                // resize the image
                image = image.ImageToFitSize(new SizeF(maxPixelDimension, maxPixelDimension));

                using (NSData data = image.AsJPEG((float) (percentQuality/100.0)))
                {
                    var byteArray = new byte[data.Length];
                    Marshal.Copy(data.Bytes, byteArray, 0, Convert.ToInt32(data.Length));

                    var imageStream = new MemoryStream();
                    imageStream.Write(byteArray, 0, Convert.ToInt32(data.Length));
                    imageStream.Seek(0, SeekOrigin.Begin);

                    pictureAvailable(imageStream);
                }
            }
            else
            {
                assumeCancelled();
            }

            _picker.DismissViewController(true, () => { });
            _modalHost.NativeModalViewControllerDisappearedOnItsOwn();
        }

        #region Nested type: CameraDelegate

        private class CameraDelegate : UIImagePickerControllerDelegate
        {
            public Action<UIImage, NSDictionary> Callback { get; set; }

            public override void FinishedPickingImage(UIImagePickerController picker, UIImage image,
                                                      NSDictionary editingInfo)
            {
                if (Callback != null)
                    Callback(image, editingInfo);
            }

            public override void Canceled(UIImagePickerController picker)
            {
                if (Callback != null)
                    Callback(null, null);
            }
        }

        #endregion
    }
}