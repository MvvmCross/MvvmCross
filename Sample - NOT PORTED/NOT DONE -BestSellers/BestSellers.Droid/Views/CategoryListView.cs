using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using MonoCross.Navigation;
using MonoCross.Droid;

using BestSellers;

namespace BestSellers.Droid.Views
{
    [Activity(Label = "The New York Times Best Sellers")]
    public class CategoryListView : MXListActivityView<CategoryList>
    {
        public override void Render()
        {
            string[] categories = new string[Model.Count];

            for (int ii = 0; ii < Model.Count; ii++)
            {
                categories[ii] = Model[ii].ListName;
            }

            ListView.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, categories);
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);
            string url = string.Format(Model[position].ListNameEncoded);

            MXNavigationExtensions.Navigate(this, url);
        }
    }
}

