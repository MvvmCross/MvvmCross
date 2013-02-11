// IMvxBindingActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com
using Android.Views;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Droid.Interfaces.Views
{
    public interface IMvxBindingActivity
    {
        // TODO: Add the following
//        IMvxViewBindingManager BindingManager { get; }

        [System.Obsolete]
        void ClearBindings (View view);

        [System.Obsolete]
        View BindingInflate (object source, int resourceId, ViewGroup viewGroup);

        [System.Obsolete]
        View BindingInflate (int resourceId, ViewGroup viewGroup);

        [System.Obsolete]
        View NonBindingInflate (int resourceId, ViewGroup viewGroup);

        [System.Obsolete]
        void RegisterBindingsFor (View view);

        [System.Obsolete]
        void RegisterBinding (IMvxBinding binding);
    }
}