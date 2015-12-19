// DroidBuilderRegistry.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CrossUI.Core.Builder;
using CrossUI.Core.Elements.Menu;
using CrossUI.Droid.Dialog.Elements;

namespace CrossUI.Droid.Builder
{
    public class DroidBuilderRegistry : BuilderRegistry
    {
        public DroidBuilderRegistry(bool registerDefaultElements = true)
        {
            this.AddBuilder(typeof(Element), new DroidElementBuilder(registerDefaultElements));
            this.AddBuilder(typeof(Group), new DroidGroupBuilder(registerDefaultElements));
            this.AddBuilder(typeof(Section), new DroidSectionBuilder(registerDefaultElements));
            this.AddBuilder(typeof(IMenu), new DroidMenuBuilder(registerDefaultElements));
        }
    }
}