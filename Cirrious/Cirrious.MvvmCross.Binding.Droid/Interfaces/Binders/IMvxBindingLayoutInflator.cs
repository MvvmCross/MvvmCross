// IMvxBindingLayoutInflator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Views;

namespace Cirrious.MvvmCross.Binding.Droid.Interfaces.Binders
{
    public interface IMvxBindingLayoutInflator : IDisposable
    {
        View Inflate(int resource, ViewGroup id);
    }
}