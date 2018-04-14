﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Wpf.Views
{
    public class MvxWindow : Window, IMvxWindow, IMvxWpfView, IDisposable
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

        public string Identifier { get; set; }

        public MvxWindow()
        {
            Unloaded += MvxWindow_Unloaded;
            Loaded += MvxWindow_Loaded;
            Initialized += MvxWindow_Initialized;
        }

        private void MvxWindow_Initialized(object sender, EventArgs e)
        {
            if (this == Application.Current.MainWindow)
            {
                (Application.Current as MvxApplication).ApplicationInitialized();
            }
        }

        private void MvxWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.ViewDisappearing();
            ViewModel?.ViewDisappeared();
        }

        private void MvxWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.ViewAppearing();
            ViewModel?.ViewAppeared();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MvxWindow()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Unloaded -= MvxWindow_Unloaded;
                Loaded -= MvxWindow_Loaded;
            }
        }
    }

    public class MvxWindow<TViewModel>
        : MvxWindow
          , IMvxWpfView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
