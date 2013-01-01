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
        void ClearBindings(View view);
        View BindingInflate(object source, int resourceId, ViewGroup viewGroup);
        View BindingInflate(int resourceId, ViewGroup viewGroup);
        View NonBindingInflate(int resourceId, ViewGroup viewGroup);
        void RegisterBindingsFor(View view);
        void RegisterBinding(IMvxBinding binding);
    }
}