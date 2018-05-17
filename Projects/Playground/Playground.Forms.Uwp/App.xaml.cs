using System;
using System.Diagnostics;
using MvvmCross.Core;
using MvvmCross.Forms.Platforms.Uap.Core;
using MvvmCross.Forms.Platforms.Uap.Views;
using MvvmCross.Forms.Presenters;
using MvvmCross.Platforms.Uap.Core;
using MvvmCross.ViewModels;
using Playground.Forms.UI;
using MvvmCross;

namespace Playground.Forms.Uwp
{
    sealed partial class App
    {
        public App()
        {
            InitializeComponent();
        }
    }

    public abstract class PlaygroundApp : MvxWindowsApplication<CustomMvxFormsWindowsSetup, Core.App, FormsApp, MainPage>
    {
        
    }

    public class CustomMvxFormsWindowsSetup: MvxFormsWindowsSetup<Core.App, FormsApp>
    {
        protected override void InitializeIoC()
        {
            base.InitializeIoC();

            Mvx.LazyConstructAndRegisterSingleton<IMvxFormsPagePresenter, CustomFormsPagePresenter>();
        }
    }

    public class CustomFormsPagePresenter: MvxFormsPagePresenter
    {
        public override void Show(MvxViewModelRequest request)
        {
            Debug.WriteLine("Customer presenter: Show");
            base.Show(request);
        }
    }
}
