using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Platform;

namespace MvvmCross.Core.Views
{
    public static class MvxPresentationAttributeExtensions
    {
        public static bool HasBasePresentationAttribute(this Type candidateType)
        {
            var attributes = candidateType.GetCustomAttributes(typeof(MvxBasePresentationAttribute), true);
            return attributes.Length > 0;
        }

        public static IEnumerable<MvxBasePresentationAttribute> GetBasePresentationAttributes(this Type fromViewType)
        {
            var attributes = fromViewType.GetCustomAttributes(typeof(MvxBasePresentationAttribute), true);

            if (!attributes.Any())
                throw new InvalidOperationException($"Type does not have {nameof(MvxBasePresentationAttribute)} attribute!");

            return attributes.Cast<MvxBasePresentationAttribute>();
        }

        public static MvxBasePresentationAttribute GetBasePresentationAttribute(this Type fromViewType)
        {
            return fromViewType.GetBasePresentationAttributes().FirstOrDefault();
        }

        public static Type GetViewModelType(this Type viewType)
        {
            if (!viewType.HasBasePresentationAttribute())
                return null;

            return viewType.GetBasePresentationAttributes()
                .Select(x => x.ViewModelType)
                .FirstOrDefault();
        }
    }
}
