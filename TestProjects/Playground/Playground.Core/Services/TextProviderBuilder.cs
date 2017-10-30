using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.Platform.IoC;
using MvvmCross.Plugins.JsonLocalization;

namespace Playground.Core.Services
{
    public class TextProviderBuilder : MvxTextProviderBuilder
    {
        public TextProviderBuilder() : base("MvxBindingsExample", "Resources", new MvxEmbeddedJsonDictionaryTextProvider(false))
        {
        }

        protected override IDictionary<string, string> ResourceFiles
        {
            get
            {
                var dictionary = GetType().GetTypeInfo()
                                     .Assembly
                                     .CreatableTypes()
                                     .Where(t => t.Name.EndsWith("ViewModel"))
                                     .ToDictionary(t => t.Name, t => t.Name);

                dictionary.Add("Text", "Text");

                return dictionary;
            }
        }
    }
}