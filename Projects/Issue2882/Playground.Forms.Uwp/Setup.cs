using MvvmCross.Forms.Platforms.Uap.Core;
using MvvmCross.Platforms.Uap.Views.Suspension;
using Playground.Forms.UI;

namespace Playground.Forms.Uwp
{
    public class Setup : MvxFormsWindowsSetup<Core.App, FormsApp>
    {
        protected override IMvxSuspensionManager CreateSuspensionManager()
        {
            return new CustomMvxSuspensionManager();
        }
    }
}
