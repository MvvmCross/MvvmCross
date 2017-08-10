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
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    public class TestCView : MvxFragment<TestCViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_testviewc, null);

            var text = view.FindViewById<TextView>(Resource.Id.text);
            text.Text = "C";

            var layout = view.FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            layout.SetBackgroundColor(Color.Red);

            return view;
        }
    }
}