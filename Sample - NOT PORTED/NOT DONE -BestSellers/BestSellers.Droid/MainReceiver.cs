using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;

using MonoCross.Navigation;
using MonoCross.Droid;

using BestSellers;

namespace BestSellers.Droid
{
    [BroadcastReceiver]
    [IntentFilter(new string[] { "MonoCross.MainReceiver.BestSellers" })]
    public class MainReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Android.Util.Log.Debug("MainReceiver", "OnReceive");

            // initialize app
            MXDroidContainer.Initialize(new BestSellers.App(), context.ApplicationContext);

            // initialize views
            MXDroidContainer.AddView<CategoryList>(typeof(Views.CategoryListView), ViewPerspective.Read);
            MXDroidContainer.AddView<BookList>(typeof(Views.BookListView), ViewPerspective.Read);
            MXDroidContainer.AddView<Book>(typeof(Views.BookView), ViewPerspective.Read);

            // navigate to first view
            MXDroidContainer.Navigate(null, MXContainer.Instance.App.NavigateOnLoad);
        }
    }
}