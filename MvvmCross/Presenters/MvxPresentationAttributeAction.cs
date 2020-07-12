// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;

namespace MvvmCross.Presenters
{
    public class MvxPresentationAttributeAction
    {
        public Func<Type, IMvxPresentationAttribute, MvxViewModelRequest, ValueTask<bool>>? ShowAction { get; set; }

        public Func<IMvxViewModel, IMvxPresentationAttribute, ValueTask<bool>>? CloseAction { get; set; }
    }
}
