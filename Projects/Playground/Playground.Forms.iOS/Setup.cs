using MvvmCross;
using MvvmCross.Forms.Platforms.Ios.Core;
using MvvmCross.Forms.Presenters;
using Playground.Core;
using Playground.Forms.UI;

namespace Playground.Forms.iOS
{
    public class Setup : MvxFormsIosSetup<Core.App, FormsApp>
    {
        protected override IMvxFormsPagePresenter CreateFormsPagePresenter(IMvxFormsViewPresenter viewPresenter)
        {
            // Workaround/fix for: https://github.com/MvvmCross/MvvmCross/issues/2502
            var formsPagePresenter = new CustomMvxFormsPagePresenter(viewPresenter);
            Mvx.IoCProvider.RegisterSingleton<IMvxFormsPagePresenter>(formsPagePresenter);
            return formsPagePresenter;
        }
    }
}
