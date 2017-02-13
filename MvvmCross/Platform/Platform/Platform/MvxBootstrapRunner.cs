// MvxBootstrapRunner.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;

using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.IoC;

namespace MvvmCross.Platform.Platform
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
            try
            {
                var toRun = Activator.CreateInstance(type);
                var bootstrapAction = toRun as IMvxBootstrapAction;
                if (bootstrapAction == null)
                {
                    Mvx.Warning("Could not run startup task {0} - it's not a startup task", type.Name);
                    return;
                }

                bootstrapAction.Run();
            }
            catch (Exception exception)
            {
                // pokemon handling
                Mvx.Warning("Error running startup task {0} - error {1}", type.Name, exception.ToLongString());
            }
        }
    }
}