using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class RootViewModel : MvxViewModel
    {
        private string _hello = "Hello MvvmCross";
        public string Hello
        {
            get { return _hello; }
            set { _hello = value; RaisePropertyChanged(() => Hello); }
        }

        private int _counter = 2;
        public int Counter
        {
            get { return _counter; }
            set { _counter = value; RaisePropertyChanged(() => Counter); }
        }

        private ICommand _showChildCommand;
        public ICommand ShowChildCommand
        {
            get
            {
                return _showChildCommand ?? (_showChildCommand = new MvxCommand(() => this.ShowViewModel<ChildViewModel>()));
            }
        }

        private ICommand _showModalCommand;
        public ICommand ShowModalCommand
        {
            get
            {
                return _showModalCommand ?? (_showModalCommand = new MvxCommand(() => this.ShowViewModel<ModalViewModel>()));
            }
        }

        private ICommand _showModalNavCommand;
        public ICommand ShowModalNavCommand
        {
            get
            {
                return _showModalNavCommand ?? (_showModalNavCommand = new MvxCommand(() => ShowViewModel<ModalNavViewModel>()));
            }
        }

        private ICommand _showTabsCommand;
        public ICommand ShowTabsCommand
        {
            get
            {
                return _showTabsCommand ?? (_showTabsCommand = new MvxCommand(() => ShowViewModel<TabsRootViewModel>()));
            }
        }

        private ICommand _showSplitCommand;
        public ICommand ShowSplitCommand
        {
            get
            {
                return _showSplitCommand ?? (_showSplitCommand = new MvxCommand(() => ShowViewModel<SplitRootViewModel>()));
            }
        }
    }
}
