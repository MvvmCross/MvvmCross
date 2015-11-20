// MvxWpfView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.ViewModels;
using System.Windows.Controls;

namespace Cirrious.MvvmCross.Wpf.Views
{
    public class MvxWpfView : UserControl, IMvxWpfView
    {
        private IMvxViewModel _viewModel;

        public IMvxViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                DataContext = value;
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