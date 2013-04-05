using System;
using System.Collections.Generic;
using System.Device.Location;
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
using Cirrious.Conference.Core.ViewModels.SessionLists;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;

namespace Cirrious.Conference.UI.WP7.Views
{
    public partial class SessionView : BaseSessionView
    {
        public SessionView()
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

        private void ApplicationBarTwitterButtonClick(object sender, EventArgs e)
        {
            ViewModel.ShareCommand.Execute(null);
        }
    }

    public abstract class BaseSessionView : BaseView<SessionViewModel>
    {
    }
}