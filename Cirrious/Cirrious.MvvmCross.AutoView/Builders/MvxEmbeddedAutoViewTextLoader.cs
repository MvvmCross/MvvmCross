using System;
using System.IO;
using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.AutoView.Builders
{
    public class MvxEmbeddedAutoViewTextLoader : IMvxServiceConsumer, IMvxAutoViewTextLoader
    {
        public bool HasDefinition(Type viewModelType, string key)
        {
            var path = PathForView(viewModelType, key);
            //foreach (var n in viewModelType.Assembly.GetManifestResourceNames())
            //{
            //    MvxTrace.Trace("Name : {0}", n);
            //}
            var stream = viewModelType.Assembly.GetManifestResourceStream(path);
            if (stream == null)
                return false;
            return true;
        }

        public string GetDefinition(Type viewModelType, string key)
        {
            var path = PathForView(viewModelType, key);

            using (var stream = viewModelType.Assembly.GetManifestResourceStream(path))
            {
                if (stream == null)
                    return null;

                using (var streamReader = new StreamReader(stream))
                    return streamReader.ReadToEnd();
            }
        }

        private static string PathForView(Type viewModelType, string key)
        {
            var path = string.Format("{0}.AutoViews.{1}.{2}.json", viewModelType.Namespace, viewModelType.Name, key);
            return path;
        }
    }
}