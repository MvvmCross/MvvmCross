using System.Diagnostics.CodeAnalysis;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using Playground.Core.ViewModels;

namespace Playground.Droid.Activities
{
    [MvxActivityPresentation]
    [Activity(Theme = "@style/AppTheme")]
    [RequiresUnreferencedCode("Uses MvxBindings which require unreferenced code")]
    public sealed class CollectionView : MvxActivity<CollectionViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CollectionView);
        }
    }
}
