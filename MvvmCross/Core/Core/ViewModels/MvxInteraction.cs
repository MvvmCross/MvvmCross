// IMvxInteraction.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Platform.Core;

namespace MvvmCross.Core.ViewModels
{
    public class MvxInteraction : IMvxInteraction
    {
        public event EventHandler Requested;

        public void Raise()
        {
            Requested.Raise(this);
        }
    }

    public class MvxInteraction<T> : IMvxInteraction<T>
    {
        public event EventHandler<MvxValueEventArgs<T>> Requested;

        public void Raise(T request)
        {
            Requested.Raise(this, request);
        }
    }
}