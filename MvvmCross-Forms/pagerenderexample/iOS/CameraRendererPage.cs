using System;

using AVFoundation;
using UIKit;
using CoreGraphics;

using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;

using MvvmCross.Forms.Views.iOS;

using PageRendererExample.Pages;
using PageRendererExample.ViewModels;
using Foundation;
using MvvmCross.Binding.BindingContext;

[assembly: ExportRenderer(typeof(CameraRendererPage), typeof(PageRendererExample.UI.iOS.CameraRendererPage))]

namespace PageRendererExample.UI.iOS
{
    public class CameraRendererPage : MvxPageRenderer<CameraRendererViewModel>
    {
        AVCaptureSession _captureSession;
        AVCaptureDeviceInput _captureDeviceInput;
        AVCaptureStillImageOutput _stillImageOutput;
        AVCaptureVideoPreviewLayer _videoPreviewLayer;
        UIView _liveCameraStream;
        UIButton _takePhotoButton;
        UIButton _toggleCameraButton;
        UIButton _toggleFlashButton;
        UIButton _cancelButton;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null) {
                return;
            }

            try {
                SetupUserInterface();
                BindViewModel();
                SetupEventHandlers();
                SetupLiveCameraStream();
                AuthorizeCameraUse();
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(@"          ERROR: ", ex.Message);
            }
        }

        void SetupUserInterface()
        {
            _liveCameraStream = new UIView {
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            _takePhotoButton = new UIButton {
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            _takePhotoButton.SetBackgroundImage(UIImage.FromFile("TakePhotoButton.png"), UIControlState.Normal);

            _toggleCameraButton = new UIButton {
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            _toggleCameraButton.SetBackgroundImage(UIImage.FromFile("ToggleCameraButton.png"), UIControlState.Normal);

            _toggleFlashButton = new UIButton {
                TranslatesAutoresizingMaskIntoConstraints = false
            };
            _toggleFlashButton.SetBackgroundImage(UIImage.FromFile("NoFlashButton.png"), UIControlState.Normal);

            _cancelButton = new UIButton(UIButtonType.Custom) {
                TranslatesAutoresizingMaskIntoConstraints = false 
            };
            _cancelButton.SetTitle("Cancel", UIControlState.Normal);
            _cancelButton.SetTitleColor(UIColor.White, UIControlState.Normal);

            View.Add(_liveCameraStream);
            View.Add(_takePhotoButton);
            View.Add(_toggleCameraButton);
            View.Add(_toggleFlashButton);
            View.Add(_cancelButton);

            var viewMetrics = new object[] {
                "cameraView", _liveCameraStream,
                "photoButton", _takePhotoButton,
                "photoButtonWidth", 70,
                "photoButtonHeight", 70,
                "photoButtonBottomMargin", 15,
                "cancelButton", _cancelButton,
                "cancelButtonWidth", 70,
                "cancelButtonHeight", 70,
                "cancelButtonMargin", 15,
                "toggleCameraButton", _toggleCameraButton,
                "toggleCameraButtonWidth", 35,
                "toggleCameraButtonHeight", 26,
                "toggleCameraButtonTopMargin", 20,
                "toggleCameraButtonRightMargin", 25,
                "toggleFlashButton", _toggleFlashButton,
                "toggleFlashButtonWidth", 37,
                "toggleFlashButtonHeight", 37,
                "toggleFlashButtonTopMargin", 20,
                "toggleFlashButtonLeftMargin", 25
            };

            var centerPhotoButton = NSLayoutConstraint.Create(View, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, _takePhotoButton, NSLayoutAttribute.CenterX, 1.0f, 0.0f);
            View.AddConstraint(centerPhotoButton);

            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:[photoButton(photoButtonWidth)]", 0, viewMetrics));
            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:[photoButton(photoButtonHeight)]-(photoButtonBottomMargin)-|", 0, viewMetrics));

            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-(cancelButtonMargin)-[cancelButton(cancelButtonWidth)]", 0, viewMetrics));
            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:[cancelButton(cancelButtonHeight)]-(cancelButtonMargin)-|", 0, viewMetrics));

            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:[toggleCameraButton(toggleCameraButtonWidth)]-(toggleCameraButtonRightMargin)-|", 0, viewMetrics));
            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-(toggleCameraButtonTopMargin)-[toggleCameraButton(toggleCameraButtonHeight)]", 0, viewMetrics));

            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-(toggleFlashButtonLeftMargin)-[toggleFlashButton(toggleFlashButtonWidth)]", 0, viewMetrics));
            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-(toggleFlashButtonLeftMargin)-[toggleFlashButton(toggleFlashButtonHeight)]", 0, viewMetrics));

            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[cameraView]|", 0, viewMetrics));
            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[cameraView]|", 0, viewMetrics));

            View.SetNeedsUpdateConstraints();
        }

        void BindViewModel()
        {
            var set = this.CreateBindingSet<CameraRendererPage, CameraRendererViewModel>();
            set.Bind(_cancelButton).To(nameof(ViewModel.CloseCommand));
            set.Apply();
        }

        void SetupEventHandlers()
        {
            _takePhotoButton.TouchUpInside += CapturePhoto;

            _toggleCameraButton.TouchUpInside += ToggleFrontBackCamera;

            _toggleFlashButton.TouchUpInside += ToggleFlash;
        }

        async void CapturePhoto(object sender, EventArgs e)
        {
            var videoConnection = _stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
            var sampleBuffer = await _stillImageOutput.CaptureStillImageTaskAsync(videoConnection);
            var jpegImage = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);

            ViewModel.SetImage(jpegImage.ToArray(), "image/jpeg");

            ViewModel.CloseCommand.Execute(this);
        }

        void ToggleFrontBackCamera(object sender, EventArgs e)
        {
            var devicePosition = _captureDeviceInput.Device.Position;
            if (devicePosition == AVCaptureDevicePosition.Front) {
                devicePosition = AVCaptureDevicePosition.Back;
            } else {
                devicePosition = AVCaptureDevicePosition.Front;
            }

            var device = GetCameraForOrientation(devicePosition);
            ConfigureCameraForDevice(device);

            _captureSession.BeginConfiguration();
            _captureSession.RemoveInput(_captureDeviceInput);
            _captureDeviceInput = AVCaptureDeviceInput.FromDevice(device);
            _captureSession.AddInput(_captureDeviceInput);
            _captureSession.CommitConfiguration();
        }

        void ToggleFlash(object sender, EventArgs e)
        {
            var device = _captureDeviceInput.Device;

            NSError error = null;
            if (device.HasFlash) {
                if (device.FlashMode == AVCaptureFlashMode.On) {
                    device.LockForConfiguration(out error);
                    device.FlashMode = AVCaptureFlashMode.Off;
                    device.UnlockForConfiguration();
                    _toggleFlashButton.SetBackgroundImage(UIImage.FromFile("NoFlashButton.png"), UIControlState.Normal);
                } else {
                    device.LockForConfiguration(out error);
                    device.FlashMode = AVCaptureFlashMode.On;
                    device.UnlockForConfiguration();
                    _toggleFlashButton.SetBackgroundImage(UIImage.FromFile("FlashButton.png"), UIControlState.Normal);
                }
            }
        }

        AVCaptureDevice GetCameraForOrientation(AVCaptureDevicePosition orientation)
        {
            var devices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);

            foreach (var device in devices) {
                if (device.Position == orientation) {
                    return device;
                }
            }

            return null;
        }

        public override void ViewWillTransitionToSize(CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
        {
            base.ViewWillTransitionToSize(toSize, coordinator);

            if (_videoPreviewLayer.Connection == null) {
                return;
            }

            if (_videoPreviewLayer.Connection.SupportsVideoOrientation) {
                var orientation = UIDevice.CurrentDevice.Orientation;

                switch (orientation) {
                    case UIDeviceOrientation.Portrait:
                        _videoPreviewLayer.Connection.VideoOrientation = AVCaptureVideoOrientation.Portrait;
                        break;
                    case UIDeviceOrientation.PortraitUpsideDown:
                        _videoPreviewLayer.Connection.VideoOrientation = AVCaptureVideoOrientation.PortraitUpsideDown;
                        break;
                    case UIDeviceOrientation.LandscapeLeft:
                        _videoPreviewLayer.Connection.VideoOrientation = AVCaptureVideoOrientation.LandscapeRight;
                        break;
                    case UIDeviceOrientation.LandscapeRight:
                        _videoPreviewLayer.Connection.VideoOrientation = AVCaptureVideoOrientation.LandscapeLeft;
                        break;
                    default:
                        _videoPreviewLayer.Connection.VideoOrientation = AVCaptureVideoOrientation.Portrait;
                        break;
                }
            }
        }

        void ObservedBoundsChange(NSObservedChange observedChange)
        {
            _videoPreviewLayer.Frame = _liveCameraStream.Bounds;
            _videoPreviewLayer.Bounds = _liveCameraStream.Bounds;
            _videoPreviewLayer.SetNeedsDisplay();
        }

        void SetupLiveCameraStream()
        {
            _captureSession = new AVCaptureSession();

            var viewLayer = _liveCameraStream.Layer;
            _videoPreviewLayer = new AVCaptureVideoPreviewLayer(_captureSession) {
                Frame = _liveCameraStream.Bounds
            };

            _liveCameraStream.AddObserver("bounds", NSKeyValueObservingOptions.New, ObservedBoundsChange);

            _liveCameraStream.Layer.AddSublayer(_videoPreviewLayer);

            var captureDevice = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);
            ConfigureCameraForDevice(captureDevice);
            _captureDeviceInput = AVCaptureDeviceInput.FromDevice(captureDevice);

            var dictionary = new NSMutableDictionary();
            dictionary[AVVideo.CodecKey] = new NSNumber((int)AVVideoCodec.JPEG);
            _stillImageOutput = new AVCaptureStillImageOutput {
                OutputSettings = new NSDictionary()
            };

            _captureSession.AddOutput(_stillImageOutput);
            _captureSession.AddInput(_captureDeviceInput);
            _captureSession.StartRunning();
        }

        void ConfigureCameraForDevice(AVCaptureDevice device)
        {
            NSError error = null;
            if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus)) {
                device.LockForConfiguration(out error);
                device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
                device.UnlockForConfiguration();
            } else if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure)) {
                device.LockForConfiguration(out error);
                device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
                device.UnlockForConfiguration();
            } else if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance)) {
                device.LockForConfiguration(out error);
                device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
                device.UnlockForConfiguration();
            }
        }

        async void AuthorizeCameraUse()
        {
            var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
            if (authorizationStatus != AVAuthorizationStatus.Authorized) {
                await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
            }
        }
    }
}

