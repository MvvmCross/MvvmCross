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
    [MvxFragment(typeof(SecondHostViewModel), Resource.Id.content_frame, true)]
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

