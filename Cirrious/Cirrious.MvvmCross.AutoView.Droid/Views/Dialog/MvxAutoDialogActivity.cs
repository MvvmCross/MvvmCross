// MvxAutoDialogActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Android.Views;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.AutoView.Droid.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces;
using Cirrious.MvvmCross.AutoView.ExtensionMethods;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Dialog.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using CrossUI.Core.Elements.Menu;
using CrossUI.Droid.Dialog.Elements;

namespace Cirrious.MvvmCross.AutoView.Droid.Views.Dialog
{
    [Activity(Name = "cirrious.mvvmcross.autoview.droid.views.dialog.MvxAutoDialogActivity")]
    [MvxUnconventional]
    public class MvxAutoDialogActivity
        : MvxDialogActivity
          , IMvxAndroidAutoView
    {
        private IParentMenu _parentMenu;

        public new MvxViewModel ViewModel
        {
            get { return (MvxViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            using (
                new MvxBindingContextStackRegistration<IMvxAndroidBindingContext>((IMvxAndroidBindingContext)BindingContext)
                )
            {
                Root = this.LoadDialogRoot<Element, RootElement>();
                _parentMenu = this.LoadMenu();
            }
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