using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace PageRendererExample
{
    public class CameraRendererViewModel : MvxViewModel
    {
        private readonly IImageHolder _imageHolder;

        private MvxCommand _closeCommand;
        public ICommand CloseCommand {
            get {
                if (_closeCommand == null) {
                    _closeCommand = new MvxCommand(() => Close(this));
                }

                return _closeCommand;
            }
        }

        public void SetImage(byte[] imageBytes, string mimeType)
        {
            _imageHolder.Update(imageBytes, mimeType);
        }

        public CameraRendererViewModel(IImageHolder imageHolder)
        {
            _imageHolder = imageHolder;
        }
    }
}