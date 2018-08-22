using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using MvvmCross.Forms.Uwp;
using PageRendererExample;
using Xamarin.Forms.Platform.UWP;
using Panel = Windows.UI.Xaml.Controls.Panel;

[assembly: ExportRenderer(typeof(CameraRendererPage), typeof(PageRendererExample.WindowsUWP.CameraRendererPage))]

namespace PageRendererExample.WindowsUWP
{
    public class CameraRendererPage : MvxPageRenderer<CameraRendererViewModel>
    {
        private Page _page;
        private MediaCapture _mediaCapture;
        private CaptureElement _captureElement;
        private AppBarButton _takePhotoButton;
        private AppBarButton _cancelButton;
        private Button _anyButton;

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

                var container = ContainerElement as Panel;
                container.Children.Add(_page);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"      ERROR: ", ex.Message);
            }
        }

        protected override Size ArrangeOverride(Size size)
        {
            return size;
        }

        private void SetupUserInterface()
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

        private void BindViewModel()
        {
            var binding = new Binding {
                Path = new PropertyPath("CloseCommand"),
                Source = ViewModel
            };
            _cancelButton.SetBinding(ButtonBase.CommandProperty, binding);
        }

        private void SetupEventHandlers()
        {
            var app = Application.Current;
            app.Suspending += OnAppSuspending;
            app.Resuming += OnAppResuming;

            _page.Unloaded += OnPageUnloaded;

            _takePhotoButton.Click += CapturePhotoPressed;
        }

        private async void SetupLiveCameraStream()
        {
            var devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            var frontCamera = devices.FirstOrDefault(c => c.EnclosureLocation != null && c.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Front);
            var backCamera = devices.FirstOrDefault(c => c.EnclosureLocation != null && c.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back);

            DeviceInformation currentCamera = null;
            if (backCamera != null) 
            {
                currentCamera = backCamera;
            } 
            else if (frontCamera != null) 
            {
                currentCamera = frontCamera;
            } 
            else if (devices.Count() > 0) 
            {
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

        private async void CapturePhotoPressed(object sender, RoutedEventArgs e)
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

        private async void OnAppSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await CleanUpCaptureResourcesAsync();
            deferral.Complete();
        }

        private void OnAppResuming(object sender, object e)
        {
            SetupLiveCameraStream();
        }

        private async void OnPageUnloaded(object sender, RoutedEventArgs e)
        {
            await CleanUpCaptureResourcesAsync();
        }

        private async Task CleanUpCaptureResourcesAsync()
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
                    Debug.WriteLine(@"          Error: ", ex.Message);
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
                    Debug.WriteLine(@"          Error: ", ex.Message);
                }
                finally
                {
                    _mediaCapture = null;
                }
            }
        }
    }
}
