using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Tutorial.Core.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Tutorial.UI.WindowsMetro.Views
{
    public class BaseMainView
        : Cirrious.MvvmCross.WinRT.Views.MvxWinRTPage
    {
        public new MainMenuViewModel ViewModel
        {
            get { return (MainMenuViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }

    public sealed partial class MainView : BaseMainView
    {
        public MainView()
        {
            this.InitializeComponent();
        }
    }
}
