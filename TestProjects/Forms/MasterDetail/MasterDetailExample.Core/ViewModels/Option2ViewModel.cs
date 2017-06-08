using MvvmCross.Core.ViewModels;

namespace MasterDetailExample.Core.ViewModels
{
    public class Option2ViewModel : MvxViewModel
    {
        private string _message;
        public string Message { get { return _message; } set { SetProperty(ref _message, value); } }

        public Option2ViewModel()
        {
            Message = "Opción 2";
        }
    }
}
