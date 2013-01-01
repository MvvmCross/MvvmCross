// IMvxAndroidAutoView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.AutoView.Droid.Interfaces
{
    public interface IMvxAndroidAutoView<TViewModel>
        : IMvxAndroidView<TViewModel>
          , IMvxAutoView
          , IMvxBindingActivity
        where TViewModel : class, IMvxViewModel
    {
    }
}