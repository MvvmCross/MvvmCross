using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.GoogleMaps;
using Android.Graphics.Drawables;
using Android.Widget;
using Cirrious.Conference.Core.ViewModels;

namespace Cirrious.Conference.UI.Droid.Views
{
    [Activity(Label = "SqlBits")]
    public class MapsView : BaseMapView<MapViewModel>
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_Map);

            var map = FindViewById<MapView>(Resource.Id.map);

            map.Clickable = true;
            map.Traffic = false;
            map.Satellite = true;

            map.SetBuiltInZoomControls(true);
            map.Controller.SetZoom(15);
            var point = new GeoPoint((int) (ViewModel.Latitude*1e6), (int) (ViewModel.Longitude*1e6));
            map.Controller.SetCenter(point);

            AddPinOverlay(map, point);
        }

        void AddPinOverlay(MapView map, GeoPoint point)
        {
            var pin = Resources.GetDrawable(Resource.Drawable.Icon);
            var pinOverlay = new MapPinOverlay(pin, point);
            map.Overlays.Add(pinOverlay);
        }

        class MapPinOverlay : ItemizedOverlay
        {
            List<OverlayItem> pins;

            public MapPinOverlay(Drawable pin, GeoPoint point)
                : base(pin)
            {
                // populate some sample location data for the overlay items
                pins = new List<OverlayItem>{
                    new OverlayItem (point, null, null),
                };

                BoundCenterBottom(pin);
                Populate();
            }

            protected override Java.Lang.Object CreateItem(int i)
            {
                var item = pins[i];
                return item;
            }

            public override int Size()
            {
                return pins.Count;
            }
        }
    }
}