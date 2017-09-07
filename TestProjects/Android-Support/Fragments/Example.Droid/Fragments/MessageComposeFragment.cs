﻿using Android.OS;
using Android.Runtime;
using Android.Views;
using Example.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;

namespace Example.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(MessagesViewModel), Resource.Id.content_frame, true)]
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