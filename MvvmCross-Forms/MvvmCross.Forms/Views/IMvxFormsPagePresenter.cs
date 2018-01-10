using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Platform;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    public interface IMvxFormsPagePresenter: IMvxAttributeViewPresenter
    {
        MvxFormsApplication FormsApplication { get; set; }

        IMvxViewModelLoader ViewModelLoader { get; set; }

        Page CreatePage(Type viewType, MvxViewModelRequest request, MvxBasePresentationAttribute attribute);

        Func<bool> ClosePlatformViews { get; set; }

        // This method allows each platform to create attributes for native views
        Func<Type,Type, MvxBasePresentationAttribute> PlatformCreatePresentationAttribute { get; set; }

        Func<Type, bool> ShowPlatformHost { get; set; }
    }
}