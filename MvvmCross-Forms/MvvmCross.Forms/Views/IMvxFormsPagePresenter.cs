﻿using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Platform;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    public interface IMvxFormsPagePresenter
    {
        MvxFormsApplication FormsApplication { get; set; }

        IMvxViewModelLoader ViewModelLoader { get; set; }

        IMvxViewsContainer ViewsContainer { get; set; }

        IMvxViewModelTypeFinder ViewModelTypeFinder { get; set; }

        Page CreatePage(Type viewType, MvxViewModelRequest request, MvxBasePresentationAttribute attribute);

        Func<bool> ClosePlatformViews { get; set; }
    }
}