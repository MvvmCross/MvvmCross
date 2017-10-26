using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Logging;

namespace Logging.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        public MainViewModel(IMvxLogProvider logProvider)
        {
            logProvider.GetLogFor("MainViewModel").Warn(() => "Testing log");
        }
        
        public override Task Initialize()
        {
            //TODO: Add starting logic here
		    
            return base.Initialize();
        }
        
        public IMvxCommand ResetTextCommand => new MvxCommand(ResetText);
        private void ResetText()
        {
            Text = "Hello MvvmCross";
        }

        private string _text = "Hello MvvmCross";
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }
    }
}