// MvxBindingLayoutCleaner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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