// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.IoC;
using MvvmCross.Logging;

namespace MvvmCross.Base
{
    public class MvxBootstrapRunner
    {
        public virtual void Run(Assembly assembly)
        {
            var types = assembly.CreatableTypes()
                                .Inherits<IMvxBootstrapAction>();

            foreach (var type in types)
            {
                Run(type);
            }
        }

        protected virtual void Run(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            try
            {
                var toRun = Activator.CreateInstance(type);
                if (toRun is not IMvxBootstrapAction bootstrapAction)
                {
                    MvxLogHost.Default?.Log(LogLevel.Trace,
                        "Could not run startup task {TypeName} - it's not a startup task", type.Name);
                    return;
                }

                bootstrapAction.Run();
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                // pokemon handling
                MvxLogHost.Default?.Log(LogLevel.Trace, exception, "Error running startup task {TypeName}", type.Name);
            }
        }
    }
}
