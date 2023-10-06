// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#if NET7_0
using System.Reflection;
using MvvmCross.Exceptions;
#endif

namespace MvvmCross.Platforms.Android.Binding.ResourceHelpers
{
    public class MvxAppResourceTypeFinder : IMvxAppResourceTypeFinder
    {
#if NET7_0
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
#endif

#if NET8_0_OR_GREATER
        public Type Find() => typeof(global::_Microsoft.Android.Resource.Designer.Resource);
#endif
    }
}
