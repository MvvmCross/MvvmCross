// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Platform.Core;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views.EventSource
{
    public interface IMvxEventSourceElement
    {
        event EventHandler BindingContextChangedCalled;

        event EventHandler ParentSetCalled;
    }
}