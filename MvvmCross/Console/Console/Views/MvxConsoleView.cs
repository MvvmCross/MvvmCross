// MvxConsoleView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace MvvmCross.Console.Views
{
    public class MvxConsoleView<T>
        : IMvxConsoleView
        where T : IMvxViewModel
    {
        public T ViewModel
        {
            get => (T) DataContext;
            set => DataContext = value;
        }

        public Type ViewModelType => typeof(T);
        public object DataContext { get; set; }

        IMvxViewModel IMvxView.ViewModel
        {
            get => (IMvxViewModel) DataContext;
            set => DataContext = (T) value;
        }

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
    }
}