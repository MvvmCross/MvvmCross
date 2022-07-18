// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Android.Binding.Binders
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
