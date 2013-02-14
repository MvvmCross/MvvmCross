// MvxDialogActivityView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CrossUI.Droid.Dialog;

namespace Cirrious.MvvmCross.Dialog.Droid.Views
{
    public abstract partial class MvxDialogActivityView<TViewModel> : DialogActivity
    {
        protected MvxDialogActivityView()
        {
            IsVisible = true;
        }
    }
}