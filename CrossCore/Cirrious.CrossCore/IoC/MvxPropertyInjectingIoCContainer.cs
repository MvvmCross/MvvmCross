// MvxPropertyInjectingIoCContainer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using System.Reflection;

namespace Cirrious.CrossCore.IoC
{
    [Obsolete("This functionality is now moved into MvxSimpleIoCContainer and can be enabled using MvxIoCOptions")]
    public class MvxPropertyInjectingIoCContainer
        : MvxSimpleIoCContainer
    {
        public static new IMvxIoCProvider Initialize(MvxIoCOptions options)
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

        protected MvxPropertyInjectingIoCContainer(MvxIoCOptions options)
            : base(options)
        {
        }
    }
}