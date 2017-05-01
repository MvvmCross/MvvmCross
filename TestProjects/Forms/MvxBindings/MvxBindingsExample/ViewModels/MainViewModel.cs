using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace MvxBindingsExample.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        public IMvxLanguageBinder TextSource
        {
            get { return new MvxLanguageBinder("MvxBindingsExample", "Text"); }
        }

        private string _bindableText = "I'm bound!";
        public string BindableText
        {
            get { return _bindableText; }
            set
            {
                if (BindableText != value)
                {
                    _bindableText = value;
                    RaisePropertyChanged();
                }
            }
        }

        public MainViewModel()
        {
        }

        public override void Start()
        {
            base.Start();
        }
    }
}

