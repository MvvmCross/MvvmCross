// IMvxFragmentHost.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.OS;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Droid.Support.Fragging.Presenter
{
    public interface IMvxFragmentHost
    {
        bool Show(MvxViewModelRequest request, Bundle bundle);
    }
}