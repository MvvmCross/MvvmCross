// MvxLayoutInflaterFactoryFactory.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Binders
{
    public class MvxLayoutInflaterFactoryFactory
        : IMvxLayoutInflaterHolderFactoryFactory
    {
        public IMvxLayoutInflaterHolderFactory Create(object bindingSource)
        {
            return new MvxBindingLayoutInflaterFactory(bindingSource);
        }
    }
}