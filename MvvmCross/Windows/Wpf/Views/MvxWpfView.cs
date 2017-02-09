// MvxWpfView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Wpf.Views
{
    using System.Windows.Controls;

    using MvvmCross.Core.ViewModels;
    using System;

    public class MvxWpfView : UserControl, IMvxWpfView, IDisposable
    {
        private IMvxViewModel _viewModel;

        public IMvxViewModel ViewModel
        {
            get { return this._viewModel; }
            set
            {
                this._viewModel = value;
                this.DataContext = value;
            }
        }

        public MvxWpfView()
        {
            this.Unloaded += MvxWpfView_Unloaded;
            this.Loaded += MvxWpfView_Loaded;
        }

        private void MvxWpfView_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel?.Disappearing();
            ViewModel?.Disappeared();
        }

        private void MvxWpfView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel?.Appearing();
            ViewModel?.Appeared();
        }

        public void Dispose()
        {
            this.Unloaded -= MvxWpfView_Unloaded;
            this.Loaded -= MvxWpfView_Loaded;
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