using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;

namespace PageRendererExample
{
    public class CoreApp : MvxApplication
    {
        public CoreApp()
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