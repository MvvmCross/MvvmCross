// MvxResourceAutoViewTextLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.AutoView.Interfaces;

namespace Cirrious.MvvmCross.AutoView.Builders
{
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
                MvxTrace.Warning( "Definition file not loaded {0}", path);
                return null;
            }
        }

        private static string PathForView(string viewType, string key)
        {
            var path = string.Format("DefaultViews/{0}/{1}.json", viewType, key);
            return path;
        }
    }
}