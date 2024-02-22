using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using Playground.Core.ViewModels.Samples;

namespace Playground.Droid.Activities;

[MvxActivityPresentation]
[Activity(Theme = "@style/AppTheme")]
public sealed class ConvertersActivity : MvxActivity<ConvertersViewModel>
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_converters);
    }
}
