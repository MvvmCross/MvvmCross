using Android.Views;
using AndroidX.Activity;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;
using Playground.Core.ViewModels;
using Playground.Core.ViewModels.Navigation;

namespace Playground.Droid.Fragments;

[MvxFragmentPresentation(typeof(RootViewModel), Resource.Id.content_frame, true,
    Resource.Animation.abc_fade_in,
    Resource.Animation.abc_fade_out,
    Resource.Animation.abc_fade_in,
    Resource.Animation.abc_fade_out)]
public sealed class ChildWithResultFragment : MvxFragment<ChildWithResultViewModel>
{
    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        _ = base.OnCreateView(inflater, container, savedInstanceState);

        var view = this.BindingInflate(Resource.Layout.fragment_childwithresult, container, false);
        return view;
    }

    public override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        var activity = RequireActivity();
        activity.OnBackPressedDispatcher
            .AddCallback(this, new BackPressedCallback(true,
                () => ViewModel!.CloseCommand.Execute(null)));
    }
}

public sealed class BackPressedCallback(
        bool enabled,
        Action onBackPressedAction)
    : OnBackPressedCallback(enabled)
{
    public override void HandleOnBackPressed() => onBackPressedAction.Invoke();
}
