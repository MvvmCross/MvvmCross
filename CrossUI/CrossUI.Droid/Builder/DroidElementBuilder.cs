using System;
using CrossUI.Core.Builder;
using FooBar.Dialog.Droid.Elements;

namespace FooBar.Dialog.Droid.Builder
{
    public class DroidElementBuilder : TypedUserInterfaceBuilder
    {
        public DroidElementBuilder(bool registerDefaults) 
            : base(typeof(Element), "Element", "String")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}