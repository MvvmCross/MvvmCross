using System;
using MvvmCross.Droid.Views;
using TestProject.Core.ViewModels;

namespace TestProject.Droid
{
    public class MainViewActivity : MvxActivity<MainViewModel>
    {
        public MainViewActivity()
        {
            
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.MainView);
        }
    }
}
