#region Copyright

// <copyright file="MvxAutoListActivityView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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