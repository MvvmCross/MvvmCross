using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cirrious.MvvmCross.WinRT.Views;
using Tutorial.Core.ViewModels;
using Tutorial.UI.WinRT.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Tutorial.UI.WinRT.Views
{
    public class BaseMainMenuView
        : LayoutAwarePage
    {
        public new MainMenuViewModel ViewModel
        {
            get { return (MainMenuViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }

    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainMenuView
        : BaseMainMenuView
    {
        public MainMenuView()
        {
            this.InitializeComponent();
        }
    }
}
