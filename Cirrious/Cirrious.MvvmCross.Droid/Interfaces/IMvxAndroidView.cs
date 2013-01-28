// IMvxAndroidView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Droid.Interfaces
{
    public interface IMvxAndroidView
        : IMvxBaseAndroidView
    {
        void MvxInternalStartActivityForResult(Intent intent, int requestCode);
    }

    public interface IMvxAndroidView<TViewModel>
        : IMvxView<TViewModel>
          , IMvxAndroidView
          , IMvxServiceConsumer
        where TViewModel : class, IMvxViewModel
    {
        new TViewModel ViewModel { get; set; }
    }
}