// MvxAutoListActivityView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.AutoView.iOS.Interfaces;

namespace MvvmCross.AutoView.iOS.Views.Lists
{
    using CrossUI.Core.Elements.Menu;

    using MvvmCross.AutoView.ExtensionMethods;
    using iOS.ExtensionMethods;
    using iOS.Interfaces;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Platform.IoC;
    using MvvmCross.iOS.Views;

    using UIKit;

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

            this._parentMenu = this.LoadMenu();

            this._list = this.LoadList<GeneralListLayout>();
            var source = this._list.InitializeSource(TableView);
            TableView.Source = source;
            TableView.ReloadData();

            if (this._parentMenu != null)
            {
                NavigationItem.SetRightBarButtonItem(
                    new UIBarButtonItem(UIBarButtonSystemItem.Action,
                                        (sender, e) => { this.ShowActionMenu(); }),
                    false);
            }
        }

        private void ShowActionMenu()
        {
            this.ShowOptionsMenu(this._parentMenu);
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