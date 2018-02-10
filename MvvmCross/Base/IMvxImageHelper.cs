// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Base
{
    public interface IMvxImageHelper<T>
        : IDisposable
        where T : class
    {
        string DefaultImagePath { get; set; }

        string ErrorImagePath { get; set; }

        string ImageUrl { get; set; }

        event EventHandler<MvxValueEventArgs<T>> ImageChanged;

        int MaxWidth { get; set; }

        int MaxHeight { get; set; }
    }
}
