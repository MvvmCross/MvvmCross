// IMvxBindingContext.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Views;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Droid.Interfaces.Views
{
    public interface IMvxBindingContext
    {
        void RegisterBindingsFor(View view);
        void RegisterBinding(IMvxBinding binding);
        void ClearBindings(View view);
        void ClearAllBindings();
        View BindingInflate(int resourceId, ViewGroup viewGroup);
        View BindingInflate(object source, int resourceId, ViewGroup viewGroup);
    }
}