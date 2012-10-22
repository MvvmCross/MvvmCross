using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Plugins.ResourceLoader;

namespace CustomerManagement.Droid
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
            return service.GetTextResource(path);
        }

        private static string PathForView(string viewType, string key)
        {
            var path = string.Format("DefaultViews/{0}/{1}.json", viewType, key);
            return path;
        }
    }
}