
using Android.App;
using Android.Content;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Views;
using Tutorial.Core.ViewModels.Lessons;

namespace Tutorial.UI.Droid.Views.Lessons
{
    [Activity]
    public class CompositeView
        : MvxTabActivity
    {
        public new CompositeViewModel ViewModel
        {
            get { return (CompositeViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_CompositeView);

            TabHost.TabSpec spec;     // Resusable TabSpec for each tab
            Intent intent;            // Reusable Intent for each tab

            // Initialize a TabSpec for each tab and add it to the TabHost
            spec = TabHost.NewTabSpec("text");
            spec.SetIndicator("Text", Resources.GetDrawable(Resource.Drawable.tab_speakers));
            spec.SetContent(this.CreateIntentFor(ViewModel.Text));
            TabHost.AddTab(spec);

            spec = TabHost.NewTabSpec("tip");
            spec.SetIndicator("Tip", Resources.GetDrawable(Resource.Drawable.tab_speakers));
            spec.SetContent(this.CreateIntentFor(ViewModel.Tip));
            TabHost.AddTab(spec);

            spec = TabHost.NewTabSpec("pull");
            spec.SetIndicator("Pull", Resources.GetDrawable(Resource.Drawable.tab_speakers));
            spec.SetContent(this.CreateIntentFor(ViewModel.Pull));
            TabHost.AddTab(spec);

        }
    }
}