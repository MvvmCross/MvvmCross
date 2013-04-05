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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cirrious.Conference.Core.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Cirrious.Conference.UI.WP7.Views
{
    public partial class HomeView 
        : BaseHomeView
    {
        public HomeView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New)
                UpdateApplicationBar();
        }

        // TODO - this is horrid - we should really use a bindable app bar here! 
        private void UpdateApplicationBar()
        {
            ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).Text = ViewModel.SharedTextSource.GetText("Share");
        }


        private void PanoramaSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            var panorama = ((Panorama) sender);
            var index = panorama.SelectedIndex;
            if (index < 0)
                return;
            var panoramaItem = (PanoramaItem)panorama.Items[index];
            this.ApplicationBar.IsVisible = (((string)panoramaItem.Tag) == "AppBar");
        }

        private void ApplicationBarTwitterButtonClick(object sender, EventArgs e)
        {
            ViewModel.ShareGeneralCommand.Execute(null);
        }
    }

    public abstract class BaseHomeView : BaseView<HomeViewModel>
    {
    }
}