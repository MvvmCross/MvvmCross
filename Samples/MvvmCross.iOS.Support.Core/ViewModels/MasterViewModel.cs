namespace MvvmCross.iOS.Support.Core.ViewModels
{
    using MvvmCross.Core.ViewModels;

    public class MasterViewModel : BaseViewModel
    {
        public MasterViewModel()
        {
            ExampleValue = "Master View";
        }

        public IMvxCommand ShowDetailCommand
        {
            get
            {
                return new MvxCommand(ShowDetailCommandExecuted);
            }
        }

        private void ShowDetailCommandExecuted()
        {
            ShowViewModel<DetailViewModel>();
        }
    }
}