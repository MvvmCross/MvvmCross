// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Xamarin.Forms;

namespace MvvmCross.Forms.Presenters
{
    public interface IMvxFormsPagePresenter : IMvxAttributeViewPresenter
    {
        Application FormsApplication { get; set; }

        IMvxViewModelLoader ViewModelLoader { get; set; }

        ValueTask<Page> CreatePage(Type viewType, MvxViewModelRequest request, MvxBasePresentationAttribute attribute);

        IEnumerable<Page> CurrentPageTree { get; }

        NavigationPage TopNavigationPage(Page? rootPage = null);
    }
}
