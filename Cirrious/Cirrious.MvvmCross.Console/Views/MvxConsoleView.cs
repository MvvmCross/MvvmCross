﻿// MvxConsoleView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Console.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Console.Views
{
    public class MvxConsoleView<T>
        : IMvxConsoleView
          , IMvxConsumer
        where T : IMvxViewModel
    {
        public object DataContext { get; set; }

        public T ViewModel
        {
            get { return (T) DataContext; }
            set { DataContext = value; }
        }

        IMvxViewModel IMvxView.ViewModel
        {
            get { return (IMvxViewModel) DataContext; }
            set { DataContext = (T) value; }
        }

        public Type ViewModelType
        {
            get { return typeof (T); }
        }

#warning Need to get the Console implementation level with the mobile implementations again - HackSetViewModel is a Hack
        public void HackSetViewModel(object viewModel)
        {
            ViewModel = (T) viewModel;
            OnViewModelChanged();
        }

        public virtual bool HandleInput(string input)
        {
            return false;
        }

        protected virtual void OnViewModelChanged()
        {
        }

        public bool IsVisible
        {
            get { return this.GetService<IMvxConsoleCurrentView>().CurrentView == this; }
        }
    }
}