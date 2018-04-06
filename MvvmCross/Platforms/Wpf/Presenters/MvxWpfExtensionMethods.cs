using System;
using System.Linq;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;

namespace MvvmCross.Platforms.Wpf.Presenters
{
    public static class MvxWpfExtensionMethods
    {
        public static bool HasRegionAttribute(this Type view)
        {
            var attributes = view
                .GetCustomAttributes(typeof(MvxRegionAttribute), true);

            return attributes.Any();
        }

        public static string GetRegionName(this Type view)
        {
            var attributes = view
                .GetCustomAttributes(typeof(MvxRegionAttribute), true);

            if (!attributes.Any())
            {
                throw new InvalidOperationException("The MvxWpfView has no region attribute");
            }

            return ((MvxRegionAttribute)attributes.First()).Name;
        }
    }
}
