// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Platform.Android.Core;
using MvvmCross.Logging;
using MvvmCross.Plugin.Json;
using MvvmCross.ViewModels;
using Playground.Forms.UI;
using Xamarin.Forms;

namespace Playground.Forms.Droid
{
    public class Setup : MvxFormsAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override MvxLogProviderType GetDefaultLogProviderType()
            => MvxLogProviderType.Serilog;

        protected override IMvxLogProvider CreateLogProvider()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.AndroidLog()
                .CreateLogger();
            return base.CreateLogProvider();
        }

        protected override IEnumerable<Assembly> GetViewAssemblies()
        {
            return new List<Assembly>(base.GetViewAssemblies().Union(new[] { typeof(FormsApp).GetTypeInfo().Assembly }));
        }

        protected override Application CreateFormsApplication() => new FormsApp();

        protected override IMvxApplication CreateApp() => new Core.App();
    }
}
