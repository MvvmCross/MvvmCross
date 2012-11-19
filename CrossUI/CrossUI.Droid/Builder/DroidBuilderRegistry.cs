using CrossUI.Core.Builder;
using CrossUI.Core.Elements.Menu;
using FooBar.Dialog.Droid.Elements;

namespace FooBar.Dialog.Droid.Builder
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