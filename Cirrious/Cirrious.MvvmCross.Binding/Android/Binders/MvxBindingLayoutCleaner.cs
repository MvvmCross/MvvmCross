using System.Collections.Generic;
using Android.Views;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Android.Binders
{
    public class MvxBindingLayoutCleaner
    {
        public void Clean(View view)
        {
            if (view == null)
                return;

            var bindings = (MvxJavaContainer<Dictionary<View, IList<IMvxUpdateableBinding>>>)view.GetTag(MvxAndroidBindingResource.Instance.BindingTagUnique);
            if (bindings == null)
                return;

            var dictionary = bindings.Object;
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