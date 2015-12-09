// IMvxInteraction.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    using System;

    using MvvmCross.Platform.Core;

    public interface IMvxInteraction
    {
        event EventHandler Requested;
    }

    public interface IMvxInteraction<T>
    {
        event EventHandler<MvxValueEventArgs<T>> Requested;
    }
}