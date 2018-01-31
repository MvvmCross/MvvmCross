// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Platform.Core;

namespace MvvmCross.Core.ViewModels
{
    public class MvxInteraction : IMvxInteraction
    {
        public void Raise()
        {
            Requested.Raise(this);
        }

        public event EventHandler Requested;
    }

    public class MvxInteraction<T> : IMvxInteraction<T>
    {
        public void Raise(T request)
        {
            Requested.Raise(this, request);
        }

        public event EventHandler<MvxValueEventArgs<T>> Requested;
    }
}