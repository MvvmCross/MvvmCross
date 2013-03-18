// IMvxAndroidBindingContext.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Interfaces.BindingContext;

namespace Cirrious.MvvmCross.Binding.Droid.Interfaces.BindingContext
{
    public interface IMvxAndroidBindingContext
        : IMvxBindingContext
    {
        IMvxLayoutInflater LayoutInflater { get; }
        View BindingInflate(int resourceId, ViewGroup viewGroup);
    }
}