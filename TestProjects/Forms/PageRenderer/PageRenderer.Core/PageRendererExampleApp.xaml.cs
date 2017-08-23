﻿
using MvvmCross.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Core;

namespace PageRendererExample
{
    public partial class PageRendererExampleApp : MvxFormsApplication
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