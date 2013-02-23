// IMvxBindingDialogActivityView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;

namespace Cirrious.MvvmCross.Dialog.Droid.Interfaces
{
    public interface IMvxBindingDialogActivityView : IMvxDroidBindingContext
    {
        object DefaultBindingSource { get; }
    }
}