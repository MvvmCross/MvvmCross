﻿using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class SplitMasterViewModel : MvxViewModel
    {
        private ICommand _openDetailCommand;
        public ICommand OpenDetailCommand
        {
            get
            {
                return _openDetailCommand ?? (_openDetailCommand = new MvxCommand(() => ShowViewModel<SplitDetailViewModel>()));
            }
        }

        private ICommand _openDetailNavCommand;
        public ICommand OpenDetailNavCommand
        {
            get
            {
                return _openDetailNavCommand ?? (_openDetailNavCommand = new MvxCommand(() => ShowViewModel<SplitDetailNavViewModel>()));
            }
        }

        private ICommand _showRootViewModel;
        public ICommand ShowRootViewModel
        {
            get
            {
                return _showRootViewModel ?? (_showRootViewModel = new MvxCommand(() => ShowViewModel<RootViewModel>()));
            }
        }
    }
}
