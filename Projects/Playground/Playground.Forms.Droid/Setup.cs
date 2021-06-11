// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using MvvmCross.Forms.Platforms.Android.Core;
using MvvmCross.IoC;
using MvvmCross.Logging;
using Playground.Forms.UI;
using Serilog;

namespace Playground.Forms.Droid
{
    public class Setup : MvxFormsAndroidSetup<Core.App, FormsApp>
    {
        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.AndroidLog()
                .CreateLogger();

            return new SerilogLoggerFactory();
        }
    }
}
