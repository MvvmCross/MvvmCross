// MvxEmbeddedAutoViewTextLoader.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Builders
{
    using System;
    using System.IO;
    using System.Reflection;

    using MvvmCross.AutoView.Interfaces;

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