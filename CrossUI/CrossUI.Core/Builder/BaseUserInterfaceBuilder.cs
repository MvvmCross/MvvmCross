// BaseUserInterfaceBuilder.cs
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CrossUI.Core.Descriptions;
using System.Collections.Generic;
using System.Linq;

namespace CrossUI.Core.Builder
{
    public abstract class BaseUserInterfaceBuilder
    {
        private readonly Dictionary<string, bool> _platformTags;

        protected abstract IPropertyBuilder PropertyBuilder { get; }

        protected BaseUserInterfaceBuilder(string platformName)
        {
            _platformTags = new Dictionary<string, bool>();
            AddPlatformName(platformName);
        }

        public void AddPlatformName(string tag)
        {
            _platformTags[tag] = true;
        }

        protected virtual void FillProperties(object target, Dictionary<string, object> propertyDescriptions)
        {
            PropertyBuilder.FillProperties(target, propertyDescriptions);
        }

        protected virtual bool ShouldBuildDescription(BaseDescription description)
        {
            if (description == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(description.NotFor))
            {
                var notFor = description.NotFor.Split(';');
                if (notFor.Any(_platformTags.ContainsKey))
                {
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(description.OnlyFor))
            {
                var onlyFor = description.OnlyFor.Split(';');
                if (onlyFor.Any(_platformTags.ContainsKey))
                {
                    return true;
                }

                return false;
            }

            return true;
        }
    }
}