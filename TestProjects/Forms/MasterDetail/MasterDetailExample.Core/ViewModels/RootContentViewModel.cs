using MvvmCross.Core.ViewModels;

namespace MasterDetailExample.Core.ViewModels
{
    public class RootContentViewModel : MvxViewModel
    {
        private string _message;

        public RootContentViewModel()
        {
            Message = "Página inicio principal";
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
    }
}