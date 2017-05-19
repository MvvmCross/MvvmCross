using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MvvmCross.Droid.Views;
using RoutingExample.Core.ViewModels;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;

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

