using MvvmCross.Core.ViewModels;

namespace MvxBindingsExample.ViewModels
{
    public class CodeBehindViewModel : MvxViewModel
    {
        private string _bindableText = "I'm bound!";
        public string BindableText
        {
            get
            {
                return _bindableText;
            }
            set
            {
                if (BindableText != value)
                {
                    _bindableText = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
