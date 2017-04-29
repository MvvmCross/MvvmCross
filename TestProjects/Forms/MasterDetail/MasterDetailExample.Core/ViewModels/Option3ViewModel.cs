using MvvmCross.Core.ViewModels;

namespace MasterDetailExample.Core.ViewModels
{
    public class Option3ViewModel : MvxViewModel
    {
        private string _message;

        public Option3ViewModel()
        {
            Message = "Opción 3";
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
    }
}