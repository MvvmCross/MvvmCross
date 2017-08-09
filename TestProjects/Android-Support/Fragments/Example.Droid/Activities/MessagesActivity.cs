using Android.App;
using Android.Content.PM;
using Android.OS;
using Example.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace Example.Droid.Activities
{
    [Activity(
       Label = "Examples",
       Theme = "@style/AppTheme",
       LaunchMode = LaunchMode.SingleTop,
       Name = "example.droid.activities.AnotherFragmentHostActivity"
       )]
    public class MessagesActivity : MvxAppCompatActivity<MessagesViewModel>
    {
        /*public override IFragmentCacheConfiguration BuildFragmentCacheConfiguration()
        {
            return new MessagesFragmentCacheConfigurationCustomFragmentInfo();
        }*/

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_messages);
        }
    }
}