// MvxResourceAutoViewTextLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Builders
{
    using System;

    using MvvmCross.AutoView.Interfaces;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;

    public class MvxResourceAutoViewTextLoader : IMvxAutoViewTextLoader
    {
        public bool HasDefinition(Type viewModelType, string key)
        {
            var service = Mvx.Resolve<IMvxResourceLoader>();
            var path = PathForView(viewModelType.Name, key);
            return service.ResourceExists(path);
        }

        public string GetDefinition(Type viewModelType, string key)
        {
            var service = Mvx.Resolve<IMvxResourceLoader>();
            var path = PathForView(viewModelType.Name, key);
            try
            {
                return service.GetTextResource(path);
            }
            catch (MvxException)
            {
                MvxTrace.Warning("Definition file not loaded {0}", path);
                return null;
            }
        }

        private static string PathForView(string viewType, string key)
        {
            var path = $"DefaultViews/{viewType}/{key}.json";
            return path;
        }
    }
}