using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Android.App;
using Android.Sax;
using Android.Widget;
using BestSellers;
using BestSellers.ViewModels;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Views;

namespace BestSellers.Droid.Views
{
    [Activity(Label = "Book View")]
    public class BookView : MvxActivity
    {
        public new BookViewModel ViewModel
        {
            get { return (BookViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_BookView);
            var list = new List<string>();
        }
    }
}