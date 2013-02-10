using Android.App;
using Android.Content;
using Android.Widget;
using Cirrious.Conference.Core.ViewModels;
using Cirrious.MvvmCross.Droid.Views;

namespace Cirrious.Conference.UI.Droid.Views
{
    [Activity(Label = "SqlBits")]
    public class HomeView : BaseTabbedView<HomeViewModel>
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_Home);

            TabHost.TabSpec spec;     // Resusable TabSpec for each tab
            Intent intent;            // Reusable Intent for each tab

            // Initialize a TabSpec for each tab and add it to the TabHost
            spec = TabHost.NewTabSpec("welcome");
            spec.SetIndicator(this.GetText("Welcome"), Resources.GetDrawable(Resource.Drawable.Tab_Welcome));
            spec.SetContent(this.CreateIntentFor(ViewModel.Welcome));
            TabHost.AddTab(spec);

            spec = TabHost.NewTabSpec("sessions");
            spec.SetIndicator(this.GetText("Sessions"), Resources.GetDrawable(Resource.Drawable.Tab_Sessions));
            spec.SetContent(this.CreateIntentFor(ViewModel.Sessions));
            TabHost.AddTab(spec);
            
            spec = TabHost.NewTabSpec("favorites");
            spec.SetIndicator(this.GetText("Favorites"), Resources.GetDrawable(Resource.Drawable.Tab_Favorites));
            spec.SetContent(this.CreateIntentFor(ViewModel.Favorites));
            TabHost.AddTab(spec);

            spec = TabHost.NewTabSpec("tweets");
            spec.SetIndicator(this.GetText("Tweets"), Resources.GetDrawable(Resource.Drawable.Tab_Tweets));
            spec.SetContent(this.CreateIntentFor(ViewModel.Twitter));
            TabHost.AddTab(spec);
        }
    }
}