using Android.OS;
using Android.Runtime;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Example.Core.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using MvvmCross.Droid.Support.V7.Fragging.Fragments;

namespace Example.Droid.Fragments
{
    [MvxFragment(typeof(MessagesViewModel), Resource.Id.content_frame)]
    [Register("example.droid.fragments.MessageComposeFragment")]
    public class MessageComposeFragment : MvxFragment<ComposeMessageViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.fragment_compose_message, null);

            return view;
        }
    }
}