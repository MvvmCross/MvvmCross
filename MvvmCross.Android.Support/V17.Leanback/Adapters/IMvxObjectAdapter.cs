// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using MvvmCross.Binding.Attributes;

namespace MvvmCross.Droid.Support.V17.Leanback.Adapters
{
    public interface IMvxObjectAdapter
    {
        [MvxSetToNullAfterBinding]
        IEnumerable ItemsSource { get; set; }
    }
}