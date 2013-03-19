// IMvxAndroidView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Droid.Views
{
    public interface IMvxAndroidView
        : IMvxView
          , IMvxLayoutInflater
          , IMvxBindingContextOwner
    {
        void MvxInternalStartActivityForResult(Intent intent, int requestCode);
    }
}