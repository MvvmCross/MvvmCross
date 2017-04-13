using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;

namespace PageRendererExample.ViewModels
{
    public class MvvmApp : MvxApplication
    {
        public MvvmApp()
        {
            this.CreatableTypes()
                .EndingWith("Page")
                .InNamespace("PageRendererExample.Pages")
                .AsTypes()
                .RegisterAsDynamic();

            this.RegisterAppStart<ViewModels.BootViewModel>();
        }
    }
}

