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
using Microsoft.UI.Windowing;
using MvvmCross.Platforms.WinUi.Presenters.Attributes;
using MvvmCross.Platforms.WinUi.Presenters.Models;
using MvvmCross.Platforms.WinUi.Presenters.Utils;
using MvvmCross.Platforms.WinUi.Views;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Playground.WinUi3.Views
{
    [MvxViewFor(typeof(NewWindowViewModel))]
    [MvxNewWindowPresentation]
    public sealed partial class NewWindow : NewWindowPage, INeedWindow
    {
        public NewWindow()
        {
            this.InitializeComponent();
            this.PopupLocation.Navigate(typeof(BlankPage));
        }

        public void SetWindow(Window window, AppWindow appWindow)
        {
            this.AppWindow = appWindow;
            AppWindowUtils.SetTitleBar(appWindow, "Hello new window");
        }

        public AppWindow AppWindow { get; set; }

        public bool CanClose()
        {
            return true;
        }
    }


    public abstract class NewWindowPage : MvxWindowsPage<NewWindowViewModel>
    {
    }
}
