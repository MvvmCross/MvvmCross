using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Cirrious.Conference.Core.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Controls;

namespace Cirrious.Conference.UI.WP7.Views
{
    public abstract class BaseSplashScreenView
        : BaseView<SplashScreenViewModel>
    {
    }

    public partial class SplashScreenView : BaseSplashScreenView
    {
        public SplashScreenView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var storyboard = (Storyboard)Resources["StartStoryboard"];
            storyboard.Begin();
            storyboard.Completed += (sender, args) => ViewModel.SplashScreenComplete = true;
        }
    }
}