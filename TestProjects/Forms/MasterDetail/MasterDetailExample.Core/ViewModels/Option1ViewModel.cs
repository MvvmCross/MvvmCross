using MvvmCross.Core.ViewModels;

namespace MasterDetailExample.Core.ViewModels
{
    public class Option1ViewModel : MvxViewModel
    {
        private string _message;

        public Option1ViewModel()
        {
            Message = "Opción 1";
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
    }
}