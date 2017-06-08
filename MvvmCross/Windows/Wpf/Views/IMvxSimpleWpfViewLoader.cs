// IMvxSimpleWpfViewLoader.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Windows;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Wpf.Views
{
    public interface IMvxSimpleWpfViewLoader
    {
        FrameworkElement CreateView(MvxViewModelRequest request);
    }
}