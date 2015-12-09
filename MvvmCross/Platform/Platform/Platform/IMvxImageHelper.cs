// IMvxImageHelper.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Platform
{
    using System;

    using MvvmCross.Platform.Core;

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