using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Droid.Support.V4;
using RoutingExample.Core.ViewModels;

namespace RoutingExample.Droid
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true, Resource.Animation.abc_fade_in,
                Resource.Animation.abc_fade_out,
                Resource.Animation.abc_fade_in,
                Resource.Animation.abc_fade_out)]
    public class TestBView : MvxFragment<TestBViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.Test, null);

            var text = view.FindViewById<TextView>(Resource.Id.text);
            text.Text = "B";

            var layout = view.FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            layout.SetBackgroundColor(Color.Green);

            return view;
        }
    }
}