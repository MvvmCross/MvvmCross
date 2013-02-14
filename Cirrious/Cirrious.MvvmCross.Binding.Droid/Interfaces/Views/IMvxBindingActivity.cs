// IMvxBindingActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com
using Android.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Binders;

namespace Cirrious.MvvmCross.Binding.Droid.Interfaces.Views
{
    public interface IMvxBindingActivity
    {
        IMvxViewBindingManager BindingManager { get; }
    }
}