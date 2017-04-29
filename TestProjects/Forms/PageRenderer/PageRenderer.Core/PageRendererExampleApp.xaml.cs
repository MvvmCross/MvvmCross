﻿
using MvvmCross.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Core;

namespace PageRendererExample
{
    public partial class PageRendererExampleApp : MvxFormsApp
    {
        public PageRendererExampleApp()
        {
            InitializeComponent();
        }

        protected override void OnStart()
        {
            var startUp = Mvx.Resolve<IMvxAppStart>();
            startUp.Start();
        }
    }
}

