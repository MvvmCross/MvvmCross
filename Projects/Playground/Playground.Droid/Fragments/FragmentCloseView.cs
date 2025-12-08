using System.Diagnostics.CodeAnalysis;
using Android.Views;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;
using Playground.Core.ViewModels;
using Playground.Core.ViewModels.Navigation;

namespace Playground.Droid.Fragments
{
    //[MvxFragmentPresentation(typeof(RootViewModel), Resource.Id.content_frame, true)]
    [MvxFragmentPresentation(typeof(RootViewModel), Resource.Id.content_frame, true, popBackStackImmediateName: null, popBackStackImmediateFlag: MvxPopBackStack.None)]
    [RequiresUnreferencedCode("Uses MvxBindings which require unreferenced code")]
    internal sealed class FragmentCloseView : MvxFragment<FragmentCloseViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            return this.BindingInflate(Resource.Layout.FragmnetCloseView, container, false);
        }
    }
}
