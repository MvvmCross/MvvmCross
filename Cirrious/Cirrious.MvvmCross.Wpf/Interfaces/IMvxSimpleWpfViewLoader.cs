using System.Windows;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Wpf.Interfaces
{
    public interface IMvxSimpleWpfViewLoader
    {
        FrameworkElement CreateView(MvxShowViewModelRequest request);
    }
}