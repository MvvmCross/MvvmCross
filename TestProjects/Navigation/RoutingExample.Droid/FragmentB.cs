using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Droid.Support.V4;
using RoutingExample.Core.ViewModels;

namespace RoutingExample.Droid
{
    [MvxFragment(typeof(SecondHostViewModel), Resource.Id.content_frame, true)]
    [Register(nameof(FragmentB))]
    public class FragmentB : MvxFragment<ViewModelB>
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.fragment_b, null);

            return view;
        }
    }
}