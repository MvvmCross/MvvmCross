// IMvxFragmentView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Droid.Fragging.Fragments
{
    public interface IMvxFragmentView
        : IMvxBindingContextOwner
          , IMvxDataConsumer
    {
        IMvxViewModel ViewModel { get; set; }
    }
}