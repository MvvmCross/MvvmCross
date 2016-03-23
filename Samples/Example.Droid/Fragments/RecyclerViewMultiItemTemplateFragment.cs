using Android.Runtime;
using Example.Core.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;

namespace Example.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("example.droid.fragments.RecyclerViewMultiItemTemplateFragment")]
    public class RecyclerViewMultiItemTemplateFragment : BaseFragment<RecyclerViewMultiItemTemplateViewModel>
    {
         

        protected override int FragmentId => Resource.Layout.fragment_recyclerview_multiitemtemplate;
    }
}