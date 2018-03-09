using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Platform.Ios.Core;
using MvvmCross.Platform.Ios.Core;
using MvvmCross.Plugin.Json;
using MvvmCross.ViewModels;
using Playground.Forms.UI;
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

        protected override Xamarin.Forms.Application CreateFormsApplication() => new FormsApp();

        protected override IMvxApplication CreateApp() => new Core.App();
    }
}
