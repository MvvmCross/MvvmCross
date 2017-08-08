// MvxAppResourceTypeFinder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid;
using MvvmCross.Platform.Exceptions;

namespace MvvmCross.Binding.Droid.ResourceHelpers
{
    public class MvxAppResourceTypeFinder : IMvxAppResourceTypeFinder
    {
        public Type Find()
        {
            var setup = Mvx.Resolve<IMvxAndroidGlobals>();
            var resourceTypeName = setup.ExecutableNamespace + ".Resource";
            var resourceType = setup.ExecutableAssembly.GetType(resourceTypeName);
            if (resourceType == null)
                throw new MvxException("Unable to find resource type - " + resourceTypeName +
                                       ". Please check if your setup class is in your application's root namespace.");
            return resourceType;
        }
    }
}
