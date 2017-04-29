using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class RootViewModel : MvxViewModel
    {
        private int _counter = 2;
        private string _hello = "Hello MvvmCross";

        private ICommand _showChildCommand;

        private ICommand _showModalCommand;

        private ICommand _showModalNavCommand;

        private ICommand _showSplitCommand;

        private ICommand _showTabsCommand;

        public string Hello
        {
            get => _hello;
            set
            {
                _hello = value;
                RaisePropertyChanged(() => Hello);
            }
        }

        public int Counter
        {
            get => _counter;
            set
            {
                _counter = value;
                RaisePropertyChanged(() => Counter);
            }
        }

        public ICommand ShowChildCommand
        {
            get
            {
                return _showChildCommand ?? (_showChildCommand = new MvxCommand(() => ShowViewModel<ChildViewModel>()));
            }
        }

        public ICommand ShowModalCommand
        {
            get
            {
                return _showModalCommand ?? (_showModalCommand = new MvxCommand(() => ShowViewModel<ModalViewModel>()));
            }
        }

        public ICommand ShowModalNavCommand
        {
            get
            {
                return _showModalNavCommand ?? (_showModalNavCommand =
                           new MvxCommand(() => ShowViewModel<ModalNavViewModel>()));
            }
        }

        public ICommand ShowTabsCommand
        {
            get
            {
                return _showTabsCommand ?? (_showTabsCommand =
                           new MvxCommand(() => ShowViewModel<TabsRootViewModel>()));
            }
        }

        public ICommand ShowSplitCommand
        {
            get
            {
                return _showSplitCommand ?? (_showSplitCommand =
                           new MvxCommand(() => ShowViewModel<SplitRootViewModel>()));
            }
        }
    }
}