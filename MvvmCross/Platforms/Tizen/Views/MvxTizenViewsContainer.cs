// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using MvvmCross.Exceptions;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Tizen.Views
{
    public class MvxTizenViewsContainer
        : MvxViewsContainer
        , IMvxTizenViewsContainer
    {
        public MvxViewModelRequest CurrentRequest { get; private set; }
    }
}
