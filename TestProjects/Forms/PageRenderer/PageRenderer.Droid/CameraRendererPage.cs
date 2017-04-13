using System;
using System.IO;

using Android.App;
using Android.Graphics;
using Android.Hardware;
using Android.Views;
using Android.Widget;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using MvvmCross.Forms.Views.Droid;

using PageRendererExample.Pages;
using PageRendererExample.ViewModels;
using MvvmCross.Binding.BindingContext;

[assembly:ExportRenderer (typeof(CameraRendererPage), typeof(PageRendererExample.UI.Droid.CameraRendererPage))]

namespace PageRendererExample.UI.Droid
{
    public class CameraRendererPage : MvxPageRenderer<CameraRendererViewModel>, TextureView.ISurfaceTextureListener
    {
        // The Camera API is officially deprecated and should be replaced with Camera2.
        // Camera2 is, however, not available until Android 5.1 and there are lots of reports
        // of devices reporting incorrect information.
        Android.Hardware.Camera camera;
        Android.Widget.Button takePhotoButton;
        Android.Widget.Button toggleFlashButton;
        Android.Widget.Button switchCameraButton;
        Android.Widget.Button cancelButton;
        Android.Views.View view;

        Activity activity;
        CameraFacing cameraType;
        TextureView textureView;
        SurfaceTexture surfaceTexture;

        bool flashOn;

        protected override void OnElementChanged (ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged (e);

            if (e.OldElement != null || Element == null) {
                return;
            }

            try {

                SetupUserInterface();
                BindViewModel();
                SetupEventHandlers();
                AddView(view);
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(@"          ERROR: ", ex.Message);
            }

        }

        void SetupUserInterface()
        {
            activity = Context as Activity;
            view = activity.LayoutInflater.Inflate(Resource.Layout.CameraLayout, this, false);
            cameraType = CameraFacing.Back;

            textureView = view.FindViewById<TextureView>(Resource.Id.textureView);
            textureView.SurfaceTextureListener = this;

            takePhotoButton = view.FindViewById<Android.Widget.Button>(Resource.Id.takePhotoButton);
            switchCameraButton = view.FindViewById<Android.Widget.Button>(Resource.Id.switchCameraButton);
            toggleFlashButton = view.FindViewById<Android.Widget.Button>(Resource.Id.toggleFlashButton);
            cancelButton = view.FindViewById<Android.Widget.Button>(Resource.Id.cancelButton);
        }

        void SetupEventHandlers()
        {            
            takePhotoButton.Click += TakePhotoButtonTapped;
            switchCameraButton.Click += SwitchCameraButtonTapped;
            toggleFlashButton.Click += ToggleFlashButtonTapped;
        }

        void BindViewModel()
        {
            var set = this.CreateBindingSet<CameraRendererPage, CameraRendererViewModel>();
            set.Bind(cancelButton).To(nameof(ViewModel.CloseCommand));
            set.Apply();
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

            view.Measure(msw, msh);
            view.Layout(0, 0, r - l, b - t);
        }

        public void OnSurfaceTextureUpdated (SurfaceTexture surface)
        {

        }

        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            camera = Android.Hardware.Camera.Open((int)cameraType);
            textureView.LayoutParameters = new FrameLayout.LayoutParams(width, height);
            surfaceTexture = surface;

            camera.SetPreviewTexture(surface);
            PrepareAndStartCamera();
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            camera.StopPreview();
            camera.Release();
            return true;
        }

        public void OnSurfaceTextureSizeChanged (SurfaceTexture surface, int width, int height)
        {
            PrepareAndStartCamera();
        }

        void PrepareAndStartCamera()
        {
            camera.StopPreview();

            var display = activity.WindowManager.DefaultDisplay;
            if (display.Rotation == SurfaceOrientation.Rotation0) {
                camera.SetDisplayOrientation(90);
            }

            if (display.Rotation == SurfaceOrientation.Rotation270) {
                camera.SetDisplayOrientation(180);
            }

            camera.StartPreview();
        }

        void ToggleFlashButtonTapped(object sender, EventArgs e)
        {
            flashOn = !flashOn;
            if (flashOn) {
                if (cameraType == CameraFacing.Back) {
                    toggleFlashButton.SetBackgroundResource(Resource.Drawable.FlashButton);
                    cameraType = CameraFacing.Back;

                    camera.StopPreview();
                    camera.Release();
                    camera = Android.Hardware.Camera.Open((int)cameraType);
                    var parameters = camera.GetParameters();
                    parameters.FlashMode = Android.Hardware.Camera.Parameters.FlashModeTorch;
                    camera.SetParameters(parameters);
                    camera.SetPreviewTexture(surfaceTexture);
                    PrepareAndStartCamera();
                }
            } else {
                toggleFlashButton.SetBackgroundResource(Resource.Drawable.NoFlashButton);
                camera.StopPreview();
                camera.Release();

                camera = Android.Hardware.Camera.Open((int)cameraType);
                var parameters = camera.GetParameters();
                parameters.FlashMode = Android.Hardware.Camera.Parameters.FlashModeOff;
                camera.SetParameters(parameters);
                camera.SetPreviewTexture(surfaceTexture);
                PrepareAndStartCamera();
            }
        }

        void SwitchCameraButtonTapped(object sender, EventArgs e)
        {
            if (cameraType == CameraFacing.Front) {
                cameraType = CameraFacing.Back;

                camera.StopPreview();
                camera.Release();
                camera = Android.Hardware.Camera.Open((int)cameraType);
                camera.SetPreviewTexture(surfaceTexture);
                PrepareAndStartCamera();
            } else {
                cameraType = CameraFacing.Front;

                camera.StopPreview();
                camera.Release();
                camera = Android.Hardware.Camera.Open((int)cameraType);
                camera.SetPreviewTexture(surfaceTexture);
                PrepareAndStartCamera();
            }
        }

        async void TakePhotoButtonTapped(object sender, EventArgs e)
        {
            camera.StopPreview();

            var image = textureView.Bitmap;

            try {
                using (var stream = new MemoryStream()) {
                    await image.CompressAsync(Bitmap.CompressFormat.Jpeg, 50, stream);
                    image.Recycle();

                    ViewModel.SetImage(stream.GetBuffer(), "image/jpeg");
                }

                ViewModel.CloseCommand.Execute(this);
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(@"              ", ex.Message);
            }
        }
    }
}
    