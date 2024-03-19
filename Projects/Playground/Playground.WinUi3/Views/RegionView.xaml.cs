using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using MvvmCross.Platforms.WinUi.Presenters.Attributes;
using MvvmCross.Platforms.WinUi.Views;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels;
using Playground.Core.ViewModels.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Playground.WinUi3.Views
{
    [MvxViewFor(typeof(RegionViewModel))]
    [MvxRegionPresentation("PopupLocation")]
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegionView : RegionViewPage
    {
        public RegionView()
        {
            this.InitializeComponent();
        }
    }

    public abstract class RegionViewPage : MvxWindowsPage<RegionViewModel>;
}
