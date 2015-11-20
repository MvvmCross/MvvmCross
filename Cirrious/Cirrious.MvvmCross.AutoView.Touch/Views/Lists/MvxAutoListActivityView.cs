// MvxAutoListActivityView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.AutoView.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Touch.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.ViewModels;
using CrossUI.Core.Elements.Menu;
using UIKit;

namespace Cirrious.MvvmCross.AutoView.Touch.Views.Lists
{
    [MvxUnconventional]
    public class MvxAutoListActivityView
        : MvxTableViewController
          , IMvxTouchAutoView
    {
        private IParentMenu _parentMenu;
        private GeneralListLayout _list;

        public new MvxViewModel ViewModel
        {
            get { return (MvxViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _parentMenu = this.LoadMenu();

            _list = this.LoadList<GeneralListLayout>();
            var source = _list.InitializeSource(TableView);
            TableView.Source = source;
            TableView.ReloadData();

            if (_parentMenu != null)
            {
                NavigationItem.SetRightBarButtonItem(
                    new UIBarButtonItem(UIBarButtonSystemItem.Action,
                                        (sender, e) => { ShowActionMenu(); }),
                    false);
            }
        }

        private void ShowActionMenu()
        {
            this.ShowOptionsMenu(_parentMenu);
        }

        /*
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
         */
    }
}