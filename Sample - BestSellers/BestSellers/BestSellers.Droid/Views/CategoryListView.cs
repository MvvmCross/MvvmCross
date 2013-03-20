using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using BestSellers.ViewModels;
using Cirrious.MvvmCross.Binding.Droid.Views;

using BestSellers;
using Cirrious.MvvmCross.Droid.Views;

namespace BestSellers.Droid.Views
{
    [Activity(Label = "The New York Times Best Sellers")]
    public class CategoryListView : MvxActivity
    {
        public new CategoryListViewModel ViewModel
        {
            get { return (CategoryListViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_CategoryListView);
        }
    }
}

