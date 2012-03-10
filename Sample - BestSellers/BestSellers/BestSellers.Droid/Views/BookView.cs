using System;
using System.Collections.Specialized;
using Android.App;
using Android.Sax;
using BestSellers;
using BestSellers.ViewModels;
using Cirrious.MvvmCross.Binding.Android.Views;

namespace BestSellers.Droid.Views
{
    [Activity(Label = "Book View")]
    public class BookView : MvxBindingActivityView<BookViewModel>
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_BookView);
        }
    }
}