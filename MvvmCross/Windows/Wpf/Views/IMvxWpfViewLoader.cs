// IMvxWpfViewLoader.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Windows;
using MvvmCross.Core.ViewModels;
using System;

namespace MvvmCross.Wpf.Views
{
    public interface IMvxWpfViewLoader
    {
        FrameworkElement CreateView(MvxViewModelRequest request);

        FrameworkElement CreateView(Type viewType);
    }
}