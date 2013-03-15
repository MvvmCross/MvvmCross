using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Mime;
using Cirrious.MvvmCross.Binding.Touch;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.Conference.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Cirrious.Conference.UI.Touch
{
    public partial class MapView : MvxViewController
    {
        public MapView()
            : base("MapView", null)
        {
        }

		public new MapViewModel ViewModel {
			get { return (MapViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var map = new MKMapView(new RectangleF(20, 166, 280, 192));
            this.View.Add(map);
            map.ShowsUserLocation = true;
            map.MapType = MKMapType.Standard;
            map.Delegate = new MapViewDelegate();

            var location = new CLLocationCoordinate2D(ViewModel.Latitude, ViewModel.Longitude);
            //map.SetCenterCoordinate(location, true);
            map.SetRegion(new MKCoordinateRegion(location, new MKCoordinateSpan(0.1, 0.1)), true);

            var annotation = new MyAnnotation(
                                  location
                                  , ViewModel.SharedTextSource.GetText("AppTitle")
                                  , ViewModel.Name);
            map.AddAnnotationObject(annotation);


            Button1.SetImage(UIImage.FromFile("ConfResources/Images/appbar.link.png"), UIControlState.Normal);
            Button2.SetImage(UIImage.FromFile("ConfResources/Images/appbar.phone.png"), UIControlState.Normal);
            Button3.SetImage(UIImage.FromFile("ConfResources/Images/appbar.feature.email.rest.png"), UIControlState.Normal);

            this.AddBindings(new Dictionary<object, string>()
		                         {
                                     {Label1,"Text Name"}, 
                                     {Button1,"Title Address"},
                                     {Button2,"Title Phone"},
                                     {Button3,"Title Email"},
		                         });

            this.AddBindings(new Dictionary<object, string>()
		                         {
                                     {Button1,"TouchUpInside WebPageCommand"},
                                     {Button2,"TouchUpInside PhoneCommand"},
                                     {Button3,"TouchUpInside EmailCommand"},
		                         });

            NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Tweet", UIBarButtonItemStyle.Bordered, (sender, e) => ViewModel.DoShareGeneral()), false);
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();

            // Clear any references to subviews of the main view in order to
            // allow the Garbage Collector to collect them sooner.
            //
            // e.g. myOutlet.Dispose (); myOutlet = null;

            ReleaseDesignerOutlets();
        }

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            // Return true for supported orientations
            return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
        }

        public class MapViewDelegate : MKMapViewDelegate
        {
            public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, NSObject annotation)
            {
                Console.WriteLine("attempt to get view for MKAnnotation " + annotation);
                try
                {
                    var anv = mapView.DequeueReusableAnnotation("thislocation");
                    if (anv == null)
                    {
                        Console.WriteLine("creating new MKAnnotationView");
                        var pinanv = new MKPinAnnotationView(annotation, "thislocation");
                        pinanv.AnimatesDrop = true;
                        pinanv.PinColor = MKPinAnnotationColor.Green;
                        pinanv.CanShowCallout = true;
                        anv = pinanv;
                    }
                    else
                    {
                        anv.Annotation = annotation;
                    }
                    return anv;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("GetViewForAnnotation Exception " + ex);
                    return null;
                }
            }
        }

        public class MyAnnotation : MKAnnotation
        {
            private CLLocationCoordinate2D _coordinate;
            private string _title, _subtitle;
            public override CLLocationCoordinate2D Coordinate
            {
                get
                {
                    return _coordinate;
                }
                set { _coordinate = value; }
            }
            public override string Title
            {
                get
                {
                    return _title;
                }
            }
            public override string Subtitle
            {
                get
                {
                    return _subtitle;
                }
            }
            /// <summary>
            /// custom constructor
            /// </summary>
            public MyAnnotation(CLLocationCoordinate2D coord, string t, string s)
                : base()
            {
                _coordinate = coord;
                _title = t;
                _subtitle = s;
            }
        }
    }
}

