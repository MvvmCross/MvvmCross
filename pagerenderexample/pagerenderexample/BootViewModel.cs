using System.Windows.Input;

using MvvmCross.Core.ViewModels;


namespace PageRendererExample.ViewModels
{
    public class BootViewModel : MvxViewModel
    {
        readonly IImageHolder _imageHolder;

        MvxCommand _showCustomPageRenderer;
        public ICommand  ShowCustomPageRenderer {
            get {
                if (_showCustomPageRenderer == null) {
                    _showCustomPageRenderer = new MvxCommand(() => ShowViewModel<CameraRendererViewModel>());
                }
                return _showCustomPageRenderer;
            }
        }

        public string PageTitle { get { return "Custom Renderer MvvmCross Style";} }

        public byte[] ImageBytes { get { return _imageHolder.ImageBytes; } }
        public string ImageMimeType { get { return _imageHolder.MimeType; } }

        public BootViewModel(IImageHolder imageHolder)
        {
            _imageHolder = imageHolder;
        }
    }
}

