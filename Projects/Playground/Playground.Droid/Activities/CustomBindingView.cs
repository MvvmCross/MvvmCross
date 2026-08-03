using System.Diagnostics.CodeAnalysis;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

namespace Playground.Droid.Activities
{
    [MvxActivityPresentation]
    [Activity(Label = "View for CustomBindingViewModel", Theme = "@style/AppTheme")]
    [RequiresUnreferencedCode("Uses MvxBindings which require unreferenced code")]
    public sealed class CustomBindingView : MvxActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CustomBindingView);
        }
    }
}
