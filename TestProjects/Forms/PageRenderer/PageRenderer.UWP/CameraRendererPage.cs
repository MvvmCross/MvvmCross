
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Linq;
using System.Threading.Tasks;

using Windows.ApplicationModel;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

using Xamarin.Forms.Platform.UWP;

using MvvmCross.Forms.Views.WindowsUWP;

using PageRendererExample.Pages;
using PageRendererExample.ViewModels;




[assembly: ExportRenderer(typeof(CameraRendererPage), typeof(PageRendererExample.UI.WindowsUWP.CameraRendererPage))]

namespace PageRendererExample.UI.WindowsUWP
{
    public class CameraRendererPage : MvxPageRenderer<CameraRendererViewModel>
    {

        Page _page;
        MediaCapture _mediaCapture;
        CaptureElement _captureElement;
        AppBarButton _takePhotoButton;
        AppBarButton _cancelButton;
        Button _anyButton;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Page> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            try
            {               
                SetupUserInterface();
                BindViewModel();
                SetupEventHandlers();
                SetupLiveCameraStream();

                var container = ContainerElement as Windows.UI.Xaml.Controls.Panel;
                container.Children.Add(_page);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"      ERROR: ", ex.Message);
            }
        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size size)
        {
            return size;
        }

        void SetupUserInterface()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            _takePhotoButton = new AppBarButton
            {
                Icon = new SymbolIcon(Symbol.Camera)
            };

            _cancelButton = new AppBarButton
            {
                Icon = new SymbolIcon(Symbol.Cancel)
            };

            var commandBar = new CommandBar();
            commandBar.PrimaryCommands.Add(_takePhotoButton);
            commandBar.PrimaryCommands.Add(_cancelButton);

            _captureElement = new CaptureElement();
            _captureElement.Stretch = Stretch.UniformToFill;

            var stackPanel = new StackPanel();
            stackPanel.Children.Add(_captureElement);

            _page = new Page();
            _page.BottomAppBar = commandBar;
            _page.Content = stackPanel;            
        }

        void BindViewModel()
        {
            var binding = new Binding {
                Path = new PropertyPath("CloseCommand"),
                Source=ViewModel
            };
            _cancelButton.SetBinding(ButtonBase.CommandProperty, binding);
        }

        void SetupEventHandlers()
        {
            var app = Application.Current;
            app.Suspending += OnAppSuspending;
            app.Resuming += OnAppResuming;

            _page.Unloaded += OnPageUnloaded;

            _takePhotoButton.Click += CapturePhotoPressed;
        }

        async void SetupLiveCameraStream()
        {
            var devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            var frontCamera = devices.FirstOrDefault(c => c.EnclosureLocation != null && c.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Front);
            var backCamera = devices.FirstOrDefault(c => c.EnclosureLocation != null && c.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back);

            DeviceInformation currentCamera = null;
            if (backCamera != null) {
                currentCamera = backCamera;
            } else if (frontCamera != null) {
                currentCamera = frontCamera;
            } else if (devices.Count() > 0) {
                currentCamera = devices.First();
            }

            _mediaCapture = new MediaCapture();
            await _mediaCapture.InitializeAsync(new MediaCaptureInitializationSettings
            {
                VideoDeviceId = currentCamera.Id,
                AudioDeviceId = string.Empty,
                StreamingCaptureMode = StreamingCaptureMode.Video,
                PhotoCaptureSource = PhotoCaptureSource.VideoPreview
            });

            _captureElement.Source = _mediaCapture;
            await _mediaCapture.StartPreviewAsync();
        }

        async void CapturePhotoPressed(object sender, RoutedEventArgs e)
        {
            var photoEncoding = ImageEncodingProperties.CreateJpeg();
            using (var imageStream = new InMemoryRandomAccessStream())
            {
                await _mediaCapture.CapturePhotoToStreamAsync(photoEncoding, imageStream);
                imageStream.Seek(0);
                var imageBytes = new byte[imageStream.Size];
                await imageStream.ReadAsync(imageBytes.AsBuffer(), (uint)imageStream.Size, InputStreamOptions.None);
                ViewModel.SetImage(imageBytes, "image/jpeg");
            }

            ViewModel.CloseCommand.Execute(this);
        }

        async void OnAppSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await CleanUpCaptureResourcesAsync();
            deferral.Complete();
        }

        void OnAppResuming(object sender, object e)
        {
            SetupLiveCameraStream();
        }

        async void OnPageUnloaded(object sender, RoutedEventArgs e)
        {
            await CleanUpCaptureResourcesAsync();
        }

        async Task CleanUpCaptureResourcesAsync()
        {
            if (_captureElement != null)
            {
                _captureElement.Source = null;
                _captureElement = null;
            }

            if (_mediaCapture != null)
            {
                try
                {
                    await _mediaCapture.StopPreviewAsync();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(@"          Error: ", ex.Message);
                }
              
            }

            if (_mediaCapture != null)
            {
                try
                {
                    _mediaCapture.Dispose();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(@"          Error: ", ex.Message);
                }
                finally
                {
                    _mediaCapture = null;
                }

            }
        }
    }
}
