// IMvxBindingActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Android.Views;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Droid.Interfaces.Views
{
    public interface IMvxLayoutInflaterProvider
    {
        LayoutInflater LayoutInflater { get; }
    }

    public interface IMvxBindingOwner
    {
        IMvxBindingOwnerHelper BindingOwnerHelper { get; }
    }

    public interface IMvxBindingOwnerHelper : IDisposable
    {
        void RegisterBindingsFor(View view);
        void RegisterBinding(IMvxBinding binding);
        void ClearBindings(View view);
        void ClearAllBindings();
        View BindingInflate(int resourceId, ViewGroup viewGroup);
        View BindingInflate(object source, int resourceId, ViewGroup viewGroup);
    }


    public interface IMvxBindingActivity 
        : IMvxBindingOwner
        , IMvxLayoutInflaterProvider
    {
    }
}