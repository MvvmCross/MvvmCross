#region Copyright

// <copyright file="MvxAutoDialogActivityView.cs" company="Cirrious">
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
using Cirrious.MvvmCross.Dialog.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views.Attributes;
using CrossUI.Core.Elements.Menu;

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