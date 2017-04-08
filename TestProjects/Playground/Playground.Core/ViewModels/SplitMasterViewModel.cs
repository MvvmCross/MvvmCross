using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
namespace Playground.Core.ViewModels
{
    public class SplitMasterViewModel : MvxViewModel
    {
        public SplitMasterViewModel()
        {
        }

        private ICommand _openDetailCommand;
        public ICommand OpenDetailCommand
        {
            get
            {
                return _openDetailCommand ?? (_openDetailCommand = new MvxCommand(() => ShowViewModel<SplitDetailViewModel>()));
            }
        }
    }
}
