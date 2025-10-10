// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.Binding.Bindings;

public abstract class MvxBinding : IMvxBinding
{
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Bindings inherently use reflection. This is by design and callers are warned through derived class usage.")]
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
    protected virtual void Dispose(bool isDisposing)
    {
        // nothing to do in this base class
    }
}
