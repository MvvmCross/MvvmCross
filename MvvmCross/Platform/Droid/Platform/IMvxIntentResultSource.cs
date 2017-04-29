// IMvxIntentResultSource.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Platform.Droid.Views;

namespace MvvmCross.Platform.Droid.Platform
{
    public interface IMvxIntentResultSource
    {
        event EventHandler<MvxIntentResultEventArgs> Result;
    }
}