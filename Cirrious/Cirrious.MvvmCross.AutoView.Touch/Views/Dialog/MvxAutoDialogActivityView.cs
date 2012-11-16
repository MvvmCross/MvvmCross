using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Dialog.Touch.AutoView.ExtensionMethods;
using Cirrious.MvvmCross.Dialog.Touch.AutoView.Interfaces;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Views.Attributes;
using Foobar.Dialog.Core.Menus;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.AutoView.Views.Dialog
{
    [MvxUnconventionalView]
    public class MvxAutoDialogTouchView
        : MvxTouchDialogViewController<MvxViewModel>
        , IMvxTouchAutoView<MvxViewModel>
    {
        private IParentMenu _parentMenu;

        public MvxAutoDialogTouchView(MvxShowViewModelRequest request)
            : base(request, UITableViewStyle.Grouped, null, true)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Root = this.LoadDialogRoot();
#warning Menu removed for now...
			//_parentMenu = this.LoadMenu();
        }

        /*
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
        public void RegisterBinding(IMvxUpdateableBinding binding)
        {
            Bindings.Add(binding);
        }
    }
}