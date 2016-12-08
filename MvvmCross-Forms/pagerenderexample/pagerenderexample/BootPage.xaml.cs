using System.IO;

using Xamarin.Forms;

namespace PageRendererExample.Pages
{
    public partial class BootPage : BootContentPage
    {
        Button _showCameraButton;
        Image _capturedImage;

        public BootPage()
        {
            InitializeComponent();

            _capturedImage = new Image {
                BackgroundColor = Color.White
            };
            _relativeLayout.Children.Add(
                _capturedImage,
                Constraint.RelativeToParent(parent => 0.10 * parent.Width),
                Constraint.RelativeToParent(parent => 0.10 * parent.Height),
                Constraint.RelativeToParent(parent => 0.80 * parent.Width),
                Constraint.RelativeToParent(parent => 0.80 * parent.Height)
            );

            _showCameraButton = new Button {
                HeightRequest=44,
                WidthRequest=200,
                Text = "Show Camera",
                StyleId = "ShowCustomPageRendererButton"
            };

            _relativeLayout.Children.Add(
                _showCameraButton,
                Constraint.RelativeToParent(parent => 0.5 * parent.Width - 100.0),
                Constraint.RelativeToParent(parent => parent.Height - 44.0)
            );
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            _showCameraButton.Command = ViewModel.ShowCustomPageRenderer;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel.ImageBytes != null && ViewModel.ImageBytes.Length > 0)
            {
                _capturedImage.Source = ImageSource.FromStream(() => new MemoryStream(ViewModel.ImageBytes));
            }
        }
    }
}

