using System;
using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Simple;

namespace DroidAutoComplete
{
    [Activity(Label = "Droid AutoComplete - Books!", MainLauncher = true, Icon = "@drawable/icon")]
    public sealed class BooksActivity : MvxSimpleBindingActivity, IMainThreadRunner
    {
        public BooksActivity()
        {
            DataContext = new BooksViewModel(this);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
        }

        #region Implementation of IMainThreadRunner

        public void Run(Action action)
        {
            this.RunOnUiThread(action);
        }

        #endregion
    }
}

