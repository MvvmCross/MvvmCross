// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.IoC
{
    public interface IMvxPropertyInjector
    {
        void Inject<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] TTarget>(
            TTarget target, IMvxPropertyInjectorOptions options = null);
    }
}
