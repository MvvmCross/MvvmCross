using System;
using MvvmCross.Forms.Platform;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    public interface IMvxFormsViewPresenter
    {
        Application FormsApplication { get; set; }
        IMvxFormsPagePresenter FormsPagePresenter { get; set; }
    }
}