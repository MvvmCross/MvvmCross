using System.ComponentModel;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.UI;

namespace Cirrious.MvvmCross.Plugins.Visibility.WindowsPhone
{
    public class MvxPhoneVisibilityDesignTimeHelper
    {
        public MvxPhoneVisibilityDesignTimeHelper()
        {
            if (!DesignerProperties.IsInDesignTool)
                return;

            if (MvxSingleton<IMvxIoCProvider>.Instance == null)
            {
                var iocProvider = MvxSimpleIoCContainer.Initialise();
                Mvx.RegisterSingleton(iocProvider);
            }

            if (Mvx.CanResolve<IMvxNativeVisibility>())
                return;

            var forceVisibilityLoaded = new Plugin();
            forceVisibilityLoaded.Load();
        }
    }
}