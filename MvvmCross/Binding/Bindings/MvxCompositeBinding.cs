// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

namespace MvvmCross.Binding.Bindings
{
    public class MvxCompositeBinding : MvxBinding
    {
        private readonly List<IMvxBinding> _bindings;

        public MvxCompositeBinding(params IMvxBinding[] args)
        {
            _bindings = args.ToList();
        }

        public void Add(params IMvxBinding[] args)
        {
            _bindings.AddRange(args);
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                foreach (var mvxBinding in _bindings)
                {
                    mvxBinding.Dispose();
                }
                _bindings.Clear();
            }
            base.Dispose(isDisposing);
        }
    }
}
