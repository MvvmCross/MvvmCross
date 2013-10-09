// IMvxInteraction.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Core;

namespace Cirrious.MvvmCross.ViewModels
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