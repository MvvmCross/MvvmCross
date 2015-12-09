// IMvxLayoutListItemView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Droid.Interfaces.Lists
{
    using MvvmCross.Platform.Core;

    public interface IMvxLayoutListItemView
        : IMvxDataConsumer
    {
        string UniqueName { get; }
    }
}