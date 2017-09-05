using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace $rootnamespace$.Core.ViewModels
{
    public class MvxFormsViewModel : MvxViewModel
    {
        private string _text;

        public MvxFormsViewModel()
        {
        }

        public IMvxCommand ShowTextCommand => new MvxCommand(ShowText);

        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        public override Task Initialize()
        {		    
            return base.Initialize();
        }        
        
        private void ShowText()
        {
            Text = "Hello MvvmCross!";
        }               
    }
}