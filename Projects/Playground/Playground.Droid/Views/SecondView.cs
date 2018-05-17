using Android.Runtime;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Playground.Droid.ViewModels;
using StarWarsSample.Droid.ViewModels;

namespace Playground.Droid.Views
{
    [MvxFragmentPresentation(typeof(MainViewModel), Playground.Droid.Resource.Id.content_frame, true)]
    [Register(nameof(SecondView))]
    public class SecondView : BaseFragment<SecondViewModel>
    {
        protected override int FragmentId => Resource.Layout.SecondView;

    }
}
