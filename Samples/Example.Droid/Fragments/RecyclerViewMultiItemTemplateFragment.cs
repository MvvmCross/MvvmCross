using Android.OS;
using Android.Runtime;
using Android.Views;
using Example.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using MvvmCross.Droid.Support.V7.Fragging.Fragments;

namespace Example.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("example.droid.fragments.RecyclerViewMultiItemTemplateFragment")]
    public class RecyclerViewMultiItemTemplateFragment : MvxFragment<RecyclerViewMultiItemTemplateViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.fragment_recyclerview_multiitemtemplate, null);
        }

    }
}