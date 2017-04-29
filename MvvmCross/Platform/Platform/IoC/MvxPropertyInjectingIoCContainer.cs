// MvxPropertyInjectingIoCContainer.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Platform.IoC
{
    [Obsolete("This functionality is now moved into MvxSimpleIoCContainer and can be enabled using MvxIocOptions")]
    public class MvxPropertyInjectingIoCContainer
        : MvxSimpleIoCContainer
    {
        protected MvxPropertyInjectingIoCContainer(IMvxIocOptions options)
            : base(options ?? CreatePropertyInjectionOptions())
        {
        }

        public new static IMvxIoCProvider Initialize(IMvxIocOptions options)
        {
            if (Instance != null)
                return Instance;

            // create a new ioc container - it will register itself as the singleton
            // ReSharper disable ObjectCreationAsStatement
            new MvxPropertyInjectingIoCContainer(options);
            // ReSharper restore ObjectCreationAsStatement
            return Instance;
        }

        private static MvxIocOptions CreatePropertyInjectionOptions()
        {
            return new MvxIocOptions
            {
                TryToDetectDynamicCircularReferences = true,
                TryToDetectSingletonCircularReferences = true,
                CheckDisposeIfPropertyInjectionFails = true,
                PropertyInjectorType = typeof(MvxPropertyInjector),
                PropertyInjectorOptions = new MvxPropertyInjectorOptions
                {
                    ThrowIfPropertyInjectionFails = false,
                    InjectIntoProperties = MvxPropertyInjection.AllInterfaceProperties
                }
            };
        }
    }
}