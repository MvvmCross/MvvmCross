using System;
using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;

namespace Example.Core.ViewModels
{
    public class StartViewModel 
		: MvxViewModel
    {

        private MvxCommand _listCommand;

        public ICommand ListCommand {
            get {
                _listCommand = _listCommand ?? new MvxCommand (() => ShowViewModel<ListViewModel> ());
                return _listCommand;
            }
        }
    }
}

