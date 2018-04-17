using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;
using RoutingExample.Core.ViewModels;

namespace RoutingExample.Droid
{
    [MvxFragmentPresentation(typeof(SecondHostViewModel), Resource.Id.nested_content_frame, true)]
    [Register(nameof(FragmentNested))]
    public class FragmentNested : MvxFragment<ViewModelNested>
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.fragment_a, null);

            return view;
        }
    }
}