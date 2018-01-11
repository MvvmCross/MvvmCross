using System;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Playground.Core.ViewModels;

namespace Playground.Droid.Views
{
    [MvxDialogFragmentPresentation]
    [Register(nameof(ModalView))]
    public class ModalView : MvxDialogFragment<ModalViewModel>
    {
        public ModalView()
        {
        }

        protected ModalView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.ChildView, null);

            return view;
        }

        public override void OnPause()
        {
            var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            var activity = top.Activity;

            base.OnPause();
        }
    }
}