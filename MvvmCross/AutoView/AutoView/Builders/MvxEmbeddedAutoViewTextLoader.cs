// MvxEmbeddedAutoViewTextLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.AutoView.Interfaces;
using System;
using System.IO;
using System.Reflection;

namespace Cirrious.MvvmCross.AutoView.Builders
{
    public class MvxEmbeddedAutoViewTextLoader : IMvxAutoViewTextLoader
    {
        public bool HasDefinition(Type viewModelType, string key)
        {
            var path = PathForView(viewModelType, key);
            //foreach (var n in viewModelType.Assembly.GetManifestResourceNames())
            //{
            //    MvxTrace.Trace("Name : {0}", n);
            //}
            var stream = viewModelType.GetTypeInfo().Assembly.GetManifestResourceStream(path);
            if (stream == null)
                return false;
            return true;
        }

        public string GetDefinition(Type viewModelType, string key)
        {
            var path = PathForView(viewModelType, key);

            using (var stream = viewModelType.GetTypeInfo().Assembly.GetManifestResourceStream(path))
            {
                if (stream == null)
                    return null;

                using (var streamReader = new StreamReader(stream))
                    return streamReader.ReadToEnd();
            }
        }

        private static string PathForView(Type viewModelType, string key)
        {
            var path = $"{viewModelType.Namespace}.AutoViews.{viewModelType.Name}.{key}.json";
            return path;
        }
    }
}