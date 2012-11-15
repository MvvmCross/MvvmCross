using System.Collections.Generic;
using Android.App;
using Android.Views;
using Cirrious.MvvmCross.AutoView.Droid.Builders;
using Cirrious.MvvmCross.AutoView.Droid.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Plugins.Json;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views.Attributes;
using Foobar.Dialog.Core.Descriptions;
using Foobar.Dialog.Core.Lists;
using Foobar.Dialog.Core.Menus;

namespace Cirrious.MvvmCross.AutoView.Droid.Views.Lists
{
    [Activity]
    [MvxUnconventionalView]
    public class MvxAutoListActivityView
        : MvxBindingActivityView<MvxViewModel>
        , IMvxAndroidAutoView<MvxViewModel>
    {
        private IParentMenu _parentMenu;
        private GeneralListLayout _list;

        protected override void OnViewModelSet()
        {
            _parentMenu = this.LoadMenu();

            _list = this.LoadList();
            var listView = _list.InitialiseListView(this);
            this.SetContentView(listView);
#warning RegisterBindingsFor needs thinking about here - what binding is stored/released where?
            RegisterBindingsFor(listView);
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