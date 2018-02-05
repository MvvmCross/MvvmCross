using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Platform;
using MvvmCross.Forms.Uwp;
using MvvmCross.Platform.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;

namespace Playground.Forms.Uwp
{
    public class Setup : MvxFormsWindowsSetup
    {
        public Setup(Frame rootFrame, LaunchActivatedEventArgs e) : base(rootFrame, e)
        {
        }

        protected override IEnumerable<Assembly> GetViewAssemblies()
        {
            return new List<Assembly>(base.GetViewAssemblies().Union(new[] { typeof(FormsApp).GetTypeInfo().Assembly }));
        }

        protected override MvxFormsApplication CreateFormsApplication() => new FormsApp();

        protected override IMvxApplication CreateApp() => new Core.App();
    }
}
