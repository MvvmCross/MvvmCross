// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using MvvmCross.IoC;

namespace MvvmCross.Plugin
{
    public interface IMvxPlugin
    {
        void Load(IMvxIoCProvider provider);
    }
}
