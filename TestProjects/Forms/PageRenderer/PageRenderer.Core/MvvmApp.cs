using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;

namespace PageRendererExample
{
    public class MvvmApp : MvxApplication
    {
        public MvvmApp()
        {
            CreatableTypes()
                .EndingWith("Page")
                .InNamespace("PageRendererExample.Pages")
                .AsTypes()
                .RegisterAsDynamic();

            RegisterAppStart<BootViewModel>();
        }
    }
}

