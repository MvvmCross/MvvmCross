using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Xamarin.Forms.Platform.WinRT;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Forms.Presenter.Windows81;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Example.W81
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// 
    /// Note: Extensively modified for MvvMCross-Forms
    /// </summary>
    public sealed partial class MainPage : WindowsPage
    {
        public MainPage()
        {
            // WindowsPage initialization
            InitializeComponent();

            // Start MvvMCross
            var start = Mvx.Resolve<MvvmCross.Core.ViewModels.IMvxAppStart>();
            start.Start();

            // Locate the MvvMCross-Forms Presenter
            var presenter = Mvx.Resolve<IMvxViewPresenter>() as MvxFormsWindows81PagePresenter;


            // Xamarin.Forms.Platform.WinRT.dll Loads the Xamarin Form found at presenter.MvxFormsApp
            LoadApplication(presenter.MvxFormsApp);

            LayoutUpdated += MainPage_LayoutUpdated;
        }

        private void MainPage_LayoutUpdated(object sender, object e)
        {
            try
            {
                this.BottomAppBar.IsOpen = true;
                this.BottomAppBar.Visibility = Visibility.Visible;
                this.BottomAppBar.IsSticky = true;
            }
            catch (Exception ex) { }
        }
    }
}
