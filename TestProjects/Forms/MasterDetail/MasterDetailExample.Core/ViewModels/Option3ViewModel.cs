using MvvmCross.Core.ViewModels;

namespace MasterDetailExample.Core.ViewModels
{
    public class Option3ViewModel : MvxViewModel
    {
        private string _message;
        public string Message { get { return _message; } set { SetProperty(ref _message, value); } }

        public Option3ViewModel()
        {
            Message = "Opción 3";
        }
    }
}
