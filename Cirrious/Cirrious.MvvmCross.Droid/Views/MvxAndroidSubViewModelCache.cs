#region Copyright
// <copyright file="MvxAndroidSubViewModelCache.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Collections.Generic;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxAndroidSubViewModelCache : IMvxAndroidSubViewModelCache
    {
        private static int _unique = 1;

        private readonly Dictionary<int, IMvxViewModel> _viewModels = new Dictionary<int, IMvxViewModel>();

        #region IMvxAndroidSubViewModelCache Members

        public int Cache(IMvxViewModel viewModel)
        {
            var index = _unique++;
            _viewModels[index] = viewModel;
            return index;
        }

        public IMvxViewModel Get(int index)
        {
            return _viewModels[index];
        }

        public void Remove(int index)
        {
            _viewModels.Remove(index);
        }

        #endregion
    }
}