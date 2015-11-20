// MvxAutoViewTextLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.AutoView.Builders
{
    public class MvxAutoViewTextLoader : MvxCompositeAutoViewTextLoader
    {
        public MvxAutoViewTextLoader()
        {
            // note - order is important here...
            Add(new MvxResourceAutoViewTextLoader());
            Add(new MvxEmbeddedAutoViewTextLoader());
        }
    }
}