// MvxMissingActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Droid.Views
{
    using Android.App;

    using CrossUI.Droid.Dialog.Elements;

    using MvvmCross.AutoView.Droid.Interfaces;
    using MvvmCross.AutoView.ExtensionMethods;
    using MvvmCross.Platform.IoC;

    [Activity(Name = "cirrious.mvvmcross.autoview.droid.views.MvxMissingActivity")]
    [MvxUnconventional]
    public class MvxMissingActivity
        : MvxDialogActivity
          , IMvxAndroidAutoView
    {
        public new MvxViewModel ViewModel
        {
            get { return (MvxViewModel)base.ViewModel; }
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