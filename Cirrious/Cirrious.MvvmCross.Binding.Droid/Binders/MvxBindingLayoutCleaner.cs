#region Copyright

// <copyright file="MvxBindingLayoutCleaner.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System.Collections.Generic;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Droid.Binders
{
    public class MvxBindingLayoutCleaner
    {
        public void Clean(View view)
        {
            if (view == null)
                return;

            IDictionary<View, IList<IMvxUpdateableBinding>> dictionary;
            if (!view.TryGetStoredBindings(out dictionary))
            {
                return;
            }

            foreach (var bindingPair in dictionary)
            {
                foreach (var binding in bindingPair.Value)
                {
                    binding.Dispose();
                }
            }
            dictionary.Clear();
        }
    }
}