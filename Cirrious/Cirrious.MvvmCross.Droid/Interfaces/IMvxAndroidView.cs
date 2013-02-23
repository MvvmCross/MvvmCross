// IMvxAndroidView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Droid.Interfaces
{
    public interface IMvxAndroidView
        : IMvxView
        , IMvxBindingContextOwner
        , IMvxServiceConsumer
    {
        void MvxInternalStartActivityForResult(Intent intent, int requestCode);
        new bool IsVisible { get; set; }
    }
}