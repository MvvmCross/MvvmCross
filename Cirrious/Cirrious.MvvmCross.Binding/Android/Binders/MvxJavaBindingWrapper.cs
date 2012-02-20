using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Android.Binders
{
    public class MvxJavaBindingWrapper : Java.Lang.Object
    {
        public IList<IMvxUpdateableBinding> Bindings { get; private set; }

        public MvxJavaBindingWrapper(IEnumerable<IMvxUpdateableBinding> bindings)
        {
            Bindings = bindings.ToList();
        }
    }
}