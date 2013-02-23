// MvxMissingActivityView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces;
using Cirrious.MvvmCross.AutoView.ExtensionMethods;
using Cirrious.MvvmCross.Dialog.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views.Attributes;
using CrossUI.Droid.Dialog.Elements;

namespace Cirrious.MvvmCross.AutoView.Droid.Views
{
    [Activity]
    [MvxUnconventionalView]
    public class MvxMissingActivityView
        : MvxDialogActivityView
          , IMvxAndroidAutoView
    {
        public new MvxViewModel ViewModel
        {
            get { return (MvxViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            var description = this.ViewModel.CreateMissingDialogDescription();
            var root = this.LoadDialogRoot<Element, RootElement>(description);
            Root = root;
        }
    }
}