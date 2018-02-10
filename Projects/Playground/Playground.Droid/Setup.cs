// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Logging;
using MvvmCross.Platform.Android.Presenters;
using MvvmCross.ViewModels;
using Playground.Core;
using Serilog;

namespace Playground.Droid
{
    public class Setup : MvxAppCompatSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new MvxAppCompatViewPresenter(AndroidViewAssemblies);
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
    }
}
