#region Copyright
// <copyright file="IMvxAndroidSubViewModelCache.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Android.Interfaces
{
    public interface IMvxAndroidSubViewModelCache
    {
        int Cache(IMvxViewModel viewModel);
        IMvxViewModel Get(int index);
        void Remove(int index);
    }
}