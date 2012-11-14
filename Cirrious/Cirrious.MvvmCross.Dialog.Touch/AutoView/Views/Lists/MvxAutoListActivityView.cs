using System.Collections.Generic;
using Cirrious.MvvmCross.AutoView.Droid.Builders;
using Cirrious.MvvmCross.AutoView.Droid.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Plugins.Json;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Views.Attributes;
using Foobar.Dialog.Core.Descriptions;
using Foobar.Dialog.Core.Lists;
using Foobar.Dialog.Core.Menus;

namespace Cirrious.MvvmCross.AutoView.Droid.Views.Lists
{
    [MvxUnconventionalView]
    public class MvxAutoListActivityView
        : MvxBindingTouchTableViewController<MvxViewModel>
        , IMvxTouchAutoView<MvxViewModel>
    {
        private IParentMenu _parentMenu;
        //private GeneralListLayout _list;

        protected MvxAutoListActivityView(MvxShowViewModelRequest request) : base(request)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            /*
            _parentMenu = this.LoadMenu();

            
            _list = this.LoadList();
            var listView = _list.InitialiseListView(this);
            this.SetContentView(listView);
            RegisterBindingsFor(listView);
            */
        }

        public void RegisterBinding(IMvxUpdateableBinding binding)
        {
#warning            // TODO - what to do with these bindings !
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