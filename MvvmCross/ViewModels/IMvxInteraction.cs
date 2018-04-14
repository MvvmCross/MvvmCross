// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Base;

namespace MvvmCross.ViewModels
{
    public interface IMvxInteraction
    {
        event EventHandler Requested;
    }

    public interface IMvxInteraction<T>
    {
        event EventHandler<MvxValueEventArgs<T>> Requested;
    }
}