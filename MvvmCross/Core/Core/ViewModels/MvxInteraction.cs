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

    public class MvxInteraction : IMvxInteraction
    {
        public void Raise()
        {
            this.Requested.Raise(this);
        }

        public event EventHandler Requested;
    }

    public class MvxInteraction<T> : IMvxInteraction<T>
    {
        public void Raise(T request)
        {
            this.Requested.Raise(this, request);
        }

        public event EventHandler<MvxValueEventArgs<T>> Requested;
    }
}