// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using Microsoft.Extensions.Logging;
using MvvmCross.Logging;
using MvvmCross.Platforms.Ios;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.WeakSubscription;
using UIKit;

namespace MvvmCross.Plugin.PictureChooser.Platforms.Ios
{
    [MvvmCross.Preserve(AllMembers = true)]
    public class MvxImagePickerTask
        : MvxIosTask, IMvxPictureChooserTask
    {
        private UIImagePickerController _picker;
        private bool _currentlyActive;
        private int _maxPixelDimension;
        private int _percentQuality;
        private Action<Stream, string> _pictureAvailable;
        private Action _assumeCancelled;

        private UIImagePickerController EnsurePickerController()
        {
            if (_picker != null)
            {
                UnsubscribeEvents();
                SubscribeEvents();
                return _picker;
            }

            _picker = new UIImagePickerController();
            SubscribeEvents();

            return _picker;
        }

        private void SubscribeEvents()
        {
            _picker.FinishedPickingMedia += Picker_FinishedPickingMedia;
            _picker.FinishedPickingImage += Picker_FinishedPickingImage;
            _picker.Canceled += Picker_Canceled;
        }

        private void UnsubscribeEvents()
        {
            _picker.FinishedPickingMedia -= Picker_FinishedPickingMedia;
            _picker.FinishedPickingImage -= Picker_FinishedPickingImage;
            _picker.Canceled -= Picker_Canceled;
        }

        public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream, string> pictureAvailable,
                                     Action assumeCancelled)
        {
            var picker = EnsurePickerController();

            picker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            ChoosePictureCommon(maxPixelDimension, percentQuality, pictureAvailable, assumeCancelled);
        }

        public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                             Action assumeCancelled)
        {
            ChoosePictureFromLibrary(maxPixelDimension, percentQuality, (stream, name) => pictureAvailable(stream), assumeCancelled);
        }

        public void TakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                Action assumeCancelled)
        {
            var picker = EnsurePickerController();

            picker.SourceType = UIImagePickerControllerSourceType.Camera;
            ChoosePictureCommon(maxPixelDimension, percentQuality, (stream, name) => pictureAvailable(stream), assumeCancelled);
        }

        public Task<Stream> ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality)
        {
            var task = new TaskCompletionSource<Stream>();
            ChoosePictureFromLibrary(maxPixelDimension, percentQuality, task.SetResult, () => task.SetResult(null));
            return task.Task;
        }

        public Task<Stream> TakePicture(int maxPixelDimension, int percentQuality)
        {
            var task = new TaskCompletionSource<Stream>();
            TakePicture(maxPixelDimension, percentQuality, task.SetResult, () => task.SetResult(null));
            return task.Task;
        }

        public void ContinueFileOpenPicker(object args)
        {
        }

        private void ChoosePictureCommon(int maxPixelDimension, int percentQuality,
                                         Action<Stream, string> pictureAvailable, Action assumeCancelled)
        {
            SetCurrentlyActive();
            _maxPixelDimension = maxPixelDimension;
            _percentQuality = percentQuality;
            _pictureAvailable = pictureAvailable;
            _assumeCancelled = assumeCancelled;

            var picker = EnsurePickerController();
            UIApplication.SharedApplication.KeyWindow.GetTopModalHostViewController().PresentViewController(picker, true, null);
        }

        private void HandleImagePick(UIImagePickerController picker, UIImage image, string name)
        {
            ClearCurrentlyActive();
            if (image != null)
            {
                if (_maxPixelDimension > 0 && (image.Size.Height > _maxPixelDimension || image.Size.Width > _maxPixelDimension))
                {
                    // resize the image
                    image = image.ImageToFitSize(new CGSize(_maxPixelDimension, _maxPixelDimension));
                }

                using (NSData data = image.AsJPEG(_percentQuality / 100f))
                {
                    var byteArray = new byte[data.Length];
                    Marshal.Copy(data.Bytes, byteArray, 0, Convert.ToInt32(data.Length));

                    var imageStream = new MemoryStream(byteArray, false);
                    _pictureAvailable?.Invoke(imageStream, name);
                }
            }
            else
            {
                _assumeCancelled?.Invoke();
            }

            picker.DismissViewController(true, () => { });
        }

        private void Picker_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            // As of iOS 12, multiple taps on an image in the library may lead to the FinishedPickingImage event
            // being fired more than once (see #3215). We unsubscribe from the controller events to prevent this.
            UnsubscribeEvents();
            var referenceURL = e.Info[new NSString("UIImagePickerControllerReferenceURL")] as NSUrl;
            var image = e.EditedImage ?? e.OriginalImage;
            HandleImagePick((UIImagePickerController)sender, image, referenceURL != null ? referenceURL.AbsoluteString : string.Empty);
        }

        private void Picker_FinishedPickingImage(object sender, UIImagePickerImagePickedEventArgs e)
        {
            UnsubscribeEvents();
            var referenceURL = e.EditingInfo["UIImagePickerControllerReferenceURL"] as NSUrl;
            var image = e.Image;
            HandleImagePick((UIImagePickerController)sender, image, referenceURL != null ? referenceURL.AbsoluteString : string.Empty);
        }

        private void Picker_Canceled(object sender, EventArgs e)
        {
            UnsubscribeEvents();
            ClearCurrentlyActive();
            _assumeCancelled?.Invoke();
            ((UIImagePickerController)sender).DismissViewController(true, () => { });
        }

        private void SetCurrentlyActive()
        {
            if (_currentlyActive)
                MvxPluginLog.Instance?.Log(LogLevel.Warning, "MvxImagePickerTask called when task already active");
            _currentlyActive = true;
        }

        private void ClearCurrentlyActive()
        {
            if (!_currentlyActive)
                MvxPluginLog.Instance?.Log(LogLevel.Warning, "Tried to clear currently active - but already cleared");
            _currentlyActive = false;
        }
    }
}
