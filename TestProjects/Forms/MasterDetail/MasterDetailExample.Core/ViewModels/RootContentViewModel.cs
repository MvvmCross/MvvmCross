using MvvmCross.Core.ViewModels;

namespace MasterDetailExample.Core.ViewModels
{
    public class RootContentViewModel : MvxViewModel
    {
        private string _message;
        public string Message { get { return _message; } set { SetProperty(ref _message, value); } }

        public RootContentViewModel()
        {
            Message = "Página inicio principal";
        }
    }
}
