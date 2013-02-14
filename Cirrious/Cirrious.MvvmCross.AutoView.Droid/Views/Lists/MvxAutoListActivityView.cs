// MvxAutoListActivityView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Android.Views;
using Cirrious.MvvmCross.AutoView.Droid.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views.Attributes;
using CrossUI.Core.Elements.Menu;

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