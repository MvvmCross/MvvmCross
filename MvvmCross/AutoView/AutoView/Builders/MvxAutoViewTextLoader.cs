// MvxAutoViewTextLoader.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Builders
{
    public class MvxAutoViewTextLoader : MvxCompositeAutoViewTextLoader
    {
        public MvxAutoViewTextLoader()
        {
            // note - order is important here...
            this.Add(new MvxResourceAutoViewTextLoader());
            this.Add(new MvxEmbeddedAutoViewTextLoader());
        }
    }
}