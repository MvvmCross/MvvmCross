﻿// MvxWpfView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MvvmCross.Wpf.Views
{
    public class MvxWpfView : UserControl, IMvxWpfView, IDisposable
    {
        private IMvxViewModel _viewModel;

        public IMvxViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                DataContext = value;
            }
        }

        public MvxWpfView()
        {
            Unloaded += MvxWpfView_Unloaded;
            Loaded += MvxWpfView_Loaded;
        }

        private void MvxWpfView_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.ViewDisappearing();
            ViewModel?.ViewDisappeared();
        }

        private void MvxWpfView_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.ViewAppearing();
            ViewModel?.ViewAppeared();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MvxWpfView()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Unloaded -= MvxWpfView_Unloaded;
                Loaded -= MvxWpfView_Loaded;
            }
        }
    }

    public class MvxWpfView<TViewModel>
        : MvxWpfView
          , IMvxWpfView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}