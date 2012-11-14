using Android.App;
using Android.Views;
using Cirrious.MvvmCross.AutoView.Droid.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces;
using Cirrious.MvvmCross.Dialog.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views.Attributes;
using Foobar.Dialog.Core.Menus;

namespace Cirrious.MvvmCross.AutoView.Droid.Views.Dialog
{
    [Activity]
    [MvxUnconventionalView]
    public class MvxAutoDialogActivityView 
        : MvxBindingDialogActivityView<MvxViewModel>
        , IMvxAndroidAutoView<MvxViewModel>
    {
        private IParentMenu _parentMenu;

        protected override void OnViewModelSet()
        {
            Root = this.LoadDialogRoot();
            _parentMenu = this.LoadMenu();
        }

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