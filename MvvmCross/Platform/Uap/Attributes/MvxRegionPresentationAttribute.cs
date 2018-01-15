// MvxRegionAttribute.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.Views;

namespace MvvmCross.Uwp.Attributes
{
    public sealed class MvxRegionPresentationAttribute : MvxBasePresentationAttribute
    {
        public MvxRegionPresentationAttribute(string regionName = null)
        {
            Name = regionName;
        }

        public string Name { get; private set; }
    }
}