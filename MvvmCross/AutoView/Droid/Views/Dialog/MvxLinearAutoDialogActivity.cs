// MvxAutoDialogActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Droid.Views.Dialog
{
    using Android.App;
    using Android.Views;

    using CrossUI.Core.Elements.Menu;
    using CrossUI.Droid.Dialog.Elements;

    using MvvmCross.AutoView.Droid.ExtensionMethods;
    using MvvmCross.AutoView.Droid.Interfaces;
    using MvvmCross.AutoView.ExtensionMethods;
    using MvvmCross.Platform.IoC;

    [Activity(Name = "cirrious.mvvmcross.autoview.droid.views.dialog.MvxLinearAutoDialogActivity")]
    [MvxUnconventional]
    public class MvxLinearAutoDialogActivity
        : MvxLinearDialogActivity
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
            using (new MvxBindingContextStackRegistration<IMvxAndroidBindingContext>((IMvxAndroidBindingContext)BindingContext))
            {
                Root = this.LoadDialogRoot<Element, RootElement>();
                this._parentMenu = this.LoadMenu();
            }
        }

        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            return this.CreateOptionsMenu(this._parentMenu, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (this._parentMenu.ProcessMenuItemSelected(item))
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}