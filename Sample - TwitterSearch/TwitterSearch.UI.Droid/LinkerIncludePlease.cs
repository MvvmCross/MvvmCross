using Android.Views;
using Android.Widget;

namespace TwitterSearch.UI.Droid
{
    public class LinkerIncludePlease
    {
        private void IncludeClick(View view)
        {
            view.Click += (s, e) => { };
        }

        private void IncludeVisibility(View view)
        {
            view.Visibility = view.Visibility + 1;            
        }

        private void IncludeRelativeLayout(RelativeLayout relative)
        {
            relative.Visibility = ViewStates.Visible;
        }
    }
}