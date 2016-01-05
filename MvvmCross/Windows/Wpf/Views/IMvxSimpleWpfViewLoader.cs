// IMvxSimpleWpfViewLoader.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Wpf.Views
{
    using System.Windows;

    using MvvmCross.Core.ViewModels;

    public interface IMvxSimpleWpfViewLoader
    {
        FrameworkElement CreateView(MvxViewModelRequest request);
    }
}