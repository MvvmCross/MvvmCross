using Android.Runtime;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Playground.Droid.ViewModels;

namespace Playground.Droid.Views
{
    [MvxFragmentPresentation(typeof(MainViewModel), Playground.Droid.Resource.Id.content_frame, false)]
    [Register(nameof(FirstView))]
    public class FirstView : BaseFragment<FirstViewModel>
    {
        protected override int FragmentId => Resource.Layout.FirstView;
    }
}
