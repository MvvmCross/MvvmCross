using Cirrious.MvvmCross.AutoView.Touch.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Touch.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Views.Attributes;
using CrossUI.Core.Elements.Menu;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.AutoView.Touch.Views.Dialog
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
			_parentMenu = this.LoadMenu();

			if (_parentMenu != null) {
				NavigationItem.SetRightBarButtonItem (
					new UIBarButtonItem (UIBarButtonSystemItem.Action, 
				                     (sender, e) => { ShowActionMenu (); }),
					false);
			}
        }

		private void ShowActionMenu ()
		{
			this.ShowOptionsMenu(_parentMenu);
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