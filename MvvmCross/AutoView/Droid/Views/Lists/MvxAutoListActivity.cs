// MvxAutoListActivity.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Droid.Views.Lists
{
    using Android.App;
    using Android.Views;

    using CrossUI.Core.Elements.Menu;

    using MvvmCross.AutoView.Droid.ExtensionMethods;
    using MvvmCross.AutoView.Droid.Interfaces;
    using MvvmCross.AutoView.ExtensionMethods;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Droid.BindingContext;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Droid.Views;
    using MvvmCross.Platform.IoC;

    [Activity(Name = "mvvmcross.autoview.droid.views.lists.MvxAutoListActivity")]
    [MvxUnconventional]
    public class MvxAutoListActivity
        : MvxActivity
          , IMvxAndroidAutoView
    {
        private IParentMenu _parentMenu;
        private GeneralListLayout _list;

        public new MvxViewModel ViewModel
        {
            get { return (MvxViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            this._parentMenu = this.LoadMenu();
            this._list = this.LoadList<GeneralListLayout>();

            using (
                new MvxBindingContextStackRegistration<IMvxAndroidBindingContext>((IMvxAndroidBindingContext)BindingContext)
                )
            {
                var listView = this._list.InitializeListView(this);
                this.SetContentView(listView);
            }
        }

#warning consider making static - and moving to extension method?

        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            return this.CreateOptionsMenu(this._parentMenu, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (this._parentMenu.ProcessMenuItemSelected(item))
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}