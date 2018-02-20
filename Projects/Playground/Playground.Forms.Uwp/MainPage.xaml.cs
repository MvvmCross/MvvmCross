﻿using MvvmCross;
using MvvmCross.Forms.Platform.Uap.Presenters;
using MvvmCross.Forms.Platform.Uap.Views;
using MvvmCross.Forms.Presenters;
using MvvmCross.Forms.Views;
using MvvmCross.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Playground.Forms.Uwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public MainPage()
        {
            // This is required so that navigating to a native page and back again doesn't
            // reload XF
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;

            this.InitializeComponent();

            InitializeComponent();

            var start = Mvx.Resolve<IMvxAppStart>();
            start.Start();

            var presenter = Mvx.Resolve<IMvxFormsViewPresenter>() as MvxFormsUwpViewPresenter;
            LoadApplication(presenter.FormsApplication);
        }
    }
}
