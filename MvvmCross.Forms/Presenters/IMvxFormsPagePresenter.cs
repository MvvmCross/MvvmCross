﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Forms.Core;
using MvvmCross.ViewModels;
using MvvmCross.Presenters;
using Xamarin.Forms;

namespace MvvmCross.Forms.Presenters
{
    public interface IMvxFormsPagePresenter : IMvxAttributeViewPresenter
    {
        MvxFormsApplication FormsApplication { get; set; }

        IMvxViewModelLoader ViewModelLoader { get; set; }

        Page CreatePage(Type viewType, MvxViewModelRequest request, MvxBasePresentationAttribute attribute);
    }
}
