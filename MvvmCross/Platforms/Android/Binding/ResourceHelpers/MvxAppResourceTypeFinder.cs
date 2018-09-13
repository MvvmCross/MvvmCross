// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reflection;
using MvvmCross.Exceptions;

namespace MvvmCross.Platforms.Android.Binding.ResourceHelpers
{
    public class MvxAppResourceTypeFinder : IMvxAppResourceTypeFinder
    {
        private Type FindResourceType(Assembly assembly)
        {
            var att =
                assembly.CustomAttributes.FirstOrDefault(ca =>
                    ca.AttributeType.FullName == "Android.Runtime.ResourceDesignerAttribute");

            if (att != null)
            {
                var typeName = (string)att.ConstructorArguments.First().Value;
                return assembly.GetType(typeName);
            }
            return null;
        }

        public Type Find()
        {
            var setup = Mvx.IoCProvider.Resolve<IMvxAndroidGlobals>();
            var resourceType = FindResourceType(setup.ExecutableAssembly);
            if (resourceType == null)
                throw new MvxException("Unable to find resource type. Please check if your setup class is in your application's root namespace.");
            return resourceType;
        }
    }
}
