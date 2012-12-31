#region Copyright

// <copyright file="MvxMissingActivityView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Android.App;
using Cirrious.MvvmCross.AutoView.Droid.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces;
using Cirrious.MvvmCross.AutoView.ExtensionMethods;
using Cirrious.MvvmCross.Dialog.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views.Attributes;

namespace Cirrious.MvvmCross.AutoView.Droid.Views
{
    [Activity]
    [MvxUnconventionalView]
    public class MvxMissingActivityView
        : MvxBindingDialogActivityView<MvxViewModel>
          , IMvxAndroidAutoView<MvxViewModel>
    {
        protected override void OnViewModelSet()
        {
            var description = this.ViewModel.CreateMissingDialogDescription();
            var root = this.LoadDialogRoot(description);
            Root = root;
        }
    }
}