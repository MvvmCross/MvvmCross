using MvvmCross.Core.ViewModels;

namespace $rootnamespace$.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        public MainViewModel()
        {
            ResetTextCommand = new MvxCommand(ResetTextCommandExecute);
        }

        // Even though there's no Init method on MvxViewModel, this method is used by
        // the framework to pass information when navigating from one VM to another.
        // Every ViewModel can have its own Init method, as long as they either:
        // 1 - Implement MvxViewModel<TInit> 
        // 2 - Follow the guidelines from this document: https://github.com/MvvmCross/MvvmCross/wiki/view-model-lifecycle
        // If you choose the former, avoid serializing large objects
        // If you choose the latter, avoid doing asynchronous operations on Init, prefer doing them on the Start method.
        // public void Init(int id)
        // {
        //     this.id = id
        // }

        // The start method is called as the last step of the ViewModel initialization.
        // Even though it is a void method, if you need to do any async work it should be done here.
        public override void Start()
        {
            //TODO: Add starting logic here
        }

        // Commands are a way to bind events that happen on your View to methods from the ViewModel 
        public IMvxCommand ResetTextCommand { get; }
        private void ResetTextCommandExecute()
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