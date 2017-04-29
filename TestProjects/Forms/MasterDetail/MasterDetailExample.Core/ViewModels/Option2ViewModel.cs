using MvvmCross.Core.ViewModels;

namespace MasterDetailExample.Core.ViewModels
{
    public class Option2ViewModel : MvxViewModel
    {
        private string _message;

        public Option2ViewModel()
        {
            Message = "Opción 2";
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
    }
}