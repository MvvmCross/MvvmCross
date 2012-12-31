#region Copyright

// <copyright file="IMvxBindingActivity.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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