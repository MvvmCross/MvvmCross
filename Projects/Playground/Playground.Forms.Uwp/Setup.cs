using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.Forms.Platform.Uap.Core;
using MvvmCross.ViewModels;
using Playground.Forms.UI;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;

namespace Playground.Forms.Uwp
{
    public class Setup : MvxFormsWindowsSetup
    {
        protected Setup(Frame rootFrame, IActivatedEventArgs e) : base(rootFrame, e)
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
