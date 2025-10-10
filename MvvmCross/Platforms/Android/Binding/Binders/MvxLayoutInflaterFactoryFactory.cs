// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.Platforms.Android.Binding.Binders
{
    public class MvxLayoutInflaterFactoryFactory
        : IMvxLayoutInflaterHolderFactoryFactory
    {
        [RequiresUnreferencedCode("This method creates bindings which use reflection and may not be preserved by trimming.")]
        public IMvxLayoutInflaterHolderFactory Create(object source)
        {
            return new MvxBindingLayoutInflaterFactory(source);
        }
    }
}
