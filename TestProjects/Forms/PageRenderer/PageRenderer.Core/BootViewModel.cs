using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace PageRendererExample.ViewModels
{
    public class BootViewModel : MvxViewModel
    {
        private readonly IImageHolder _imageHolder;

        private MvxCommand _showCustomPageRenderer;

        public BootViewModel(IImageHolder imageHolder)
        {
            _imageHolder = imageHolder;
        }

        public ICommand ShowCustomPageRenderer
        {
            get
            {
                if (_showCustomPageRenderer == null)
                    _showCustomPageRenderer = new MvxCommand(() => ShowViewModel<CameraRendererViewModel>());
                return _showCustomPageRenderer;
            }
        }

        public string PageTitle => "Custom Renderer MvvmCross Style";

        public byte[] ImageBytes => _imageHolder.ImageBytes;
        public string ImageMimeType => _imageHolder.MimeType;
    }
}