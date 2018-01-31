// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace MvvmCross.Core.Views
{
    public class MvxPresentationAttributeAction
    {
        public Action<Type, MvxBasePresentationAttribute, MvxViewModelRequest> ShowAction { get; set; }

        public Func<IMvxViewModel, MvxBasePresentationAttribute, bool> CloseAction { get; set; }
    }
}
