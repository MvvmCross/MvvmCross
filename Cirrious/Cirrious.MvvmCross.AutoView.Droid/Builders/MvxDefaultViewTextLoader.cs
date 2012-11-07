using Cirrious.MvvmCross.AutoView.Droid.Interfaces;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Plugins.ResourceLoader;

namespace Cirrious.MvvmCross.AutoView.Droid.Builders
{
    public class MvxDefaultViewTextLoader : IMvxServiceConsumer, IMvxDefaultViewTextLoader
    {
        public bool HasDefinition(string viewType, string key)
        {
            var service = this.GetService<IMvxResourceLoader>();
            var path = PathForView(viewType, key);
            return service.ResourceExists(path);
        }

        public string GetDefinition(string viewType, string key)
        {
            var service = this.GetService<IMvxResourceLoader>();
            var path = PathForView(viewType, key);
            try
            {
                return service.GetTextResource(path);
            }
            catch (MvxException)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning, "Definition file not loaded {0}", path);
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