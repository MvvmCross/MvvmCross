// MvxAutoListActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Android.Views;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.AutoView.Droid.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces;
using Cirrious.MvvmCross.AutoView.ExtensionMethods;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using CrossUI.Core.Elements.Menu;

namespace Cirrious.MvvmCross.AutoView.Droid.Views.Lists
{
    [Activity(Name = "cirrious.mvvmcross.autoview.droid.views.lists.MvxAutoListActivity")]
    [MvxUnconventional]
    public class MvxAutoListActivity
        : MvxActivity
          , IMvxAndroidAutoView
    {
        private IParentMenu _parentMenu;
        private GeneralListLayout _list;

        public new MvxViewModel ViewModel
        {
            get { return (MvxViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            _parentMenu = this.LoadMenu();
            _list = this.LoadList<GeneralListLayout>();

            using (
                new MvxBindingContextStackRegistration<IMvxAndroidBindingContext>((IMvxAndroidBindingContext) BindingContext)
                )
            {
                var listView = _list.InitializeListView(this);
                this.SetContentView(listView);
            }
        }

#warning consider making static - and moving to extension method?
        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            return this.CreateOptionsMenu(_parentMenu, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (_parentMenu.ProcessMenuItemSelected(item))
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}