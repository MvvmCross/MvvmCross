// MvxAutoListActivityView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.AutoView.Touch.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Touch.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Views.Attributes;
using CrossUI.Core.Elements.Menu;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.AutoView.Touch.Views.Lists
{
    [MvxUnconventionalView]
    public class MvxAutoListActivityView
        : MvxBindingTouchTableViewController<MvxViewModel>
          , IMvxTouchAutoView<MvxViewModel>
    {
        private IParentMenu _parentMenu;
        private GeneralListLayout _list;

        public MvxAutoListActivityView(MvxShowViewModelRequest request)
            : base(request)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _parentMenu = this.LoadMenu();

            _list = this.LoadList();
            var source = _list.InitialiseSource(TableView);
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

        public void RegisterBinding(IMvxUpdateableBinding binding)
        {
            Bindings.Add(binding);
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