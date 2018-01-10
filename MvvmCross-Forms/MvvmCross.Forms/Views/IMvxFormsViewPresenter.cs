using System;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Platform;

namespace MvvmCross.Forms.Views
{
    public interface IMvxFormsViewPresenter : IMvxAttributeViewPresenter
    {
        MvxFormsApplication FormsApplication { get; set; }
        IMvxFormsPagePresenter FormsPagePresenter { get; set; }

        bool ClosePlatformViews();
        bool ShowPlatformHost(Type hostViewModel = null);
    }
}