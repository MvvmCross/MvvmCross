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

namespace Cirrious.Conference.UI.WP7.Views
{
    public abstract class BaseMapView : BaseView<MapViewModel>
    {        
    }

    public partial class MapView : BaseMapView
    {
        public MapView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                SetupMap();
            }
        }

        private void SetupMap()
        {
            Map.CredentialsProvider = new ApplicationIdCredentialsProvider(Private.BingKey);
            Map.ZoomLevel = 14;
            var hotelLocation = new GeoCoordinate(ViewModel.Latitude, ViewModel.Longitude);
            Map.Center = hotelLocation;
            var layer = new MapLayer();
            Map.Children.Add(layer);
            var pin = new Pushpin() {Content = "*"};
            ToolTipService.SetToolTip(pin, ViewModel.Name);
            layer.AddChild(pin, hotelLocation);
        }
    }
}