using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Platform.iOS.Core;
using MvvmCross.Platform.Ios.Core;
using MvvmCross.Plugin.Json;
using MvvmCross.ViewModels;
using UIKit;

namespace Playground.Forms.iOS
{
    public class Setup : MvxFormsIosSetup
    {
        public Setup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        protected override IEnumerable<Assembly> GetViewAssemblies()
        {
            return new List<Assembly>(base.GetViewAssemblies().Union(new[] { typeof(FormsApp).GetTypeInfo().Assembly }));
        }

        protected override MvxFormsApplication CreateFormsApplication() => new FormsApp();

        protected override IMvxApplication CreateApp() => new Core.App();

        protected override void PerformBootstrapActions()
        {
            base.PerformBootstrapActions();

            PluginLoader.Instance.EnsureLoaded();
        }
    }
}
