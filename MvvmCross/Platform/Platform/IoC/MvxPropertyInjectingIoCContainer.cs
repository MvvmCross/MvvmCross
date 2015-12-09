// MvxPropertyInjectingIoCContainer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.IoC
{
    using System;

    [Obsolete("This functionality is now moved into MvxSimpleIoCContainer and can be enabled using MvxIocOptions")]
    public class MvxPropertyInjectingIoCContainer
        : MvxSimpleIoCContainer
    {
        public new static IMvxIoCProvider Initialize(IMvxIocOptions options)
        {
            if (Instance != null)
            {
                return Instance;
            }

            // create a new ioc container - it will register itself as the singleton
            // ReSharper disable ObjectCreationAsStatement
            new MvxPropertyInjectingIoCContainer(options);
            // ReSharper restore ObjectCreationAsStatement
            return Instance;
        }

        protected MvxPropertyInjectingIoCContainer(IMvxIocOptions options)
            : base(options ?? CreatePropertyInjectionOptions())
        {
        }

        private static MvxIocOptions CreatePropertyInjectionOptions()
        {
            return new MvxIocOptions()
            {
                TryToDetectDynamicCircularReferences = true,
                TryToDetectSingletonCircularReferences = true,
                CheckDisposeIfPropertyInjectionFails = true,
                PropertyInjectorType = typeof(MvxPropertyInjector),
                PropertyInjectorOptions = new MvxPropertyInjectorOptions()
                {
                    ThrowIfPropertyInjectionFails = false,
                    InjectIntoProperties = MvxPropertyInjection.AllInterfaceProperties
                }
            };
        }
    }
}