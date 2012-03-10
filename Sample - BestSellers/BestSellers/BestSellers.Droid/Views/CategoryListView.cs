using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using BestSellers.ViewModels;
using Cirrious.MvvmCross.Binding.Android.Views;

using BestSellers;

namespace BestSellers.Droid.Views
{
    [Activity(Label = "The New York Times Best Sellers")]
    public class CategoryListView : MvxBindingActivityView<CategoryListViewModel>
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_CategoryListView);
        }
    }
}

